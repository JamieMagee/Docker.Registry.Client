namespace Docker.Registry.Client.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using Docker.Registry.Client.Registry;

    internal static class HttpExtensions
    {
        internal static Uri BuildUri(this Uri baseUri, string path, IQueryString queryString)
        {
            if (baseUri == null)
            {
                throw new ArgumentNullException(nameof(baseUri));
            }

            var builder = new UriBuilder(baseUri);

            if (!string.IsNullOrEmpty(path))
            {
                builder.Path += path;
            }

            if (queryString != null)
            {
                builder.Query = queryString.GetQueryString();
            }

            return builder.Uri;
        }

        internal static Uri AddQueryString(this Uri baseUri, IQueryString queryString)
        {
            if (baseUri is null)
            {
                throw new ArgumentNullException(nameof(baseUri));
            }

            var builder = new UriBuilder(baseUri);

            if (queryString is not null)
            {
                builder.Query += $"&{queryString.GetQueryString()}";
            }

            return builder.Uri;
        }

        /// <summary>
        /// Attempts to retrieve the value of a response header.
        /// </summary>
        /// <param name="response"></param>
        /// <param name="name"></param>
        /// <returns>The first value if one is found, null otherwise.</returns>
        public static string GetHeader(this RegistryApiResponse response, string name)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            return response.Headers
                .FirstOrDefault(h => h.Key == name).Value?.FirstOrDefault();
        }

        /// <summary>
        /// Attempts to retrieve the value of a response header.
        /// </summary>
        /// <param name="headers"></param>
        /// <param name="name"></param>
        /// <returns>The first value if one is found, null otherwise.</returns>
        public static string[] GetHeaders(
            this IEnumerable<KeyValuePair<string, string[]>> headers,
            string name)
        {
            return headers
                .IfNullEmpty()
                .Where(h => h.Key == name)
                .Select(h => h.Value?.FirstOrDefault())
                .ToArray();
        }

        public static void AddRange(
            this HttpRequestHeaders header,
            IEnumerable<KeyValuePair<string, string>> headers)
        {
            if (header == null)
            {
                throw new ArgumentNullException(nameof(header));
            }

            foreach (var item in headers.IfNullEmpty())
            {
                header.Add(item.Key, item.Value);
            }
        }

        public static AuthenticationHeaderValue GetHeaderBySchema(
            this HttpResponseMessage response,
            string schema)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            return response.Headers.WwwAuthenticate.FirstOrDefault(s => s.Scheme == schema);
        }

        public static int? GetContentLength(this HttpResponseHeaders responseHeaders)
        {
            if (responseHeaders == null)
            {
                throw new ArgumentNullException(nameof(responseHeaders));
            }

            if (!responseHeaders.TryGetValues("Content-Length", out var values))
            {
                return null;
            }

            var raw = values.FirstOrDefault();

            if (int.TryParse(raw, out var parsed))
            {
                return parsed;
            }

            return null;
        }

        public static string GetString(this HttpResponseHeaders responseHeaders, string name)
        {
            if (responseHeaders == null)
            {
                throw new ArgumentNullException(nameof(responseHeaders));
            }

            if (!responseHeaders.TryGetValues(name, out var values))
            {
                return null;
            }

            return values.FirstOrDefault();
        }
    }
}
