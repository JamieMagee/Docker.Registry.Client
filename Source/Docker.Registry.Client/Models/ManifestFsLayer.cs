namespace Docker.Registry.Client.Models
{
    using System.Text.Json.Serialization;

    public record ManifestFsLayer
    {
        [JsonPropertyName("blobSum")]
        public string BlobSum { get; set; }
    }
}
