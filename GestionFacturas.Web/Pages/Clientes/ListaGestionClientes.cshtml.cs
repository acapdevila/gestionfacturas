using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GestionFacturas.Web.Pages.Clientes
{

    public class ListaGestionClientesModel : PageModel
    {
        public const string NombrePagina = @"/Clientes/ListaGestionClientes";
        
        [BindProperty(SupportsGet = true)]
        public string NombreOEmpresa { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public string Nif { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public int Ref { get; set; }
        
        public void OnGet()
        {

        }
        public void OnPost()
        {


        }
    }
}