using GestionFacturas.Modelos;
using GestionFacturas.Website.Viewmodels.Email;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Webdiyer.WebControls.Mvc;

namespace GestionFacturas.Website.Viewmodels.Clientes
{
    public class ListaGestionClientesViewModel
    {
        public FiltroBusquedaCliente FiltroBusqueda { get; set; }
        public IEnumerable<LineaListaGestionClientes> ListaClientes { get; set; }  

    }
    

    public class EditarClienteViewModel
    {
        public EditorCliente Cliente { get; set; }
    }
    public class CrearClienteViewModel
    {
        public EditorCliente Cliente { get; set; }  
    }
    public class ImportarClientesViewModel
    {
        public EditorCliente Cliente { get; set; }     
    }
    

    public class EliminarClienteViewModel
    {
        public EditorCliente Cliente { get; set; }
    }

    public class BuscadorClientesViewModel
    {
        public FiltroBusquedaCliente FiltroBusqueda { get; set; }
    }

  
}