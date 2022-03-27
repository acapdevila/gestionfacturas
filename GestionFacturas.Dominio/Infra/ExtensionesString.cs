using System.Globalization;
using System.Text;

namespace GestionFacturas.Dominio.Infra
{
    public static class ExtensionesString
    {
        public static string RemoveDiacritics(this string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
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
        public static List<string> ConvertirALista(this string tagsSeparadosPorPuntoYComa)
        {
            return string.IsNullOrEmpty(tagsSeparadosPorPuntoYComa) ?
                             new List<string>() :
                             tagsSeparadosPorPuntoYComa.Split(";")
                             .ToList();
        }

        public static string Truncar(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value[..maxLength];
        }
    }
}
