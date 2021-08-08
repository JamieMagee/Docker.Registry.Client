namespace Docker.Registry.Client.Endpoints
{
    using System.Threading;
    using System.Threading.Tasks;
    using JetBrains.Annotations;

    public interface ISystemOperations
    {
        [PublicAPI]
        Task PingAsync(CancellationToken cancellationToken = default);
    }
}
