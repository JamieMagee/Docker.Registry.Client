namespace Docker.Registry.Client.Models
{
    using System.Text.Json.Serialization;

    public record ManifestHistory
    {
        [JsonPropertyName("v1Compatibility")]
        public string V1Compatibility { get; set; }
    }
}
