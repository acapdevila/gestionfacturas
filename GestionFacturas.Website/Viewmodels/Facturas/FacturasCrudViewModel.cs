using GestionFacturas.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Webdiyer.WebControls.Mvc;

namespace GestionFacturas.Website.Viewmodels.Facturas
{
    public class FacturasCrearViewModel
    {
        public EditorFactura Factura { get; set; }
    }

    public class FacturasEditarViewModel
    {
        public EditorFactura Factura { get; set; }
    }

    public class FacturasEliminarViewModel
    {
        public EditorFactura Factura { get; set; }
    }
}