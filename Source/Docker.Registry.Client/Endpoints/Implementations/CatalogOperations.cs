namespace Docker.Registry.Client.Endpoints.Implementations
{
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using Docker.Registry.Client.Models;
    using Docker.Registry.Client.Registry;

    internal class CatalogOperations : ICatalogOperations
    {
        private readonly NetworkClient client;

        public CatalogOperations(NetworkClient client)
        {
            this.client = client;
        }

        public async Task<Catalog> GetCatalogAsync(
            CatalogParameters parameters = null,
            CancellationToken cancellationToken = default)
        {
            var request = new RequestBuilder()
                .WithHttpMethod(HttpMethod.Get)
                .WithPath("v2/_catalog")
                .WithQueryString(parameters ?? new CatalogParameters())
                .Build();

            var response = await this.client.MakeRequestAsync(request, cancellationToken).ConfigureAwait(false);

            return JsonSerializer.Deserialize<Catalog>(response.Body);
        }
    }
}
