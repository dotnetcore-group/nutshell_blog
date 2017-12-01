using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Nutshell.Blog.Core
{
    public static class StringExtensionMethod
    {
        public static string FixTags(this string inputHtml, string[] allowedTags, string[] allowedProperties) =>
    Regex.Replace(inputHtml, "(<.*?>)", match => fixTag(match, allowedTags, allowedProperties), RegexOptions.IgnoreCase);

        public static string FixLinks(this string input, string[] whitelist) =>
    Regex.Replace(input, "(<a.*?>)", (MatchEvaluator)(match => fixLink(match, whitelist)), RegexOptions.IgnoreCase);


        private static string fixTag(Match tagMatch, string[] allowedTags, string[] allowedProperties)
        {
            string input = tagMatch.Value;
            string str2 = Regex.Match(input, @"</?(?<tagName>[^\s/]*)[>\s/]", RegexOptions.IgnoreCase).Groups["tagName"].Value.ToLower();
            if (Array.IndexOf(allowedTags, str2) < 0)
            {
                return "";
            }
            return Regex.Replace(input, "\\S+\\s*=\\s*[\"| ']\\S*\\s*[\"|']", (MatchEvaluator)(match => fixProperty(match, allowedProperties)), RegexOptions.IgnoreCase);
        }

        private static string fixProperty(Match propertyMatch, string[] allowedProperties)
        {
            string input = propertyMatch.Value;
            string str2 = Regex.Match(input, @"(?<prop>\S*)(\s*)(?==)",RegexOptions.IgnoreCase).Groups["prop"].Value.ToLower();
            if (Array.IndexOf<string>(allowedProperties, str2) < 0)
            {
                return "";
            }
            return input;
        }

        private static string fixLink(Match linkMatch, string[] whitelist)
        {
            string input = linkMatch.Value;
            if (Regex.IsMatch(input, "rel\\s*?=\\s*?['\"]?.*?nofollow.*?['\"]?", RegexOptions.IgnoreCase))
            {
                return input;
            }
            string uriString = Regex.Match(input, "href\\s*=\\s*['\"]?(?<url>[^'\"\\s]*)['\"]?", RegexOptions.IgnoreCase).Groups["url"].Value;
            if (!uriString.Contains("http://") && !uriString.Contains("https://"))
            {
                return input;
            }
            string str3 = new Uri(uriString).Host.ToLower();
            if (Array.IndexOf<string>(whitelist, str3) >= 0)
            {
                return input;
            }
            string str4 = Regex.Replace(input, "(?<a>rel\\s*=\\s*(?<b>['\"]?))((?<c>[^'\"\\s]*|[^'\"]*))(?<d>['\"]?)?", "${a}nofollow${d}", RegexOptions.IgnoreCase);
            if (str4 != input)
            {
                return str4;
            }
            return Regex.Replace(input, "<a", "<a rel=\"nofollow\"", RegexOptions.IgnoreCase);
        }

    }
}
