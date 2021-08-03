using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Docker.Registry.Client.Helpers;
using Docker.Registry.Client.Models;
using Docker.Registry.Client.Registry;

using JetBrains.Annotations;

namespace Docker.Registry.Client.Endpoints.Implementations
{
    internal class CatalogOperations : ICatalogOperations
    {
        private readonly NetworkClient _client;

        public CatalogOperations([NotNull] NetworkClient client)
        {
            this._client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<Catalog> GetCatalogAsync(
            CatalogParameters parameters = null,
            CancellationToken cancellationToken = default)
        {
            parameters = parameters ?? new CatalogParameters();

            var queryParameters = new QueryString();

            queryParameters.AddFromObjectWithQueryParameters(parameters);

            var response = await this._client.MakeRequestAsync(
                               cancellationToken,
                               HttpMethod.Get,
                               "v2/_catalog",
                               queryParameters).ConfigureAwait(false);

            return this._client.JsonSerializer.DeserializeObject<Catalog>(response.Body);
        }
    }
}