using System;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Services.Identity.Application.DTO;
using Services.Identity.Application.Queries;
using Services.Identity.Infrastructure.Mongo.Documents;

namespace Services.Identity.Infrastructure.Mongo.Queries.Handlers
{
    internal sealed  class GetUserHandler : IQueryHandler<GetUser, UserDto>
    {
        private readonly IMongoRepository<UserDocument, Guid> _userRepository;

        public GetUserHandler(IMongoRepository<UserDocument, Guid> userRepository)
        {
            _userRepository = userRepository;
        }
        
        public async Task<UserDto> HandleAsync(GetUser query)
        {
            var user = await _userRepository.GetAsync(query.UserId);

            return user?.AsDto();
        }
    }
}