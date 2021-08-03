using System;

namespace Docker.Registry.Client.QueryParameters
{
    [AttributeUsage(AttributeTargets.Property)]
    internal class QueryParameterAttribute : Attribute
    {
        public QueryParameterAttribute(string key)
        {
            this.Key = key;
        }

        public string Key { get; }
    }
}