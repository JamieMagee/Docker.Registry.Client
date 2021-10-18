namespace Docker.Registry.Client.Tests.Integration.MicrosoftContainerRegistry
{
    using System.Threading.Tasks;
    using Xunit;

    public class SystemTests : TestBase
    {
        [Fact]
        private async Task PingAsync()
        {
            var exception = await Record.ExceptionAsync(() => this.client.System.PingAsync());
            Assert.Null(exception);
        }
    }
}
