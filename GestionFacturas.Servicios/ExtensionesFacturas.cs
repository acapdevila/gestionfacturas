using GestionFacturas.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Omu.ValueInjecter;
using PagedList.EntityFramework;
using System.Threading.Tasks;
using PagedList;

namespace GestionFacturas.Servicios
{
    public static class ExtensionesFacturas
    {

        public static IQueryable<Factura> Where_FiltroBusqueda(this IQueryable<Factura> consulta, FiltroBusquedaFactura filtroBusqueda)
        {
            if (!string.IsNullOrEmpty(filtroBusqueda.SerieFactura))
            {
                consulta = consulta.Where(m => m.SerieFactura == filtroBusqueda.SerieFactura);
            }

            if (!string.IsNullOrEmpty(filtroBusqueda.NombreOEmpresaCliente))
            {
                consulta = consulta.Where(m =>
                            m.CompradorNombreOEmpresa.Contains(filtroBusqueda.NombreOEmpresaCliente) ||
                            m.Comprador.NombreComercial.Contains(filtroBusqueda.NombreOEmpresaCliente));
            }

            if (filtroBusqueda.FechaDesde.HasValue && filtroBusqueda.FechaHasta.HasValue)
            {
                consulta = consulta.Where(m => m.FechaEmisionFactura >= filtroBusqueda.FechaDesde.Value && m.FechaEmisionFactura <= filtroBusqueda.FechaHasta.Value);
            }

            if (filtroBusqueda.IdCliente.HasValue)
            {
                consulta = consulta.Where(m => m.IdComprador == filtroBusqueda.IdCliente.Value);
            }
            
            if (!string.IsNullOrEmpty(filtroBusqueda.Conceptos))
            {
                consulta = consulta.Where(m => m.Lineas.Any(l => l.Descripcion.Contains(filtroBusqueda.Conceptos)));    
            }

            return consulta;
        }

        public static IOrderedQueryable<Factura> OrderBy_OrdenarPor(this IQueryable<Factura> consulta, OrdenFacturasEnum orden)
        {
            IOrderedQueryable<Factura> consultaOrdenada;

            switch (orden)
            {
                case OrdenFacturasEnum.NumeroDesc:
                    consultaOrdenada = consulta.OrderBy(m => m.SerieFactura)
                         .ThenByDescending(m => m.NumeracionFactura)
                        .ThenByDescending(m => m.FechaEmisionFactura);
                    break;
                case OrdenFacturasEnum.NumeroAsc:
                    consultaOrdenada = consulta.OrderBy(m => m.SerieFactura)
                        .ThenBy(m => m.NumeracionFactura)
                        .ThenBy(m => m.FechaEmisionFactura);
                    break;
                case OrdenFacturasEnum.FechaDesc:
                    consultaOrdenada = consulta.OrderBy(m => m.SerieFactura)
                         .ThenByDescending(m => m.FechaEmisionFactura)
                         .ThenByDescending(m => m.NumeracionFactura);
                    break;
                case OrdenFacturasEnum.FechaAsc:
                    consultaOrdenada = consulta.OrderBy(m => m.SerieFactura)
                       .ThenBy(m => m.FechaEmisionFactura)
                       .ThenBy(m => m.NumeracionFactura);
                    break;
                default:
                    consultaOrdenada = consulta.OrderBy(m => m.SerieFactura)
                       .ThenByDescending(m => m.NumeracionFactura)
                      .ThenByDescending(m => m.FechaEmisionFactura);
                    break;
            }


            return consultaOrdenada;
        }

   }
}
