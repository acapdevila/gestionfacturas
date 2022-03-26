using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using GestionFacturas.AccesoDatosSql;
using GestionFacturas.Dominio;
using Microsoft.EntityFrameworkCore;
using Omu.ValueInjecter;

namespace GestionFacturas.Aplicacion
{
    public class ServicioCrudFactura
    {
        
        protected readonly FacturasContext _contexto;

        private Factura Factura { get; set; }

        public ServicioCrudFactura(FacturasContext contexto)
        {
            _contexto = contexto;
        }
            

        public async Task<int> CrearFacturaAsync(EditorFactura editor)
        {
            Factura = new Factura();

            ModificarFactura(editor);         

            _contexto.Facturas.Add(Factura);

            var cambios = await GuardarCambiosAsync();
            
            editor.InyectarFactura(Factura);

            return cambios;
        }

        public async Task<int> CrearFacturasAsync(List<EditorFactura> editores)
        {
            foreach (var editor in editores)
            {
                Factura = new Factura();

                ModificarFactura(editor);

                _contexto.Facturas.Add(Factura);
            }
            var cambios = await GuardarCambiosAsync();

            return cambios;
        }

        public async Task<int> ActualizarFacturaAsync(EditorFactura editor)
        {
            Factura = await BuscarFacturaAsync(editor.Id);

            ModificarFactura(editor);
                             
            _contexto.Entry(Factura).State = EntityState.Modified;
            return  await GuardarCambiosAsync();
         }


        public async Task CambiarEstadoFacturaAsync(EditorEstadoFactura editor)
        {
            Factura = await BuscarFacturaAsync(editor.IdFactura);

            Factura.EstadoFactura = editor.EstadoFactura;

            _contexto.Entry(Factura).State = EntityState.Modified;
            await GuardarCambiosAsync();
        }


        public async Task<int> EliminarFactura(int idFactura)
        {
            Factura = await BuscarFacturaAsync(idFactura);
            
            while (Factura.Lineas.Any())
            {
                var linea = Factura.Lineas.First();
                _contexto.FacturasLineas.Remove(linea);
            }

            _contexto.Facturas.Remove(Factura);

           return  await GuardarCambiosAsync();
        }

       
        public async Task<int> GuardarCambiosAsync()
        {
            return await _contexto.SaveChangesAsync();
        }

        public async Task<Factura> BuscarFacturaAsync(int? idFactura)
        {
            return await _contexto.Facturas.Include(m => m.Lineas).FirstOrDefaultAsync(m => m.Id == idFactura);
        }


        public void Dispose()
        {
            _contexto.Dispose();
        }

        #region Lineas de factura

        private void ModificarFactura(EditorFactura editor)
        {
            ModificarCabeceraFactura(editor);
            ModificarLineasFactura(editor.Lineas);
        }

        private void ModificarCabeceraFactura(EditorFactura editor)
        {
            Factura.InjectFrom(editor);
        }

        private void ModificarLineasFactura(ICollection<EditorLineaFactura> lineasEditor)
        {
            foreach (var lineaEditor in lineasEditor)
            {
                var lineaFactura = Factura.Lineas.FirstOrDefault(m => lineaEditor.Id > 0 && m.Id == lineaEditor.Id);

                if (lineaFactura != null)
                {
                    if (lineaEditor.EstaMarcadoParaEliminar)
                    {
                        Factura.Lineas.Remove(lineaFactura);
                        _contexto.FacturasLineas.Remove(lineaFactura);
                    }
                    else
                        lineaFactura.InjectFrom(lineaEditor);
                }
                else if (!lineaEditor.EstaMarcadoParaEliminar)
                {
                    lineaFactura = new LineaFactura();
                    lineaFactura.InjectFrom(lineaEditor);
                    Factura.Lineas.Add(lineaFactura);
                }
            }
        }

        #endregion
    }
}
