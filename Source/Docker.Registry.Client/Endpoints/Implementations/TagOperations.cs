namespace Docker.Registry.Client.Endpoints.Implementations
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Helpers;
    using JetBrains.Annotations;
    using Models;
    using Registry;

    internal class TagOperations : ITagOperations
    {
        private readonly NetworkClient _client;

        public TagOperations([NotNull] NetworkClient client) => this._client = client ?? throw new ArgumentNullException(nameof(client));

        public async Task<ListImageTagsResponse> ListImageTagsAsync(
            string name,
            ListImageTagsParameters parameters = null,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException(
                    $"'{nameof(name)}' cannot be null or empty",
                    nameof(name));
            }

            parameters = parameters ?? new ListImageTagsParameters();

            var queryString = new QueryString();

            queryString.AddFromObjectWithQueryParameters(parameters);

            var response = await this._client.MakeRequestAsync(
                cancellationToken,
                HttpMethod.Get,
                $"v2/{name}/tags/list",
                queryString).ConfigureAwait(false);

            return this._client.JsonSerializer.DeserializeObject<ListImageTagsResponse>(
                response.Body);
        }
    }
}
