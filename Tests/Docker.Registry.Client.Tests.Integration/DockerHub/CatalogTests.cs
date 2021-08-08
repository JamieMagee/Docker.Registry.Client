namespace Docker.Registry.Client.Tests.Integration.DockerHub
{
    using System.Threading.Tasks;
    using Attributes;
    using Xunit;

    public class CatalogTests : DockerHubBaseTest
    {
        [SkipIfMissingEnvironmentVariable(Constants.DockerHubTokenEnvironmentVariable)]
        public async Task GetCatalogAsync()
        {
            var exception = await Record.ExceptionAsync(() => this.client.Catalog.GetCatalogAsync());
            Assert.NotNull(exception);
        }
    }
}
