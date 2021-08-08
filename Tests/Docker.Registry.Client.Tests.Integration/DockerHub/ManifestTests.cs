namespace Docker.Registry.Client.Tests.Integration.DockerHub
{
    using System.Threading.Tasks;
    using Attributes;
    using Models;
    using Xunit;

    public class ManifestTests : DockerHubBaseTest
    {
        [SkipIfMissingEnvironmentVariable(Constants.DockerHubTokenEnvironmentVariable)]
        public async Task DoesManifestExistAsync_Exists()
        {
            var result = await this.client.Manifest.DoesManifestExistAsync("library/ubuntu", "latest");

            Assert.True(result);
        }

        [SkipIfMissingEnvironmentVariable(Constants.DockerHubTokenEnvironmentVariable)]
        public async Task DoesManifestExistAsync_DoesNotExist()
        {
            var result = await this.client.Manifest.DoesManifestExistAsync("library/ubuntu", "this-tag-does-not-exist");

            Assert.False(result);
        }

        [SkipIfMissingEnvironmentVariable(Constants.DockerHubTokenEnvironmentVariable)]
        public async Task GetManifestAsync_ManifestList()
        {
            var result = await this.client.Manifest.GetManifestAsync("library/ubuntu", "latest");

            Assert.NotNull(result);
            Assert.Equal(typeof(ManifestList), result.Manifest.GetType());
        }

        [SkipIfMissingEnvironmentVariable(Constants.DockerHubTokenEnvironmentVariable)]
        public async Task GetManifestAsync_Manifest()
        {
            var result = await this.client.Manifest.GetManifestAsync("library/ubuntu",
                "sha256:07782849f2cff04e9bc29449c27d0fb2076e61e8bdb4475ec5dbc5386ed41a4f");

            Assert.NotNull(result);
            Assert.Equal(typeof(ImageManifest2_2), result.Manifest.GetType());
        }
    }
}
