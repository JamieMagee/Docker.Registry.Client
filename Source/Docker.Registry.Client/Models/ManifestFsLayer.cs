namespace Docker.Registry.Client.Models
{
    using System.Text.Json.Serialization;

    public class ManifestFsLayer
    {
        [JsonPropertyName("blobSum")]
        public string BlobSum { get; set; }
    }
}
