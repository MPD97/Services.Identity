using Convey;
using Microsoft.AspNetCore.Builder;

namespace Services.Identity.Application
{
    public static class Extension
    {
        public static IConveyBuilder AddApplication(this IConveyBuilder builder)
        {
            return builder;
        }
    
        public static IApplicationBuilder UseApplication(this IApplicationBuilder builder)
        {
            return builder;
        }
    }
}