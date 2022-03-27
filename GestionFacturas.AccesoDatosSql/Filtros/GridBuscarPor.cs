using System.Text;
using System.Linq.Dynamic.Core;
using GestionFacturas.Dominio.Infra;

namespace GestionFacturas.AccesoDatosSql.Filtros
{
    public static class GridBuscarPorQueryExtensions
    {
        public static IQueryable<T> BuscarPor<T>(this IQueryable<T> consulta, string buscarPor,
            IEnumerable<string> camposBusqueda)
        {
            // Búsqueda general por todos los campos
            if (!string.IsNullOrEmpty(buscarPor))
            {
                StringBuilder sb = new();

                // Create dynamic Linq expression
                foreach (var campo in camposBusqueda)
                {
                    sb.AppendFormat($"Convert.ToString({campo}).Contains(@0) or {Environment.NewLine}");
                }

                // Remove last "or" occurrence
                string searchExpression = sb.ToString();

                if (!string.IsNullOrEmpty(searchExpression))
                {
                    searchExpression =
                        searchExpression[..searchExpression.LastIndexOf("or", StringComparison.OrdinalIgnoreCase)];

                    // Apply filtering, 
                    consulta = consulta.Where(searchExpression, buscarPor);
                }
            }

            return consulta;
        }

        public static IQueryable<T> BuscarPorCampos<T>(this IQueryable<T> consulta,
            List<Campo> campos)
        {
            // Búsqueda por campos específicos
            foreach (var campo in campos)
            {
                if (!string.IsNullOrWhiteSpace(campo.BuscarPor))
                    consulta = consulta.Where(
                        $"({campo.Nombre} == null ? false : Convert.ToString({campo.Nombre}).Contains(@0))",
                        campo.BuscarPor);
            }

            return consulta;
        }

    }
}