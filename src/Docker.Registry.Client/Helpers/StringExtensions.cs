using System.Collections.Generic;
using System.Linq;

namespace Docker.Registry.Client.Helpers
{
    public static class StringExtensions
    {
        public static string ToDelimitedString(
            this IEnumerable<string> strings,
            string delimiter = "")
        {
            return string.Join(delimiter, strings.IfNullEmpty().ToArray());
        }
    }
}