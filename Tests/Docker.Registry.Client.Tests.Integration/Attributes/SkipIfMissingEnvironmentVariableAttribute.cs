namespace Docker.Registry.Client.Tests.Integration.Attributes
{
    using System;
    using Xunit;

    public sealed class SkipIfMissingEnvironmentVariableAttribute : FactAttribute
    {
        public SkipIfMissingEnvironmentVariableAttribute(string environmentVariableName)
        {
            if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(environmentVariableName)))
            {
                this.Skip = $"Missing environment variable {environmentVariableName}";
            }
        }
    }
}
