using System.Runtime.Serialization;

namespace Docker.Registry.Client.Models
{
    [DataContract]
    public class ListImageTagsResponse
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "tags")]
        public string[] Tags { get; set; }
    }
}