namespace Docker.Registry.Client.Endpoints
{
    using System.Threading;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using Docker.Registry.Client.Models;

    public interface ITagOperations
    {
        [PublicAPI]
        Task<ListImageTagsResponse> ListImageTagsAsync(
            string name,
            ListImageTagsParameters parameters = null,
            CancellationToken cancellationToken = default);
    }
}
