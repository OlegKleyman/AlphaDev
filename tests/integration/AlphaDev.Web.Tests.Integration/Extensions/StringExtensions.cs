using System.Text.RegularExpressions;
using JetBrains.Annotations;
using Markdig;

namespace AlphaDev.Web.Tests.Integration.Extensions
{
    public static class StringExtensions
    {
        [NotNull]
        public static string NormalizeToWindowsLineEndings([NotNull] this string target) =>
            Regex.Replace(target, @"(?<!\r)\n", "\r\n", RegexOptions.Compiled);

        public static string ToHtmlFromMarkdown(this string target) => Markdown.ToHtml(target);

        public static bool IsEllipses(this string target) => target == "...";
    }
}