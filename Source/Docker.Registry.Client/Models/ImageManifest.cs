namespace Docker.Registry.Client.Models
{
    using System.Text.Json.Serialization;

    public abstract record ImageManifest
    {
        /// <summary>
        /// This field specifies the image manifest schema version as an integer.
        /// </summary>
        [JsonPropertyName("schemaVersion")]
        public int SchemaVersion { get; set; }
    }
}
