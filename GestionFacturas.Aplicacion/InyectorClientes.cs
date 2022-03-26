using GestionFacturas.Dominio;
using Omu.ValueInjecter;

namespace GestionFacturas.Aplicacion
{
    public static class InyectorClientes
    {

        public static void InyectarCliente(this EditorCliente editor, Cliente cliente)
        {
            editor.InjectFrom(cliente);       
              
        }

    }
}
