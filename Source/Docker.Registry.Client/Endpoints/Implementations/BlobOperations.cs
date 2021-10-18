namespace Docker.Registry.Client.Endpoints.Implementations
{
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Docker.Registry.Client.Helpers;
    using Docker.Registry.Client.Models;
    using Docker.Registry.Client.Registry;

    internal class BlobOperations : IBlobOperations
    {
        private readonly NetworkClient client;

        public BlobOperations(NetworkClient client)
        {
            this.client = client;
        }

        public async Task<GetBlobResponse> GetBlobAsync(
            string name,
            string digest,
            CancellationToken cancellationToken = default)
        {
            var request = new RequestBuilder()
                .WithHttpMethod(HttpMethod.Get)
                .WithPath($"v2/{name}/blobs/{digest}")
                .Build();

            var response = await this.client.MakeRequestForStreamedResponseAsync(request, cancellationToken).ConfigureAwait(false);

            return new GetBlobResponse(
                response.Headers.GetString("Docker-Content-Digest"),
                response.Body);
        }

        public Task DeleteBlobAsync(
            string name,
            string digest,
            CancellationToken cancellationToken = default)
        {
            var request = new RequestBuilder()
                .WithHttpMethod(HttpMethod.Get)
                .WithPath($"v2/{name}/blobs/{digest}")
                .Build();

            return this.client.MakeRequestAsync(request, cancellationToken);
        }
    }
}
