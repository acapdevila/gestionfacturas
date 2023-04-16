using GestionFacturas.AccesoDatosSql;
using GestionFacturas.Dominio.Clientes;
using Microsoft.EntityFrameworkCore;

namespace GestionFacturas.Aplicacion
{
    public class ServicioCrudCliente
    {

        protected readonly SqlDb _contexto;

        private Cliente Cliente { get; set; }

        public ServicioCrudCliente(SqlDb contexto)
        {
            _contexto = contexto;
        }
            

        //public async Task<int> CrearClienteAsync(EditorCliente editor)
        //{
        //    Cliente = new Cliente();

        //    ModificarCliente(editor);         

        //    _contexto.Clientes.Add(Cliente);

        //    var cambios = await GuardarCambiosAsync();
            
        //    editor.InyectarCliente(Cliente);

        //    return cambios;
        //}

        //public async Task<int> CrearClientesAsync(List<EditorCliente> editores)
        //{
        //    foreach (var editor in editores)
        //    {
        //        Cliente = new Cliente();

        //        ModificarCliente(editor);

        //        _contexto.Clientes.Add(Cliente);
        //    }
        //    var cambios =   await GuardarCambiosAsync();

        //    return cambios;
        //}

        //public async Task<int> ActualizarClienteAsync(EditorCliente editor)
        //{
        //    Cliente = await BuscarClienteAsync(editor.Id);

        //    ModificarCliente(editor);
                             
        //    _contexto.Entry(Cliente).State = EntityState.Modified;
        //    return  await GuardarCambiosAsync();
        // }

        

        //public async Task<int> EliminarCliente(int idCliente)
        //{
        //    Cliente = await BuscarClienteAsync(idCliente);
            
        //    _contexto.Clientes.Remove(Cliente);

        //   return  await GuardarCambiosAsync();
        //}

       
        public async Task<int> GuardarCambiosAsync()
        {
            return await _contexto.SaveChangesAsync();
        }

        public async Task<Cliente?> BuscarClienteAsync(int? idCliente)
        {
            return await _contexto.Clientes.FirstOrDefaultAsync(m => m.Id == idCliente);
        }


        //public void Dispose()
        //{
        //    _contexto.Dispose();
        //}
        

        //private void ModificarCliente(EditorCliente editor)
        //{
        //    Cliente.InjectFrom(editor);
        //}

        
    }
}
