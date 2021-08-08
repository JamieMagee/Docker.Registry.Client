namespace Docker.Registry.Client.Models
{
    using System.Runtime.Serialization;

    [DataContract]
    public class ManifestFsLayer
    {
        [DataMember(Name = "blobSum")]
        public string BlobSum { get; set; }
    }
}
