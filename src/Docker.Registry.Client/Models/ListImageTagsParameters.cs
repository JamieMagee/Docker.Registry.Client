using Docker.Registry.Client.QueryParameters;

namespace Docker.Registry.Client.Models
{
    public class ListImageTagsParameters
    {
        /// <summary>
        ///     Limit the number of entries in each response. It not present, all entries will be returned
        /// </summary
        [QueryParameter("n")]
        public int? Number { get; set; }
    }
}