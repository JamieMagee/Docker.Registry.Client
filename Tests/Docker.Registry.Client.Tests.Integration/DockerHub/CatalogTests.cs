namespace Docker.Registry.Client.Tests.Integration.DockerHub
{
    using System.Threading.Tasks;
    using Xunit;

    public class CatalogTests : TestBase
    {
        [Fact]
        public async Task GetCatalogAsync()
        {
            var exception = await Record.ExceptionAsync(() => this.client.Catalog.GetCatalogAsync());
            Assert.NotNull(exception);
        }
    }
}
