using Convey;
using Microsoft.AspNetCore.Builder;

namespace Services.Identity.Infrastructure
{
    public static class Extension
    {
        public static IConveyBuilder AddInfrastructure(this IConveyBuilder builder)
        {
            return builder;
        }
        
        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder builder)
        {
            return builder;
        }
    }
}