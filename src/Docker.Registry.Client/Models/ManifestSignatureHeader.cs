using System.Runtime.Serialization;

namespace Docker.Registry.Client.Models
{
    [DataContract]
    public class ManifestSignatureHeader
    {
        [DataMember(Name = "alg")]
        public string Alg { get; set; }
    }
}