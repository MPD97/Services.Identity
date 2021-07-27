using System.Threading.Tasks;
using Services.Identity.Core.Entities;

namespace Services.Identity.Core.Repositories
{
    public interface IUserAgentRepository
    {
        Task AddAsync(UserAgent userAgent);
    }
}