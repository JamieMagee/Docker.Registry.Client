using System;
using System.Threading;

using Docker.Registry.Client.Authentication;
using Docker.Registry.Client.Registry;

using JetBrains.Annotations;

namespace Docker.Registry.Client
{
    public class RegistryClientConfiguration
    {
        /// <summary>
        ///     Creates an instance of the RegistryClientConfiguration.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="defaultTimeout"></param>
        public RegistryClientConfiguration(string host, TimeSpan defaultTimeout = default)
            : this(defaultTimeout)
        {
            if (string.IsNullOrWhiteSpace(host))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(host));

            this.Host = host;
        }

        private RegistryClientConfiguration(TimeSpan defaultTimeout)
        {
            if (defaultTimeout != TimeSpan.Zero)
            {
                if (defaultTimeout < Timeout.InfiniteTimeSpan)
                    // TODO: Should be a resource for localization.
                    // TODO: Is this a good message?
                    throw new ArgumentException(
                        "Timeout must be greater than Timeout.Infinite",
                        nameof(defaultTimeout));
                this.DefaultTimeout = defaultTimeout;
            }
        }

        public Uri EndpointBaseUri { get; }

        public string Host { get; }

        public TimeSpan DefaultTimeout { get; internal set; } = TimeSpan.FromSeconds(100);

        [PublicAPI]
        public IRegistryClient CreateClient()
        {
            return new RegistryClient(this, new AnonymousOAuthAuthenticationProvider());
        }

        [PublicAPI]
        public IRegistryClient CreateClient(AuthenticationProvider authenticationProvider)
        {
            return new RegistryClient(this, authenticationProvider);
        }
    }
}