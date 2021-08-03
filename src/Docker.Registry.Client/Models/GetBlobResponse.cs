using System;
using System.IO;

namespace Docker.Registry.Client.Models
{
    public class GetBlobResponse : BlobHeader, IDisposable
    {
        internal GetBlobResponse(string dockerContentDigest, Stream stream)
            : base(dockerContentDigest)
        {
            this.Stream = stream;
        }

        public Stream Stream { get; }

        public void Dispose()
        {
            this.Stream?.Dispose();
        }
    }
}