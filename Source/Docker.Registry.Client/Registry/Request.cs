namespace Docker.Registry.Client.Registry
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using Docker.Registry.Client.Helpers;

    internal record Request
    {
        public HttpMethod HttpMethod { get; internal set; }

        public string Path { get; internal set; }

        public string Uri { get; internal set; }

        public IQueryString QueryString { get; internal set; }

        public IDictionary<string, string> Headers { get; internal set; }

        public HttpContent Content { get; internal set; }
    }
}
