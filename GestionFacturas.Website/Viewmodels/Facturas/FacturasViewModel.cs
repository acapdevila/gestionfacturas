using GestionFacturas.Modelos;
using GestionFacturas.Website.Viewmodels.Email;
using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GestionFacturas.Website.Viewmodels.Facturas
{
    public class ListaGestionFacturasViewModel
    {
        public FiltroBusquedaFactura FiltroBusqueda { get; set; }
        public IPagedList<LineaListaGestionFacturas> ListaFacturas { get; set; }

        public TotalesFacturas Totales { get; set; }


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
    public class ImportarFacturasViewModel
    {
        public SelectorColumnasExcelFactura SelectorColumnasExcel { get; set; }

        [Display(Name = "Sólo migrar las facturas con clientes existentes")]
        public bool SoloImportarFacturasDeClientesExistentes { get; set; }
        
        public HttpPostedFileBase NombreArchivoExcel { get; set; }

        [Required]
        [Display(Name = "Excel")]
        public HttpPostedFileBase ArchivoExcelSeleccionado { get; set; }
        
     
    }




    public class EliminarFacturaViewModel
    {
        public EditorFactura Factura { get; set; }

        public string NombreArchivoLogoOriginal { get; set; }
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