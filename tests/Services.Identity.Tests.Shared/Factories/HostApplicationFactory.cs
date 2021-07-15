using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace Services.Identity.Tests.Shared.Factories
{
    public class HostApplicationFactory<TEntryPoint>: WebApplicationFactory<TEntryPoint> 
        where TEntryPoint : class
    {
        protected override IWebHostBuilder CreateWebHostBuilder()
            => base.CreateWebHostBuilder().UseEnvironment("remote_tests");
    }
}