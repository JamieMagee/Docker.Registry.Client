namespace Docker.Registry.Client.Endpoints.Implementations
{
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Docker.Registry.Client.Registry;

    internal class SystemOperations : ISystemOperations
    {
        private readonly NetworkClient client;

        public SystemOperations(NetworkClient client)
        {
            this.client = client;
        }

        public async Task PingAsync(CancellationToken cancellationToken = default)
        {
            var request = new RequestBuilder()
                .WithHttpMethod(HttpMethod.Get)
                .WithPath("v2/")
                .Build();

            await this.client.MakeRequestAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}
