using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace Docker.Registry.Client.Endpoints
{
    public interface ISystemOperations
    {
        [PublicAPI]
        Task PingAsync(CancellationToken cancellationToken = default);
    }
}