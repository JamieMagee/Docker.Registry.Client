namespace Docker.Registry.Client.Models
{
    using Docker.Registry.Client.QueryParameters;

    public record UploadParameters
    {
        [QueryParameter("digest")]
        public string Digest { get; set; }
    }
}
