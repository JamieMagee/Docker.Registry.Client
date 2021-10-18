namespace Docker.Registry.Client.Authentication
{
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Docker.Registry.Client.OAuth;
    using JetBrains.Annotations;

    [PublicAPI]
    public class PasswordOAuthAuthenticationProvider : AuthenticationProvider
    {
        private readonly OAuthClient _client = new();

        private readonly string _password;

        private readonly string _username;

        public PasswordOAuthAuthenticationProvider(string username, string password)
        {
            this._username = username;
            this._password = password;
        }

        private static string Schema { get; } = "Bearer";

        public override Task AuthenticateAsync(HttpRequestMessage request) => Task.CompletedTask;

        public override async Task AuthenticateAsync(
            HttpRequestMessage request,
            HttpResponseMessage response)
        {
            var header = this.TryGetSchemaHeader(response, Schema);

            // Get the bearer bits
            var bearerBits = AuthenticateParser.ParseTyped(header.Parameter);

            // Get the token
            var token = await this._client.GetTokenAsync(
                bearerBits.Realm,
                bearerBits.Service,
                bearerBits.Scope,
                this._username,
                this._password)
                .ConfigureAwait(false);

            // Set the header
            request.Headers.Authorization = new AuthenticationHeaderValue(Schema, this.GetToken(token));
        }

        internal virtual string GetToken(OAuthToken token) => token.Token;
    }
}
