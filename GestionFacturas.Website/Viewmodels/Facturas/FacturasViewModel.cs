using GestionFacturas.Modelos;
using GestionFacturas.Website.Viewmodels.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Webdiyer.WebControls.Mvc;

namespace GestionFacturas.Website.Viewmodels.Facturas
{
    public class ListaGestionFacturasViewModel
    {
        public FiltroBusquedaFactura FiltroBusqueda { get; set; }
        public IEnumerable<LineaListaGestionFacturas> ListaFacturas { get; set; }

        public decimal TotalBaseImponible { get { return ListaFacturas.Sum(m => m.BaseImponible); } }
        public decimal TotalImpuestos { get { return ListaFacturas.Sum(m => m.Impuestos); } }

        public decimal TotalImporte { get { return ListaFacturas.Sum(m => m.ImporteTotal); } }


    }

    public class DetallesFacturaViewModel
    {
        public VisorFactura Factura { get; set; }
    }

    public class EditarFacturaViewModel
    {
        public EditorFactura Factura { get; set; }
    }
    public class CrearFacturaViewModel
    {
        public EditorFactura Factura { get; set; }
    }

    public class EliminarFacturaViewModel
    {
        public EditorFactura Factura { get; set; }
    }

    public class BuscadorFacturasViewModel
    {
        public FiltroBusquedaFactura FiltroBusqueda { get; set; }
    }

    public class EnviarFacturaPorEmailViewModel
    {
        public int IdFactura { get; set; }
        public string NumeroFactura { get; set; }
        public EditorEmail EditorEmail { get; set; }
    }
}