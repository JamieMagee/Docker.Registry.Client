using JetBrains.Annotations;

namespace Docker.Registry.Client.Models
{
    [PublicAPI]
    public class BlobHeader
    {
        internal BlobHeader(string dockerContentDigest)
        {
            this.DockerContentDigest = dockerContentDigest;
        }

        public string DockerContentDigest { get; }
    }
}