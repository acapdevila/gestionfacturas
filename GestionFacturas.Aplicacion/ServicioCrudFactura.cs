using GestionFacturas.AccesoDatosSql;
using GestionFacturas.Dominio;
using GestionFacturas.Dominio.Infra;
using Microsoft.EntityFrameworkCore;
using Omu.ValueInjecter;

namespace GestionFacturas.Aplicacion
{
    public class ServicioCrudFactura
    {
        
        protected readonly SqlDb _contexto;
        
        public ServicioCrudFactura(SqlDb contexto)
        {
            _contexto = contexto;
        }
            

        public async Task<Factura> CrearFacturaAsync(EditorFactura editor)
        {
            var factura = new Factura();

            var comprador = await _contexto
                                        .Clientes
                                        .FirstAsync(m => m.Id == editor.IdComprador);

            factura.Comprador = comprador;

            ModificarFactura(editor, factura);         

            _contexto.Facturas.Add(factura);

             await GuardarCambiosAsync();
            
            return factura;
        }
        
        public async Task<int> ActualizarFacturaAsync(EditorFactura editor)
        {
            var factura = await _contexto.Facturas
                .Include(m => m.Lineas)
                .FirstAsync(m => m.Id == editor.Id); 

            ModificarFactura(editor, factura);
                             
            _contexto.Entry(factura).State = EntityState.Modified;
            return  await GuardarCambiosAsync();
         }

        
        public async Task<int> EliminarFactura(int idFactura)
        {
            var factura = await _contexto.Facturas.Include(m => m.Lineas).FirstAsync(m => m.Id == idFactura);

            while (factura.Lineas.Any())
            {
                var linea = factura.Lineas.First();
                _contexto.FacturasLineas.Remove(linea);
            }

            _contexto.Facturas.Remove(factura);

           return  await GuardarCambiosAsync();
        }

       
        public async Task<int> GuardarCambiosAsync()
        {
            return await _contexto.SaveChangesAsync();
        }

        public async Task<Factura> BuscarFacturaAsync(int idFactura)
        {
            return await _contexto.Facturas
                        .Include(m => m.Lineas)
                        .FirstAsync(m => m.Id == idFactura);
        }
        

        public void Dispose()
        {
            _contexto.Dispose();
        }

        #region Lineas de factura

        private void ModificarFactura(EditorFactura editor, Factura factura)
        {
            ModificarCabeceraFactura(editor, factura);
            ModificarLineasFactura(editor.Lineas, factura);
        }

        private void ModificarCabeceraFactura(EditorFactura editor, Factura factura)
        {
            factura.InjectFrom(editor);

            factura.IdComprador = editor.IdComprador;
            factura.FechaEmisionFactura = editor.FechaEmisionFactura.FromInputToDateTime();
            factura.FechaVencimientoFactura = editor.FechaVencimientoFactura?.FromInputToDateTime();
        }

        private void ModificarLineasFactura(ICollection<EditorLineaFactura> lineasEditor, Factura factura)
        {
            foreach (var lineaEditor in lineasEditor)
            {
                var lineaFactura = factura.Lineas.FirstOrDefault(m => lineaEditor.Id > 0 && m.Id == lineaEditor.Id);

                if (lineaFactura != null)
                {
                    if (lineaEditor.EstaMarcadoParaEliminar)
                    {
                        factura.Lineas.Remove(lineaFactura);
                        _contexto.FacturasLineas.Remove(lineaFactura);
                    }
                    else
                        lineaFactura.InjectFrom(lineaEditor);
                }
                else if (!lineaEditor.EstaMarcadoParaEliminar)
                {
                    lineaFactura = new LineaFactura();
                    lineaFactura.InjectFrom(lineaEditor);
                    factura.Lineas.Add(lineaFactura);
                }
            }
        }

        #endregion
    }
}
