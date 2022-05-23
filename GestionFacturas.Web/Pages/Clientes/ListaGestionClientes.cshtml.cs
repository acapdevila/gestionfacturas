using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GestionFacturas.Web.Pages.Clientes
{
    public class ListaGestionClientesModel : PageModel
    {
        public const string NombrePagina = $"/{nameof(Clientes)}/ListaGestionClientes";

        [BindProperty(SupportsGet = true)]
        public string BuscarPor { get; set; } = string.Empty;

        
        public void OnGet()
        {
        }
    }
}
