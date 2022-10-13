using System.Text;
using System.Text.RegularExpressions;

namespace ClassLibrary.Common.Extenstions
{
    public static class StringExtentions
    {
        public static string SplitCamelCase(this string str) => Regex.Replace(Regex.Replace(str,@"(\P{Ll})(\P{Ll}\p{Ll})","$1 $2"),@"(\p{Ll})(\P{Ll})","$1 $2");
        public static string StripLeadingWhiteSpace(this string str)=>new Regex(@"^\s+", RegexOptions.Multiline).Replace(str, string.Empty);
        public static string ToTitleCase(this string str)
        {
            var value = str.Trim();
            var sb = new StringBuilder();
            for (int i = 0; i < value.Length; i++)
            {
                if(value[i] == ' ')
                {
                    sb.Append(' ');
                    continue;
                }
                if (i == 0 || value[i-1] == ' ')
                {
                    sb.Append(char.ToUpper(value[i]));
                    continue;
                }
                sb.Append(char.ToLower(value[i]));
            }
            return sb.ToString();
        }

        public static string ToUrlizedCase(this string str)=>str.Trim().ToLower().Replace(" ", "_");
    }
}
