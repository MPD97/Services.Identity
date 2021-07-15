using System;
using System.Threading.Tasks;
using Services.Identity.Application.Commands;
using Services.Identity.Application.DTO;

namespace Services.Identity.Application.Services
{
    public interface IIdentityService
    {
        Task<UserDto> GetAsync(Guid id);
        Task<AuthDto> SignInAsync(SignIn command);
        Task SignUpAsync(SignUp command);
    }
}