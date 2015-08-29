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

        public async Task<IEnumerable<LineaListaGestionFacturas>> ListaGestionFacturasAsync()
        {
            var consultaFacturas = _contexto.Facturas.Select(m => new LineaListaGestionFacturas
            {
                Id = m.Id,
                IdUsuario = m.IdUsuario,
                IdComprador = m.IdComprador,
                FormatoNumeroFactura = m.FormatoNumeroFactura,
                NumeracionFactura = m.NumeracionFactura,
                SerieFactura = m.SerieFactura,
                FechaEmisionFactura = m.FechaEmisionFactura,
                FechaVencimientoFactura = m.FechaVencimientoFactura,
                EstadoFactura = m.EstadoFactura,
                BaseImponible = m.Lineas.Sum(l => (decimal?)(l.PrecioUnitario * l.Cantidad)) ?? 0,
                Impuestos = m.Lineas.Sum(l => (decimal?)(l.PrecioUnitario * l.Cantidad * l.PorcentajeImpuesto / 100)) ?? 0,
                ImporteTotal = m.Lineas.Sum(l => (decimal?)((l.PrecioUnitario * l.Cantidad) + (l.PrecioUnitario * l.Cantidad * l.PorcentajeImpuesto / 100))) ?? 0,
                CompradorNombreOEmpresa = m.CompradorNombreOEmpresa
            });

            var facturas = await consultaFacturas.ToListAsync();

            return facturas;
        }


        public async Task<VisorFactura> BuscarVisorFacturaAsync(int? idFactura)
        {
            var factura = await BuscarFacturaAsync(idFactura); 
            var visor = new VisorFactura();
            visor.InyectaFactura(factura);
            return visor;
        }

        public async Task<EditorFactura> BuscaFacturaEditorAsync(int? idFactura)
        {
            var factura = await BuscarFacturaAsync(idFactura);
            var editor = new EditorFactura();
            editor.InyectaFactura(factura);
            return editor;
        }
    }
}
