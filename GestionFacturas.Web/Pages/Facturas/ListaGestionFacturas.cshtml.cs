using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GestionFacturas.Web.Pages.Facturas
{
    public class ListaGestionFacturasModel : PageModel
    {
        public static readonly string NombrePagina = "/Facturas/ListaGestionFacturas";

        [BindProperty(SupportsGet = true)]
        public string NombreOEmpresa { get; set; } = string.Empty;
        
        public string Desde { get; set; } = string.Empty;

        public string Hasta { get; set; } = string.Empty;

        public int Orden { get; set; } 

        public void OnGet()
        {
        }
    }
}
