namespace Docker.Registry.Client.Helpers
{
    using System.Collections.Generic;
    using System.Linq;

    public static class StringExtensions
    {
        public static string ToDelimitedString(
            this IEnumerable<string> strings,
            string delimiter = "") => string.Join(delimiter, strings.IfNullEmpty().ToArray());
    }
}
