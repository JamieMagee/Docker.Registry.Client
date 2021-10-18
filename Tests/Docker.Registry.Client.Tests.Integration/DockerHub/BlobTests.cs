namespace Docker.Registry.Client.Tests.Integration.DockerHub
{
    using System.Threading.Tasks;
    using Xunit;

    public class BlobTests : TestBase
    {
        [Fact]
        public async Task GetBlobAsync_Exists()
        {
            var result = await this.client.Blobs.GetBlobAsync("library/ubuntu",
                "sha256:feac5306138255e28a9862d3f3d29025d0a4d0648855afe1acd6131af07138ac");

            Assert.NotNull(result);
        }
    }
}
