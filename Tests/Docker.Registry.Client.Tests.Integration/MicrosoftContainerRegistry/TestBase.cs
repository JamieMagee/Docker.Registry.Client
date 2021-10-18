namespace Docker.Registry.Client.Tests.Integration.MicrosoftContainerRegistry
{
    using Docker.Registry.Client.Authentication;
    using Docker.Registry.Client.Registry;

    public abstract class TestBase
    {
        private protected readonly IRegistryClient client;

        protected TestBase()
        {
            var authentication = new AnonymousOAuthAuthenticationProvider();
            this.client = new RegistryClientConfiguration("mcr.microsoft.com").CreateClient(authentication);
        }
    }
}
