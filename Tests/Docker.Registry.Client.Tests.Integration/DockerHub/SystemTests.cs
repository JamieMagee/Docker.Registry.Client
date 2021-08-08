namespace Docker.Registry.Client.Tests.Integration.DockerHub
{
    using System.Threading.Tasks;
    using Attributes;
    using Xunit;

    public class SystemTests : DockerHubBaseTest
    {
        [SkipIfMissingEnvironmentVariable(Constants.DockerHubTokenEnvironmentVariable)]
        private async Task PingAsync()
        {
            var exception = await Record.ExceptionAsync(() => this.client.System.PingAsync());
            Assert.Null(exception);
        }
    }
}
