﻿namespace Docker.Registry.Client.Models
{
    using System.Text.Json.Serialization;

    public class ManifestSignatureHeader
    {
        [JsonPropertyName("alg")]
        public string Alg { get; set; }
    }
}
