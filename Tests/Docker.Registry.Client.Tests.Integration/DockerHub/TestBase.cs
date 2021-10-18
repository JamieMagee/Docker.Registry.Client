namespace Docker.Registry.Client.Tests.Integration.DockerHub
{
    using System;
    using Docker.Registry.Client.Authentication;
    using Docker.Registry.Client.Registry;

    public abstract class TestBase
    {
        private protected readonly IRegistryClient client;

        protected TestBase()
        {
            var authentication = new PasswordOAuthAuthenticationProvider(
                Environment.GetEnvironmentVariable(Constants.DockerHubUsernameEnvironmentVariable),
                Environment.GetEnvironmentVariable(Constants.DockerHubTokenEnvironmentVariable));
            this.client = new RegistryClientConfiguration("registry-1.docker.io").CreateClient(authentication);
        }
    }
}
