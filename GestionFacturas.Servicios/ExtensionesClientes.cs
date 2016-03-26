using GestionFacturas.Modelos;
using System.Linq;

namespace GestionFacturas.Servicios
{
    public static class ExtensionesClientes
    {

        public static IQueryable<Cliente> Where_FiltroBusqueda(this IQueryable<Cliente> consulta, FiltroBusquedaCliente filtroBusqueda)
        {
            if (!string.IsNullOrEmpty(filtroBusqueda.NombreOEmpresa))
            {
                consulta = consulta.Where(m => m.NombreOEmpresa.Contains(filtroBusqueda.NombreOEmpresa) ||
                                               m.NombreComercial.Contains(filtroBusqueda.NombreOEmpresa)
                );
            }
            if (!string.IsNullOrEmpty(filtroBusqueda.IdentificacionFiscal))
            {
                consulta = consulta.Where(m => m.NumeroIdentificacionFiscal.Contains(filtroBusqueda.IdentificacionFiscal));
            }

            if (filtroBusqueda.Id.HasValue)
            {
                consulta = consulta.Where(m => m.Id == filtroBusqueda.Id.Value);
            }

            return consulta;
        }

        public static IOrderedQueryable<Cliente> OrderBy_OrdenarPor(this IQueryable<Cliente> consulta, OrdenClientesEnum orden)
        {
            IOrderedQueryable<Cliente> consultaOrdenada;

            switch (orden)
            {
                case OrdenClientesEnum.Alfabetico:
                    consultaOrdenada = consulta.OrderBy(m => m.NombreComercial ?? m.NombreOEmpresa);
                    break;
                case OrdenClientesEnum.MayorFacturacion:
                    consultaOrdenada = consulta.OrderByDescending(m => m.Facturas.Sum(f=>f.Lineas.Sum(l=> l.Cantidad * l.PrecioUnitario)))
                                    .ThenBy(m => m.NombreComercial ?? m.NombreOEmpresa);
                    break;
                case OrdenClientesEnum.MenorFacturacion:
                    consultaOrdenada = consulta.OrderBy(m => m.Facturas.Sum(f => f.Lineas.Sum(l => l.Cantidad * l.PrecioUnitario)))
                                     .ThenBy(m => m.NombreComercial ?? m.NombreOEmpresa);
                    break;
                case OrdenClientesEnum.MasFacturas:
                    consultaOrdenada = consulta.OrderByDescending(m => m.Facturas.Count)
                        .ThenBy(m => m.NombreComercial ?? m.NombreOEmpresa);
                    break;
                case OrdenClientesEnum.MenosFacturas:
                    consultaOrdenada = consulta.OrderBy(m => m.Facturas.Count)
                        .ThenBy(m => m.NombreComercial ?? m.NombreOEmpresa);
                    break;
                default:
                    consultaOrdenada = consulta.OrderBy(m => m.NombreComercial ?? m.NombreOEmpresa);
                    break;
            }
         

            return consultaOrdenada;
        }
    }
}
