using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hospi.Web.Pages
{
    public class CerrarSesionModel : PageModel
    {
        public CerrarSesionModel()
        {
            
        }

        public async Task<IActionResult> OnGet()
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                await HttpContext.SignOutAsync();
            }


            return RedirectToPage("/seguridad/acceso/entrar");

        }

        //public async Task<IActionResult> OnPost()
        //{
            
        //}
    }
}