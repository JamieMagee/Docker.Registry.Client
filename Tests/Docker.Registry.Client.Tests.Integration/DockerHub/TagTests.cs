namespace Docker.Registry.Client.Tests.Integration.DockerHub
{
    using System.Threading.Tasks;
    using Docker.Registry.Client.Models;
    using FluentAssertions;
    using Xunit;

    public class TagTests : TestBase
    {
        [Fact]
        public async Task ListTags()
        {
            var tags = await this.client.Tags.ListImageTagsAsync("library/ubuntu");

            tags.Should().NotBeNull();
            tags.Tags.Should().Contain("latest");
        }

        [Fact]
        public async Task ListTags_Limit()
        {
            var parameters = new ListImageTagsParameters
            {
                Number = 1
            };
            var tags = await this.client.Tags.ListImageTagsAsync("library/ubuntu", parameters);

            tags.Should().NotBeNull();
            tags.Tags.Should().HaveCount(1);
        }
    }
}
