using System.Threading;
using System.Threading.Tasks;

using Docker.Registry.Client.Models;

using JetBrains.Annotations;

namespace Docker.Registry.Client.Endpoints
{
    public interface ITagOperations
    {
        [PublicAPI]
        Task<ListImageTagsResponse> ListImageTagsAsync(
            string name,
            ListImageTagsParameters parameters = null,
            CancellationToken cancellationToken = default);
    }
}