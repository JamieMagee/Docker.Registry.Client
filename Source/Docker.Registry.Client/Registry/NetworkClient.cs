namespace Docker.Registry.Client.Registry
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Docker.Registry.Client.Authentication;
    using Docker.Registry.Client.Helpers;

    internal class NetworkClient : IDisposable
    {
        private const string UserAgent = "Docker.Registry.Client";

        private static readonly TimeSpan InfiniteTimeout = TimeSpan.FromMilliseconds(Timeout.Infinite);

        private readonly AuthenticationProvider authenticationProvider;

        private readonly HttpClient client;

        private readonly RegistryClientConfiguration configuration;

        private readonly IEnumerable<Action<RegistryApiResponse>> errorHandlers =
            new Action<RegistryApiResponse>[]
            {
                r =>
                {
                    if (r.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new UnauthorizedApiException(r);
                    }
                }
            };

        private Uri baseUri;

        public NetworkClient(
            RegistryClientConfiguration configuration,
            AuthenticationProvider authenticationProvider)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.authenticationProvider = authenticationProvider ?? throw new ArgumentNullException(nameof(authenticationProvider));
            this.client = new HttpClient();
            this.DefaultTimeout = configuration.DefaultTimeout;
            if (this.configuration.EndpointBaseUri != null)
            {
                this.baseUri = this.configuration.EndpointBaseUri;
            }
        }

        public TimeSpan DefaultTimeout { get; set; }

        /// <inheritdoc/>
        public void Dispose() => this.client?.Dispose();

        /// <summary>
        /// Ensures that we have configured (and potentially probed) the end point.
        /// </summary>
        private async Task EnsureConnectionAsync()
        {
            if (this.baseUri != null)
            {
                return;
            }

            var tryUrls = new List<string>();

            // clean up the host
            var host = this.configuration.Host.ToLower(CultureInfo.InvariantCulture).Trim();

            if (host.StartsWith("http", StringComparison.InvariantCultureIgnoreCase))
            {
                // includes schema -- don't add
                tryUrls.Add(host);
            }
            else
            {
                tryUrls.Add($"https://{host}");
                tryUrls.Add($"http://{host}");
            }

            var exceptions = new List<Exception>();

            foreach (var url in tryUrls)
            {
                try
                {
                    await this.ProbeSingleAsync($"{url}/v2/").ConfigureAwait(false);
                    this.baseUri = new Uri(url);
                    return;
                }
                catch (Exception e)
                {
                    exceptions.Add(e);
                }
            }

            throw new RegistryConnectionException(
                $"Unable to connect to any: {tryUrls.Select(s => $"'{s}/v2/'").ToDelimitedString(", ")}'",
                new AggregateException(exceptions));
        }

        private async Task ProbeSingleAsync(string uri)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, uri);
            await this.client.SendAsync(request).ConfigureAwait(false);
        }

        internal async Task<RegistryApiResponse<string>> MakeRequestAsync(Request request, CancellationToken cancellationToken)
        {
            using var response = await this.InternalMakeRequestAsync(
                request,
                cancellationToken).ConfigureAwait(false);

            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            var apiResponse = new RegistryApiResponse<string>(
                response.StatusCode,
                responseBody,
                response.Headers);

            this.HandleIfErrorResponse(apiResponse);

            return apiResponse;
        }

        internal async Task<RegistryApiResponse<Stream>> MakeRequestForStreamedResponseAsync(Request request,
            CancellationToken cancellationToken)
        {
            var response = await this.InternalMakeRequestAsync(
                    request,
                    cancellationToken)
                .ConfigureAwait(false);

            var body = await response.Content.ReadAsStreamAsync(cancellationToken);

            var apiResponse = new RegistryApiResponse<Stream>(
                response.StatusCode,
                body,
                response.Headers);

            this.HandleIfErrorResponse(apiResponse);

            return apiResponse;
        }

        private async Task<HttpResponseMessage> InternalMakeRequestAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            await this.EnsureConnectionAsync().ConfigureAwait(false);

            var httpRequestMessage = this.PrepareRequest(request);

            await this.authenticationProvider.AuthenticateAsync(httpRequestMessage);

            var response = await this.client.SendAsync(
                httpRequestMessage,
                this.CreateLinkedToken(cancellationToken))
                .ConfigureAwait(false);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                // Prepare another request (we can't reuse the same request)
                var request2 = this.PrepareRequest(request);

                // Authenticate given the challenge
                await this.authenticationProvider.AuthenticateAsync(request2, response).ConfigureAwait(false);

                // Send it again
                response = await this.client.SendAsync(
                    request2,
                    cancellationToken).ConfigureAwait(false);
            }

            return response;
        }

        private void HandleIfErrorResponse(RegistryApiResponse response)
        {
            // If no customer handlers just default the response.
            foreach (var handler in this.errorHandlers)
            {
                handler(response);
            }

            // No custom handler was fired. Default the response for generic success/failures.
            if (response.StatusCode is < HttpStatusCode.OK or >= HttpStatusCode.BadRequest)
            {
                throw new RegistryApiException(response);
            }
        }

        private HttpRequestMessage PrepareRequest(Request request)
        {
            if (string.IsNullOrEmpty(request.Path) && string.IsNullOrEmpty(request.Uri))
            {
                throw new ArgumentNullException("Either path or uri is required.");
            }

            var uri = string.IsNullOrEmpty(request.Uri) ?
                this.baseUri.BuildUri(request.Path, request.QueryString)
                : new Uri(request.Uri).AddQueryString(request.QueryString);

            var httpRequestMessage = new HttpRequestMessage(request.HttpMethod, uri);

            httpRequestMessage.Headers.Add("User-Agent", UserAgent);
            httpRequestMessage.Headers.AddRange(request.Headers);

            httpRequestMessage.Content = request.Content;

            return httpRequestMessage;
        }

        private CancellationToken CreateLinkedToken(CancellationToken cancellationToken)
        {
            var timeoutTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            timeoutTokenSource.CancelAfter(this.DefaultTimeout);
            return timeoutTokenSource.Token;
        }
    }
}
