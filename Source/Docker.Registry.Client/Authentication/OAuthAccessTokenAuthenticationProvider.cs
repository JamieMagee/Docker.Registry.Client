namespace Docker.Registry.Client.Authentication
{
    using Docker.Registry.Client.OAuth;
    using JetBrains.Annotations;

    [PublicAPI]
    public class OAuthAccessTokenAuthenticationProvider : PasswordOAuthAuthenticationProvider
    {
        public OAuthAccessTokenAuthenticationProvider(string username, string password)
            : base(username, password)
        {
        }

        internal override string GetToken(OAuthToken token) => token.AccessToken;
    }
}
