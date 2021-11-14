namespace Docker.Registry.Client.Models
{
    using Docker.Registry.Client.QueryParameters;

    public record ListImageTagsParameters
    {
        /// <summary>
        /// Limit the number of entries in each response. It not present, all entries will be returned
        /// </summary>
        [QueryParameter("n")]
        public int? Number { get; set; }
    }
}
