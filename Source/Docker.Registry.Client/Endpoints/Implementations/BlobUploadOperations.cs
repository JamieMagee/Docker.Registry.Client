namespace Docker.Registry.Client.Endpoints.Implementations
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;
    using Docker.Registry.Client.Helpers;
    using Docker.Registry.Client.Models;
    using Docker.Registry.Client.Registry;

    internal class BlobUploadOperations : IBlobUploadOperations
    {
        private readonly NetworkClient _client;

        internal BlobUploadOperations(NetworkClient client) => this._client = client;

        public async Task UploadBlobAsync(string name,
            long contentLength,
            Stream stream,
            string digest,
            CancellationToken cancellationToken = default)
        {
            var request = new RequestBuilder()
                .WithHttpMethod(HttpMethod.Post)
                .WithPath($"v2/{name}/blobs/uploads/")
                .Build();

            var response = await this._client.MakeRequestAsync(request, cancellationToken);
            var location = response.Headers.GetString("Location");

            var parameters = new UploadParameters
            {
                Digest = digest
            };

            var content = new StreamContent(stream);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            content.Headers.ContentLength = stream.Length;

            var request2 = new RequestBuilder()
                .WithHttpMethod(HttpMethod.Put)
                .WithContent(content)
                .WithQueryString(parameters);

            if (Uri.IsWellFormedUriString(location, UriKind.Absolute))
            {
                request2.WithUri(location);
            }
            else
            {
                request2.WithPath(location);
            }

            await this._client.MakeRequestAsync(request2.Build(), cancellationToken).ConfigureAwait(false);
        }

        public Task<ResumableUploadResponse> InitiateBlobUploadAsync(
            string name,
            Stream stream = null,
            CancellationToken cancellationToken = default) => throw new NotImplementedException();

        public Task<MountResponse> MountBlobAsync(
            string name,
            MountParameters parameters,
            CancellationToken cancellationToken = default) => throw new NotImplementedException();

        public Task<BlobUploadStatus> GetBlobUploadStatus(
            string name,
            string uuid,
            CancellationToken cancellationToken = default) => throw new NotImplementedException();

        public Task<ResumableUploadResponse> UploadBlobChunkAsync(
            string name,
            string uuid,
            Stream chunk,
            CancellationToken cancellationToken = default) => throw new NotImplementedException();

        public Task<ResumableUploadResponse> CompleteBlobUploadAsync(
            string name,
            string uuid,
            Stream chunk = null,
            CancellationToken cancellationToken = default) => throw new NotImplementedException();

        public Task CancelBlobUploadAsync(
            string name,
            string uuid,
            CancellationToken cancellationToken = default)
        {
            var request = new RequestBuilder()
                .WithHttpMethod(HttpMethod.Delete)
                .WithPath($"v2/{name}/blobs/uploads/{uuid}")
                .Build();

            return this._client.MakeRequestAsync(request, cancellationToken);
        }
    }
}
