namespace Docker.Registry.Client.Helpers
{
    internal interface IQueryString
    {
        string GetQueryString();

        void Add(string key, string value);

        void Add(string key, string[] values);
    }
}
