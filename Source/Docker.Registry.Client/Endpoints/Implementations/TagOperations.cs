namespace Docker.Registry.Client.Endpoints.Implementations
{
    using System;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using Docker.Registry.Client.Models;
    using Docker.Registry.Client.Registry;

    internal class TagOperations : ITagOperations
    {
        private readonly NetworkClient client;

        public TagOperations(NetworkClient client)
        {
            this.client = client;
        }

        public async Task<ListImageTagsResponse> ListImageTagsAsync(
            string name,
            ListImageTagsParameters parameters = default,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException(
                    $"'{nameof(name)}' cannot be null or empty",
                    nameof(name));
            }

            var request = new RequestBuilder()
                .WithHttpMethod(HttpMethod.Get)
                .WithPath($"v2/{name}/tags/list")
                .WithQueryString(parameters ?? new ListImageTagsParameters())
                .Build();

            var response = await this.client.MakeRequestAsync(request, cancellationToken).ConfigureAwait(false);

            return JsonSerializer.Deserialize<ListImageTagsResponse>(response.Body);
        }
    }
}
