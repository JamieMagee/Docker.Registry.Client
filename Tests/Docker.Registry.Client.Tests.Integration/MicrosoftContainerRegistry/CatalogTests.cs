namespace Docker.Registry.Client.Tests.Integration.MicrosoftContainerRegistry
{
    using System.Threading.Tasks;
    using Xunit;

    public class CatalogTests : TestBase
    {
        [Fact]
        public async Task GetCatalogAsync()
        {
            var response = await this.client.Catalog.GetCatalogAsync();
        }
    }
}
