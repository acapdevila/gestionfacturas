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
                case OrdenFacturas.NumeroDesc:
                    consultaOrdenada = consulta.OrderBy(m => m.SerieFactura)
                        .ThenByDescending(m => m.NumeracionFactura)
                        .ThenByDescending(m => m.FechaEmisionFacturaDateTime);
                    break;
                case OrdenFacturas.NumeroAsc:
                    consultaOrdenada = consulta.OrderBy(m => m.SerieFactura)
                        .ThenBy(m => m.NumeracionFactura)
                        .ThenBy(m => m.FechaEmisionFacturaDateTime);
                    break;
                case OrdenFacturas.FechaDesc:
                    consultaOrdenada = consulta.OrderBy(m => m.SerieFactura)
                        .ThenByDescending(m => m.FechaEmisionFacturaDateTime)
                        .ThenByDescending(m => m.NumeracionFactura);
                    break;
                case OrdenFacturas.FechaAsc:
                    consultaOrdenada = consulta.OrderBy(m => m.SerieFactura)
                        .ThenBy(m => m.FechaEmisionFacturaDateTime)
                        .ThenBy(m => m.NumeracionFactura);
                    break;
                default:
                    consultaOrdenada = consulta.OrderBy(m => m.SerieFactura)
                        .ThenByDescending(m => m.NumeracionFactura)
                        .ThenByDescending(m => m.FechaEmisionFacturaDateTime);
                    break;
            }


            return consultaOrdenada;
        }

    }
}
