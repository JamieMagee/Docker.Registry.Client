using System.Threading;
using System.Threading.Tasks;

using Docker.Registry.Client.Models;

using JetBrains.Annotations;

namespace Docker.Registry.Client.Endpoints
{
    public interface ICatalogOperations
    {
        /// <summary>
        ///     Retrieve a sorted, json list of repositories available in the registry.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [PublicAPI]
        Task<Catalog> GetCatalogAsync(
            CatalogParameters parameters = null,
            CancellationToken cancellationToken = default);
    }
}