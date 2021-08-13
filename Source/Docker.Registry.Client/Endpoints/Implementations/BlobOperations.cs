﻿namespace Docker.Registry.Client.Endpoints.Implementations
{
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Helpers;
    using Models;
    using Registry;

    internal class BlobOperations : IBlobOperations
    {
        private readonly NetworkClient _client;

        public BlobOperations(NetworkClient client) => this._client = client;

        public async Task<GetBlobResponse> GetBlobAsync(
            string name,
            string digest,
            CancellationToken cancellationToken = default)
        {
            var url = $"v2/{name}/blobs/{digest}";

            var response = await this._client.MakeRequestForStreamedResponseAsync(
                cancellationToken,
                HttpMethod.Get,
                url);

            return new GetBlobResponse(
                response.Headers.GetString("Docker-Content-Digest"),
                response.Body);
        }

        public Task DeleteBlobAsync(
            string name,
            string digest,
            CancellationToken cancellationToken = default)
        {
            var url = $"v2/{name}/blobs/{digest}";

            return this._client.MakeRequestAsync(cancellationToken, HttpMethod.Delete, url);
        }
    }
}