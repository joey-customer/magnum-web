using System.Text.RegularExpressions;

namespace Magnum.Web.Utils
{
    public static class StringUtils
    {
        public static string StripTagsRegex(string source)
        {
            if (source == null)
            {
                return "";
            }
            return Regex.Replace(source, "<.*?>", string.Empty);
        }
    }
}

