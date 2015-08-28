using GestionFacturas.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Webdiyer.WebControls.Mvc;

namespace GestionFacturas.Website.Viewmodels.Facturas
{
    public class ListaGestionFacturasViewModel
    {
        public IEnumerable<LineaListaGestionFacturas> ListaFacturas { get; set; }
    }

    public class DetallesFacturaViewModel
    {
        public VisorFactura Factura { get; set; }
    }
}