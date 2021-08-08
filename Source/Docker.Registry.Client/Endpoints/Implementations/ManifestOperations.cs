﻿namespace Docker.Registry.Client.Endpoints.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Runtime.Serialization;
    using System.Threading;
    using System.Threading.Tasks;
    using Helpers;
    using JetBrains.Annotations;
    using Models;
    using Newtonsoft.Json;
    using Registry;

    internal class ManifestOperations : IManifestOperations
    {
        private readonly NetworkClient _client;

        public ManifestOperations(NetworkClient client) => this._client = client;

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
                }
            };

            var response = await this._client.MakeRequestAsync(
                cancellationToken,
                HttpMethod.Get,
                $"v2/{name}/manifests/{reference}",
                null,
                headers).ConfigureAwait(false);

            var contentType = this.GetContentType(response.GetHeader("ContentType"), response.Body);

            switch (contentType)
            {
                case ManifestMediaTypes.ManifestSchema1:
                case ManifestMediaTypes.ManifestSchema1Signed:
                    return new GetImageManifestResult(
                        contentType,
                        this._client.JsonSerializer.DeserializeObject<ImageManifest2_1>(
                            response.Body),
                        response.Body)
                    {
                        DockerContentDigest = response.GetHeader("Docker-Content-Digest"), Etag = response.GetHeader("Etag")
                    };

                case ManifestMediaTypes.ManifestSchema2:
                    return new GetImageManifestResult(
                        contentType,
                        this._client.JsonSerializer.DeserializeObject<ImageManifest2_2>(
                            response.Body),
                        response.Body)
                    {
                        DockerContentDigest = response.GetHeader("Docker-Content-Digest")
                    };

                case ManifestMediaTypes.ManifestList:
                    return new GetImageManifestResult(
                        contentType,
                        this._client.JsonSerializer.DeserializeObject<ManifestList>(response.Body),
                        response.Body);

                default:
                    throw new Exception($"Unexpected ContentType '{contentType}'.");
            }
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
                response = await this._client.MakeRequestAsync(
                    cancellationToken,
                    HttpMethod.Head,
                    $"v2/{name}/manifests/{reference}",
                    null,
                    headers).ConfigureAwait(false);
            }
            catch (RegistryApiException e)
            {
                if (e.StatusCode == HttpStatusCode.NotFound)
                {
                    return false;
                }

                throw;
            }

            return response.StatusCode == HttpStatusCode.OK;
        }

        //public Task PutManifestAsync(string name, string reference, ImageManifest manifest,
        //    CancellationToken cancellationToken = default)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task DeleteManifestAsync(
            string name,
            string reference,
            CancellationToken cancellationToken = default)
        {
            var path = $"v2/{name}/manifests/{reference}";

            await this._client.MakeRequestAsync(cancellationToken, HttpMethod.Delete, path);
        }

        private string GetContentType(string contentTypeHeader, string manifest)
        {
            if (!string.IsNullOrWhiteSpace(contentTypeHeader))
            {
                return contentTypeHeader;
            }

            var check = JsonConvert.DeserializeObject<SchemaCheck>(manifest);

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

            throw new Exception(
                $"Unable to determine schema type from version {check.SchemaVersion}");
        }

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
                }
            };

            var response = await this._client.MakeRequestAsync(
                cancellationToken,
                HttpMethod.Get,
                $"v2/{name}/manifests/{reference}",
                null,
                headers).ConfigureAwait(false);

            return response.Body;
        }

        private class SchemaCheck
        {
            /// <summary>
            /// This field specifies the image manifest schema version as an integer.
            /// </summary>
            [DataMember(Name = "schemaVersion")]
            public int? SchemaVersion { get; set; }

            [DataMember(Name = "mediaType")]
            public string MediaType { get; set; }
        }
    }
}
