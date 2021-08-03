using System.Runtime.Serialization;

namespace Docker.Registry.Client.Models
{
    [DataContract]
    public class ManifestHistory
    {
        [DataMember(Name = "v1Compatibility")]
        public string V1Compatibility { get; set; }
    }
}