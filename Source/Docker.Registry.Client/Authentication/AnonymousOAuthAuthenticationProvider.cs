namespace Docker.Registry.Client.Authentication
{
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using OAuth;

    [PublicAPI]
    public class AnonymousOAuthAuthenticationProvider : AuthenticationProvider
    {
        private readonly OAuthClient _client = new();

        private static string Schema { get; } = "Bearer";

        public override Task AuthenticateAsync(HttpRequestMessage request) => Task.CompletedTask;

        public override async Task AuthenticateAsync(
            HttpRequestMessage request,
            HttpResponseMessage response)
        {
            var header = this.TryGetSchemaHeader(response, Schema);

            //Get the bearer bits
            var bearerBits = AuthenticateParser.ParseTyped(header.Parameter);

            //Get the token
            var token = await this._client.GetTokenAsync(
                bearerBits.Realm,
                bearerBits.Service,
                bearerBits.Scope);

            //Set the header
            request.Headers.Authorization = new AuthenticationHeaderValue(Schema, token.Token);
        }
    }
}
