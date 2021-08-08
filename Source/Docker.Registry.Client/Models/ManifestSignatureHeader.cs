namespace Docker.Registry.Client.Models
{
    using System.Runtime.Serialization;

    [DataContract]
    public class ManifestSignatureHeader
    {
        [DataMember(Name = "alg")]
        public string Alg { get; set; }
    }
}
