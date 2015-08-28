using GestionFacturas.Datos;
using GestionFacturas.Modelos;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;

namespace GestionFacturas.Servicios
{
    public class ServicioFactura : ServicioCrudFactura
    {
        public ServicioFactura(ContextoBaseDatos contexto) : base(contexto)
        {
          
        }

        public async Task<IEnumerable<ItemListaFacturas>> ListaGestionFacturasAsync()
        {
            var consultaFacturas = _contexto.Facturas.Select(m => new ItemListaFacturas
            {
                Id = m.Id,
                IdUsuario = m.IdUsuario,
                IdVendedor = m.IdVendedor,
                FormatoNumeroFactura = m.FormatoNumeroFactura,
                NumeracionFactura = m.NumeracionFactura,
                SerieFactura = m.SerieFactura,
                FechaEmisionFactura = m.FechaEmisionFactura,
                FechaVencimientoFactura = m.FechaVencimientoFactura,
                EstadoFactura = m.EstadoFactura,
                BaseImponible = m.Lineas.Sum(l => (decimal?)(l.PrecioUnitario * l.Cantidad)) ?? 0,
                Impuestos = m.Lineas.Sum(l => (decimal?)(l.PrecioUnitario * l.Cantidad * l.PorcentajeImpuesto / 100)) ?? 0,
                ImporteTotal = m.Lineas.Sum(l => (decimal?)((l.PrecioUnitario * l.Cantidad) + (l.PrecioUnitario * l.Cantidad * l.PorcentajeImpuesto / 100))) ?? 0,
                VendedorNombreOEmpresa = m.VendedorNombreOEmpresa
            });

            var facturas = await consultaFacturas.ToListAsync();

            return facturas;
        }

        public async Task<EditorFactura> BuscaFacturaEditor(int? id)
        {
            var factura = await _contexto.Facturas.FindAsync(id);
            var editor = new EditorFactura();
            editor.InjectFrom(factura);
            return editor;
        }
    }
}
