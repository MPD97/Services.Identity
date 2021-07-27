using System;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using Services.Identity.Core.Entities;
using Services.Identity.Core.Repositories;
using Services.Identity.Infrastructure.Mongo.Documents;

namespace Services.Identity.Infrastructure.Mongo.Repositories
{
    internal sealed class MongoUserAgentRepository : IUserAgentRepository
    {
        private readonly IMongoRepository<UserAgentDocument, Guid> _repository;

        public MongoUserAgentRepository(IMongoRepository<UserAgentDocument, Guid> repository)
        {
            _repository = repository;
        }

        public Task AddAsync(UserAgent userAgent) => _repository.AddAsync(userAgent.AsDocument());
    }
}