namespace Docker.Registry.Client.Tests.Integration.DockerHub
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Xunit;

    public class BlobUploadTests : TestBase
    {
        [Fact]
        public async Task UploadBlobAsync()
        {
            const string digest = "sha256:feac5306138255e28a9862d3f3d29025d0a4d0648855afe1acd6131af07138ac";
            var blob = await this.client.Blobs.GetBlobAsync("library/ubuntu", digest);
            await using var blobStream = new MemoryStream();
            await blob.Stream.CopyToAsync(blobStream);
            blobStream.Seek(0, SeekOrigin.Begin);

            var exception = await Record.ExceptionAsync(() => this.client.BlobUploads.UploadBlobAsync(
                $"{Environment.GetEnvironmentVariable(Constants.DockerHubUsernameEnvironmentVariable)}/integrationtest",
                blobStream.Length,
                blobStream,
                digest));
            exception.Should().BeNull();
        }
    }
}
