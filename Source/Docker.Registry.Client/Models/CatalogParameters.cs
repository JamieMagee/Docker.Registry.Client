namespace Docker.Registry.Client.Models
{
    using JetBrains.Annotations;
    using QueryParameters;

    [PublicAPI]
    public class CatalogParameters
    {
        /// <summary>
        /// Limit the number of entries in each response. It not present, all entries will be returned
        /// </summary>
        [QueryParameter("n")]
        public int? Number { get; set; }

        /// <summary>
        /// Result set will include values lexically after last.
        /// </summary>
        [QueryParameter("last")]
        public int? Last { get; set; }
    }
}
