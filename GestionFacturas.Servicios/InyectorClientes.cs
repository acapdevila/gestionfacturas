using GestionFacturas.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Omu.ValueInjecter;

namespace GestionFacturas.Servicios
{
    public static class InyectorClientes
    {

        public static void InyectarCliente(this EditorCliente editor, Cliente cliente)
        {
            editor.InjectFrom(cliente);       
              
        }

    }
}
