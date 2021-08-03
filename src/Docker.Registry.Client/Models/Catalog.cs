using System.Runtime.Serialization;

namespace Docker.Registry.Client.Models
{
    [DataContract]
    public class Catalog
    {
        [DataMember(Name = "repositories", EmitDefaultValue = false)]
        public string[] Repositories { get; set; }
    }
}