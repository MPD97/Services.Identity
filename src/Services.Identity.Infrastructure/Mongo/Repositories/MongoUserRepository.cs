using System;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using Services.Identity.Core.Entities;
using Services.Identity.Core.Repositories;
using Services.Identity.Infrastructure.Mongo.Documents;

namespace Services.Identity.Infrastructure.Mongo.Repositories
{
    internal sealed  class MongoUserRepository : IUserRepository
    {
        private readonly IMongoRepository<UserDocument, Guid> _repository;

        public MongoUserRepository(IMongoRepository<UserDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<User> GetAsync(Guid id)
        {
            var user = await _repository.GetAsync(id);

            return user?.AsEntity();
        }

        public async Task<User> GetAsync(string email)
        {
            var user = await _repository.GetAsync(x => x.Email == email.ToLowerInvariant());

            return user?.AsEntity();
        }

        public Task AddAsync(User user) => _repository.AddAsync(user.AsDocument());
    }
}