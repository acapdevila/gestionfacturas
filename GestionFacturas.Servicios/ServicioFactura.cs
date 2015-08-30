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
        private int PorcentajeIvaPorDefecto
        {
            get { return 21; }
        }

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

        public async Task<EditorFactura> ObtenerEditorFacturaParaCrearNuevaFactura(string serie)
        {
            var numero = 1;
            var formato = "{0}{1:1000}";

            var ultimaFacturaCreada = await ObtenerUlitmaFacturaDeLaSerie(serie);

            if(ultimaFacturaCreada != null)
            {
                serie = ultimaFacturaCreada.SerieFactura;
                numero = ultimaFacturaCreada.NumeracionFactura + 1;
                formato = ultimaFacturaCreada.FormatoNumeroFactura;
            }

            return new EditorFactura
            {
                SerieFactura = serie,
                NumeracionFactura = numero,
                FormatoNumeroFactura = formato,
                FechaEmisionFactura = DateTime.Today,
                PorcentajeIvaPorDefecto = PorcentajeIvaPorDefecto,
                FormaPago = FormaPagoEnum.Transferencia,
                
                Lineas = new List<EditorLineaFactura> {
                            new EditorLineaFactura {
                                    Cantidad = 1,
                                    PorcentajeImpuesto = PorcentajeIvaPorDefecto
                            }
                      }
            };
        }
        public async Task<VisorFactura> BuscarVisorFacturaAsync(int? idFactura)
        {
            var factura = await BuscarFacturaAsync(idFactura); 
            var visor = new VisorFactura();
            visor.InyectarFactura(factura);
            return visor;
        }

        public async Task<EditorFactura> BuscaEditorFacturaAsync(int? idFactura)
        {
            var factura = await BuscarFacturaAsync(idFactura);
            var editor = new EditorFactura();
            editor.InyectarFactura(factura);
            return editor;
        }

        private async Task<Factura> ObtenerUlitmaFacturaDeLaSerie(string serie)
        {
            var consulta = _contexto.Facturas.AsQueryable();

            if (!string.IsNullOrEmpty(serie))
            {
                consulta = consulta.Where(m => m.SerieFactura == serie);
            }                    

            return await consulta
                         .OrderByDescending(m => m.FechaEmisionFactura)
                        .FirstOrDefaultAsync();
        }
    }
}
