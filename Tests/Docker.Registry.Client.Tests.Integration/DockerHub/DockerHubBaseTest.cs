namespace Docker.Registry.Client.Tests.Integration.DockerHub
{
    using System;
    using Authentication;
    using Registry;

    public abstract class DockerHubBaseTest
    {
        internal readonly IRegistryClient client;

        protected DockerHubBaseTest()
        {
            var authentication = new PasswordOAuthAuthenticationProvider(
                Environment.GetEnvironmentVariable(Constants.DockerHubUsernameEnvironmentVariable),
                Environment.GetEnvironmentVariable(Constants.DockerHubTokenEnvironmentVariable));
            this.client = new RegistryClientConfiguration("registry-1.docker.io").CreateClient(authentication);
        }
    }
}
