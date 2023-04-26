using GestionFacturas.Dominio;

namespace GestionFacturas.Aplicacion
{
    public static class ExtensionesFacturas
    {
            
        public static IOrderedQueryable<LineaListaGestionFacturas> OrderBy_OrdenarPor(this IQueryable<LineaListaGestionFacturas> consulta, OrdenFacturas orden)
        {
            IOrderedQueryable<LineaListaGestionFacturas> consultaOrdenada;

            switch (orden)
            {
                case OrdenFacturas.FechaDesc:
                    consultaOrdenada = consulta.OrderByDescending(m => m.FechaEmisionFacturaDateTime)
                            .ThenByDescending(m => m.NumeracionFactura);
                    break;
                case OrdenFacturas.FechaAsc:
                    consultaOrdenada = consulta.OrderBy(m => m.FechaEmisionFacturaDateTime)
                        .ThenBy(m => m.NumeracionFactura);
                    break;
                default:
                    consultaOrdenada = consulta.OrderByDescending(m => m.FechaEmisionFacturaDateTime)
                        .ThenByDescending(m => m.NumeracionFactura);
                    break;
            }


            return consultaOrdenada;
        }

    }
}
