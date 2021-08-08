namespace Docker.Registry.Client.Models
{
    using JetBrains.Annotations;

    [PublicAPI]
    public class BlobHeader
    {
        internal BlobHeader(string dockerContentDigest) => this.DockerContentDigest = dockerContentDigest;

        public string DockerContentDigest { get; }
    }
}
