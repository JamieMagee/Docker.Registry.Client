namespace Docker.Registry.Client.Tests.Integration.DockerHub
{
    using System;
    using Authentication;
    using Registry;

    public class DockerHubBaseTest
    {
        protected readonly IRegistryClient client;

        public DockerHubBaseTest()
        {
            var authentication = new PasswordOAuthAuthenticationProvider(
                Environment.GetEnvironmentVariable(Constants.DockerHubUsernameEnvironmentVariable),
                Environment.GetEnvironmentVariable(Constants.DockerHubTokenEnvironmentVariable));
            this.client = new RegistryClientConfiguration("registry-1.docker.io").CreateClient(authentication);
        }
    }
}
