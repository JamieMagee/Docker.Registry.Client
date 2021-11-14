namespace Docker.Registry.Client.Models
{
    using System.Text.Json.Serialization;

    public record ListImageTagsResponse
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("tags")]
        public string[] Tags { get; set; }
    }
}
