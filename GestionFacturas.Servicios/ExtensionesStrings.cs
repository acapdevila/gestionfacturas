using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace GestionFacturas.Servicios
{
    public static class ExtensionesStrings
    {
        public static string ToStringComaSeparated(this IEnumerable<string> terms)
        {
            return ToStringSeparated(terms, ", ");
        }

        public static string ToStringSeparated(this IEnumerable<string> terms, string separator)
        {
            var enumerable = terms as IList<string> ?? terms.ToList();

            if (terms != null && enumerable.Any())
            {
                var sb = new StringBuilder();
                foreach (var item in enumerable)
                {
                    sb.Append(item);
                    sb.Append(separator);
                }
                return sb.ToString();
            }

            return null;
        }

        public static List<string> ToListString(this string stringComaSeparated)
        {
            var list = new List<string>();

            if (!string.IsNullOrEmpty(stringComaSeparated))
                list = stringComaSeparated.Split(',').Select(m => m.Trim()).ToList();

            return list;
        }

        public static string ToSlugString(this string text)
        {
            if (text == null) return null;
            return text.Replace(" ", "-").Replace("/", "-").EliminarDiacriticos();
        }

        public static string EliminarDiacriticos(this string texto)
        {
            var normalizedString = texto.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public static string ToUppercaseFirst(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            s = s.ToLower();
            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }
    }
}
