namespace Docker.Registry.Client.Models
{
    using System.Text.Json.Serialization;

    public record ManifestSignature
    {
        [JsonPropertyName("header")]
        public ManifestSignatureHeader Header { get; set; }

        [JsonPropertyName("signature")]
        public string Signature { get; set; }

        [JsonPropertyName("protected")]
        public string Protected { get; set; }
    }
}
