namespace Docker.Registry.Client.Endpoints.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Runtime.Serialization;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using Docker.Registry.Client.Helpers;
    using Docker.Registry.Client.Models;
    using Docker.Registry.Client.Registry;
    using JetBrains.Annotations;

    internal class ManifestOperations : IManifestOperations
    {
        private readonly NetworkClient client;

        public ManifestOperations(NetworkClient client)
        {
            this.client = client;
        }

        public async Task<GetImageManifestResult> GetManifestAsync(
            string name,
            string reference,
            CancellationToken cancellationToken = default)
        {
            var headers = new Dictionary<string, string>
            {
                {
                    "Accept",
                    $"{ManifestMediaTypes.ManifestSchema1}, {ManifestMediaTypes.ManifestSchema2}, {ManifestMediaTypes.ManifestList}, {ManifestMediaTypes.ManifestSchema1Signed}"
                },
            };

            var request = new RequestBuilder()
                .WithHttpMethod(HttpMethod.Get)
                .WithPath($"v2/{name}/manifests/{reference}")
                .WithHeaders(headers)
                .Build();

            var response = await this.client.MakeRequestAsync(request, cancellationToken).ConfigureAwait(false);

            var contentType = this.GetContentType(response.GetHeader("ContentType"), response.Body);

            return contentType switch
            {
                ManifestMediaTypes.ManifestSchema1 => new GetImageManifestResult(
                    contentType,
                    JsonSerializer.Deserialize<ImageManifest2_1>(response.Body),
                    response.Body)
                {
                    DockerContentDigest = response.GetHeader("Docker-Content-Digest"),
                    Etag = response.GetHeader("Etag"),
                },
                ManifestMediaTypes.ManifestSchema1Signed => new GetImageManifestResult(
                    contentType,
                    JsonSerializer.Deserialize<ImageManifest2_1>(response.Body),
                    response.Body)
                {
                    DockerContentDigest = response.GetHeader("Docker-Content-Digest"),
                    Etag = response.GetHeader("Etag"),
                },
                ManifestMediaTypes.ManifestSchema2 => new GetImageManifestResult(
                    contentType,
                    JsonSerializer.Deserialize<ImageManifest2_2>(response.Body),
                    response.Body)
                {
                    DockerContentDigest = response.GetHeader("Docker-Content-Digest")
                },
                ManifestMediaTypes.ManifestList => new GetImageManifestResult(
                    contentType,
                    JsonSerializer.Deserialize<ManifestList>(response.Body),
                    response.Body),
                _ => throw new Exception($"Unexpected ContentType '{contentType}'."),
            };
        }

        /// <inheritdoc />
        public async Task<bool> DoesManifestExistAsync(string name, string reference, CancellationToken cancellationToken = default)
        {
            var headers = new Dictionary<string, string>
            {
                {
                    "Accept",
                    $"{ManifestMediaTypes.ManifestSchema1}, {ManifestMediaTypes.ManifestSchema2}, {ManifestMediaTypes.ManifestList}, {ManifestMediaTypes.ManifestSchema1Signed}"
                },
            };

            RegistryApiResponse<string> response;

            try
            {
                var request = new RequestBuilder()
                    .WithHttpMethod(HttpMethod.Head)
                    .WithPath($"v2/{name}/manifests/{reference}")
                    .WithHeaders(headers)
                    .Build();

                response = await this.client.MakeRequestAsync(request, cancellationToken).ConfigureAwait(false);
            }
            catch (RegistryApiException e)
            {
                if (e.StatusCode == HttpStatusCode.NotFound)
                {
                    return false;
                }

                throw;
            }

            return response?.StatusCode == HttpStatusCode.OK;
        }

        public Task PutManifestAsync(string name, string reference, ImageManifest manifest, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteManifestAsync(
            string name,
            string reference,
            CancellationToken cancellationToken = default)
        {
            var request = new RequestBuilder()
                .WithHttpMethod(HttpMethod.Delete)
                .WithPath($"v2/{name}/manifests/{reference}")
                .Build();

            await this.client.MakeRequestAsync(request, cancellationToken);
        }

        private string GetContentType(string contentTypeHeader, string manifest)
        {
            if (!string.IsNullOrWhiteSpace(contentTypeHeader))
            {
                return contentTypeHeader;
            }

            var check = JsonSerializer.Deserialize<SchemaCheck>(manifest);

            if (!string.IsNullOrWhiteSpace(check.MediaType))
            {
                return check.MediaType;
            }

            if (check.SchemaVersion == null)
            {
                return ManifestMediaTypes.ManifestSchema1;
            }

            if (check.SchemaVersion.Value == 2)
            {
                return ManifestMediaTypes.ManifestSchema2;
            }

            throw new Exception($"Unable to determine schema type from version {check.SchemaVersion}");
        }

        /// <inheritdoc />
        [PublicAPI]
        public async Task<string> GetManifestRawAsync(
            string name,
            string reference,
            CancellationToken cancellationToken)
        {
            var headers = new Dictionary<string, string>
            {
                {
                    "Accept",
                    $"{ManifestMediaTypes.ManifestSchema1}, {ManifestMediaTypes.ManifestSchema2}, {ManifestMediaTypes.ManifestList}, {ManifestMediaTypes.ManifestSchema1Signed}"
                },
            };

            var request = new RequestBuilder()
                .WithHttpMethod(HttpMethod.Get)
                .WithPath($"v2/{name}/manifests/{reference}")
                .WithHeaders(headers)
                .Build();

            var response = await this.client.MakeRequestAsync(request, cancellationToken).ConfigureAwait(false);

            return response.Body;
        }

        private class SchemaCheck
        {
            /// <summary>
            /// Gets or sets the image manifest schema version as an integer.
            /// </summary>
            [DataMember(Name = "schemaVersion")]
            public int? SchemaVersion { get; set; }

            [DataMember(Name = "mediaType")]
            public string MediaType { get; set; }
        }
    }
}
