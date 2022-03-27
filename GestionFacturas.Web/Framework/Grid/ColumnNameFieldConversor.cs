namespace GestionFacturas.Web.Framework.Grid
{
    public static class ColumnNameFieldConversor
    {
        public static string ToGridField(this string word)
        {
            if (string.IsNullOrEmpty(word)) return word;
            return char.ToLowerInvariant(word[0]) + word.Substring(1);
        }
    }
}
