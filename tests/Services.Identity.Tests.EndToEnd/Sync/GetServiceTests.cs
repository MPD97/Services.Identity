using System.Net.Http;
using Services.Identity.Api;
using Services.Identity.Tests.Shared.Factories;
using Xunit;

namespace Services.Identity.Tests.EndToEnd.Sync
{
    public class GetServiceTests : IClassFixture<HostApplicationFactory<Program>>
    {
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