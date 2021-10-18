namespace Docker.Registry.Client.Registry
{
    using System.Collections.Generic;
    using System.Net.Http;
    using Docker.Registry.Client.Helpers;

    internal class RequestBuilder
    {
        private readonly Request request = new();

        public Request Build() => this.request;

        public RequestBuilder WithHttpMethod(HttpMethod method)
        {
            this.request.HttpMethod = method;
            return this;
        }

        public RequestBuilder WithPath(string path)
        {
            this.request.Path = path;
            return this;
        }

        public RequestBuilder WithUri(string uri)
        {
            this.request.Uri = uri;
            return this;
        }

        public RequestBuilder WithContent(HttpContent content)
        {
            this.request.Content = content;
            return this;
        }

        public RequestBuilder WithQueryString<T>(T instance)
            where T : class
        {
            this.request.QueryString = new QueryString();
            this.request.QueryString.AddFromObjectWithQueryParameters(instance);
            return this;
        }

        public RequestBuilder WithHeaders(IDictionary<string, string> headers)
        {
            this.request.Headers = headers;
            return this;
        }
    }
}
