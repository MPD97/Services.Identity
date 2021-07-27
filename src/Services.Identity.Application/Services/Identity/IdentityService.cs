using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Services.Identity.Application.Commands;
using Services.Identity.Application.DTO;
using Services.Identity.Application.Events;
using Services.Identity.Core.Entities;
using Services.Identity.Core.Exceptions;
using Services.Identity.Core.Repositories;
using UAParser;
using UserAgent = Services.Identity.Core.Entities.UserAgent;

namespace Services.Identity.Application.Services.Identity
{
    public class IdentityService : IIdentityService
    {
        private static readonly Regex EmailRegex = new Regex(
            @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
            @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant,
            TimeSpan.FromMilliseconds(100));
        private static readonly Parser UAParser = Parser.GetDefault();
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly IJwtProvider _jwtProvider;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IMessageBroker _messageBroker;
        private readonly ILogger<IdentityService> _logger;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IUserAgentRepository _userAgentRepository;

        public IdentityService(IUserRepository userRepository, IPasswordService passwordService,
            IJwtProvider jwtProvider, IRefreshTokenService refreshTokenService,
            IMessageBroker messageBroker, ILogger<IdentityService> logger, IDateTimeProvider dateTimeProvider, IUserAgentRepository userAgentRepository)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _jwtProvider = jwtProvider;
            _refreshTokenService = refreshTokenService;
            _messageBroker = messageBroker;
            _logger = logger;
            _dateTimeProvider = dateTimeProvider;
            _userAgentRepository = userAgentRepository;
        }

        public async Task<UserDto> GetAsync(Guid id)
        {
            var user = await _userRepository.GetAsync(id);

            return user is null ? null : new UserDto(user);
        }

        public async Task<AuthDto> SignInAsync(SignIn command)
        {
            if (!EmailRegex.IsMatch(command.Email))
            {
                _logger.LogError($"Invalid email: {command.Email}");
                throw new InvalidEmailException(command.Email);
            }

            var user = await _userRepository.GetAsync(command.Email);
            if (user is null || !_passwordService.IsValid(user.Password, command.Password))
            {
                _logger.LogError($"User with email: {command.Email} was not found.");
                throw new InvalidCredentialsException(command.Email);
            }

            if (!_passwordService.IsValid(user.Password, command.Password))
            {
                _logger.LogError($"Invalid password for user with id: {user.Id.Value}");
                throw new InvalidCredentialsException(command.Email);
            }

            var claims = user.Permissions.Any()
                ? new Dictionary<string, IEnumerable<string>>
                {
                    ["permissions"] = user.Permissions
                }
                : null;
            var auth = _jwtProvider.Create(user.Id, user.Role, claims: claims);
            auth.RefreshToken = await _refreshTokenService.CreateAsync(user.Id);
            

            ClientInfo clientInfo = UAParser.Parse(command.UserAgent);
            var userAgent =  new UserAgent(Guid.NewGuid(), user.Id, _dateTimeProvider.Now, clientInfo.UA.Family,
                clientInfo.UA.Major, clientInfo.UA.Minor,
                clientInfo.OS.Family, clientInfo.OS.Major, clientInfo.OS.Minor,
                clientInfo.Device.Family, clientInfo.Device.Brand, clientInfo.Device.Model);
            await _userAgentRepository.AddAsync(userAgent);

            _logger.LogInformation($"User with id: {user.Id} has been authenticated.");
            await _messageBroker.PublishAsync(new SignedIn(user.Id, user.Role));
            
            return auth;
        }

        public async Task SignUpAsync(SignUp command)
        {
            if (!EmailRegex.IsMatch(command.Email))
            {
                _logger.LogError($"Invalid email: {command.Email}");
                throw new InvalidEmailException(command.Email);
            }

            var user = await _userRepository.GetAsync(command.Email);
            if (user is {})
            {
                _logger.LogError($"Email already in use: {command.Email}");
                throw new EmailInUseException(command.Email);
            }

            var role = string.IsNullOrWhiteSpace(command.Role) ? "user" : command.Role.ToLowerInvariant();
            var password = _passwordService.Hash(command.Password);
            user = new User(command.UserId, command.Email, password, role, DateTime.UtcNow, command.Permissions);
            await _userRepository.AddAsync(user);
            
            _logger.LogInformation($"Created an account for the user with id: {user.Id}.");
            await _messageBroker.PublishAsync(new SignedUp(user.Id, user.Email, user.Role));
        }
    }
}