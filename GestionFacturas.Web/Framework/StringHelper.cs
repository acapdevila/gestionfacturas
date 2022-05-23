
namespace GestionFacturas.Web.Framework
{
    public static class StringHelper
    {
        
        public static string QuitarStringModel(this string texto)
        {
            return texto.Remove(
                texto.LastIndexOf(
                    "Model",
                    StringComparison.Ordinal));
        }


        public static string SeparadasPorPuntoYComma(this ICollection<string> textos)
        {
            return textos.Any() ? string.Join("; ", textos.Where(m => !string.IsNullOrEmpty(m)).Select(m => m)) : string.Empty;
        }
    }
}
