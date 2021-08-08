namespace Docker.Registry.Client.Endpoints.Implementations
{
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Registry;

    internal class SystemOperations : ISystemOperations
    {
        private readonly NetworkClient _client;

        public SystemOperations(NetworkClient client) => this._client = client;

        public Task PingAsync(CancellationToken cancellationToken = default) =>
            this._client.MakeRequestAsync(cancellationToken, HttpMethod.Get, "v2/");
    }
}
