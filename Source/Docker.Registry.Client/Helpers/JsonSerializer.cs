namespace Docker.Registry.Client.Helpers
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    /// <summary>
    /// Facade for <see cref="JsonConvert" />.
    /// </summary>
    internal class JsonSerializer
    {
        private static readonly JsonSerializerSettings Settings = new()
        {
            NullValueHandling = NullValueHandling.Ignore,
            Converters =
            {
                //new JsonIso8601AndUnixEpochDateConverter(),
                //new JsonVersionConverter(),
                new StringEnumConverter()
                //new TimeSpanSecondsConverter(),
                //new TimeSpanNanosecondsConverter()
            }
        };

        public T DeserializeObject<T>(string json) => JsonConvert.DeserializeObject<T>(json, Settings);

        public string SerializeObject<T>(T value) => JsonConvert.SerializeObject(value, Settings);
    }
}
