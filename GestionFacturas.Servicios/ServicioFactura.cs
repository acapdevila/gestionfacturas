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

        public async Task<IEnumerable<ItemListaFacturas>> ListaFacturasAsync()
        {
            var consultaFacturas = _contexto.Facturas.Select(m => new ItemListaFacturas
            {
                EstadoFactura = m.EstadoFactura,
                FechaEmisionFactura = m.FechaEmisionFactura,
                FechaVencimientoFactura = m.FechaVencimientoFactura,
                FormatoNumeroFactura = m.FormatoNumeroFactura,
                Id = m.Id,
                IdUsuario = m.IdUsuario,
                IdVendedor = m.IdVendedor,
                ImporteTotal = m.Lineas.Sum(l => l.PrecioXCantidad + (l.PrecioXCantidad * l.PorcentajeImpuesto / 100)),
                NumeracionFactura = m.NumeracionFactura,
                SerieFactura = m.SerieFactura,
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
