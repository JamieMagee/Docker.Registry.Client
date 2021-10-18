namespace Docker.Registry.Client.Tests.Integration.MicrosoftContainerRegistry
{
    using System.Threading.Tasks;
    using Docker.Registry.Client.Models;
    using FluentAssertions;
    using Xunit;

    public class ManifestTests : TestBase
    {
        [Fact]
        public async Task DoesManifestExistAsync_Exists()
        {
            var result = await this.client.Manifest.DoesManifestExistAsync("dotnet/sdk", "latest");

            result.Should().BeTrue();
        }

        [Fact]
        public async Task DoesManifestExistAsync_DoesNotExist()
        {
            var result = await this.client.Manifest.DoesManifestExistAsync("dotnet/sdk", "this-tag-does-not-exist");

            result.Should().BeFalse();
        }

        [Fact]
        public async Task GetManifestAsync_ManifestList()
        {
            var result = await this.client.Manifest.GetManifestAsync("dotnet/sdk", "latest");

            result.Should().NotBeNull();
            result.Manifest.Should().BeOfType<ManifestList>();
        }

        [Fact]
        public async Task GetManifestAsync_Manifest()
        {
            var result = await this.client.Manifest.GetManifestAsync("dotnet/sdk",
                "sha256:8257c5cbf04fd66e6e98c54178f5e02e5d6ddcf3fa38d01260306ada205e7e9e");

            result.Should().NotBeNull();
            result.Manifest.Should().BeOfType<ImageManifest2_2>();
        }
    }
}
