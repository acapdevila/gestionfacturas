using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GestionFacturas.AccesoDatosSql;
using GestionFacturas.Dominio;

namespace GestionFacturas.Web.Pages.Facturas
{
    public class ListaGestionFacturasModel : PageModel
    {
        public static readonly string NombrePagina = "/Facturas/ListaGestionFacturas";

        [BindProperty(SupportsGet = true)]
        public string BuscarPor { get; set; } = string.Empty;
        
        public string Desde { get; set; } = string.Empty;

        public string Hasta { get; set; } = string.Empty;

        public int Orden { get; set; } 

        public void OnGet()
        {
        }
    }
}
