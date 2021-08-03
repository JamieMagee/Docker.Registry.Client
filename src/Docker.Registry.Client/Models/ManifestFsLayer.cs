using System.Runtime.Serialization;

namespace Docker.Registry.Client.Models
{
    [DataContract]
    public class ManifestFsLayer
    {
        [DataMember(Name = "blobSum")]
        public string BlobSum { get; set; }
    }
}