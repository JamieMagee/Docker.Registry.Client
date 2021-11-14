namespace Docker.Registry.Client.Models
{
    using System.Text.Json.Serialization;

    public record Catalog
    {
        [JsonPropertyName("repositories")]
        public string[] Repositories { get; set; }
    }
}
