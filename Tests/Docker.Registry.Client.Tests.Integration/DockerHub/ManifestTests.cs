namespace Docker.Registry.Client.Tests.Integration.DockerHub
{
    using System.Threading.Tasks;
    using Attributes;
    using Xunit;

    public class ManifestTests : DockerHubBaseTest
    {
        [SkipIfMissingEnvironmentVariable(Constants.DockerHubTokenEnvironmentVariable)]
        public async Task GetManifestAsync()
        {
            var result = await this.client.Manifest.GetManifestAsync("library/ubuntu", "latest");

            Assert.NotNull(result);
        }
    }
}
