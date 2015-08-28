using GestionFacturas.Datos;
using GestionFacturas.Modelos;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;

namespace GestionFacturas.Servicios
{
    public class ServicioCrudFactura
    {
        protected readonly ContextoBaseDatos _contexto;

        public ServicioCrudFactura(ContextoBaseDatos contexto)
        {
            _contexto = contexto;
        }
            

        public async Task<int> CrearFacturaAsync(EditorFactura editor)
        {
            var factura = new Factura();
            factura.InyectaEditorFactura(editor);

            _contexto.Facturas.Add(factura);

            var cambios = await GuardarCambiosAsync();
            
            editor.InyectaFactura(factura);

            return cambios;
        }
        
        public async Task<int> ActualizarFacturaAsync(EditorFactura editor)
        {
            var factura = await ObtenerFacturaAsync(editor.Id);
            factura.InyectaEditorFactura(editor);

            _contexto.Entry(factura).State = EntityState.Modified;
            return  await GuardarCambiosAsync();
         }

     

        public async Task<int> EliminarFactura(int idFactura)
        {
            var factura = await ObtenerFacturaAsync(idFactura);
            
            while (factura.Lineas.Any())
            {
                var linea = factura.Lineas.First();
                _contexto.LineasFacturas.Remove(linea);
            }

            _contexto.Facturas.Remove(factura);

           return  await GuardarCambiosAsync();
        }

       
        public async Task<int> GuardarCambiosAsync()
        {
            return await _contexto.SaveChangesAsync();
        }

        public async Task<Factura> ObtenerFacturaAsync(int idFactura)
        {
            return await _contexto.Facturas.Include(m => m.Lineas).FirstOrDefaultAsync(m => m.Id == idFactura);
        }


        public void Dispose()
        {
            _contexto.Dispose();
        }
    }
}
