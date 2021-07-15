using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Convey.Types;
using Services.Identity.Api;
using Services.Identity.Tests.Shared.Factories;
using Services.Identity.Tests.Shared.Helpers;
using Shouldly;
using Xunit;

namespace Services.Identity.Tests.EndToEnd.Sync
{
    public class GetServiceTests : IClassFixture<HostApplicationFactory<Program>>
    {
        private Task<HttpResponseMessage> Act() => _httpClient.GetAsync($"");

        [Fact]
        public async Task get_service_endpoint_should_return_ok_status_code()
        {
            var response = await Act();

            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }
        
        [Fact]
        public async Task get_service_endpoint_should_return_content_containing_application_name()
        {
            var appOptions = OptionsHelper.GetOptions<AppOptions>("app", settingsFileName: "appsettings.json");
            var appName = appOptions.Name;
            
            var response = await Act();
            var content = await response.Content.ReadAsStringAsync();

            response.ShouldNotBeNull();
            content.ShouldContain(appName, Case.Insensitive);
        }
        
        
        #region Arrange

        private readonly HttpClient _httpClient;

        public GetServiceTests(HostApplicationFactory<Program> factory)
        {
            factory.Server.AllowSynchronousIO = true;
            _httpClient = factory.CreateClient();
        }

        #endregion
    }
}