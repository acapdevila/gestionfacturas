using GestionFacturas.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Webdiyer.WebControls.Mvc;

namespace GestionFacturas.Website.Viewmodels.Facturas
{
    public class FacturasIndexViewModel
    {
        public IEnumerable<ItemListaFacturas> ListaFacturas { get; set; }
    }
}