namespace Docker.Registry.Client.Authentication
{
    public class ParsedAuthentication
    {
        public ParsedAuthentication(string realm, string service, string scope)
        {
            this.Realm = realm;
            this.Service = service;
            this.Scope = scope;
        }

        public string Realm { get; }

        public string Service { get; }

        public string Scope { get; }
    }
}
