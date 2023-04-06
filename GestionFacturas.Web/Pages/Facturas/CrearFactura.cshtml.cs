using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GestionFacturas.Dominio;
using GestionFacturas.Aplicacion;

namespace GestionFacturas.Web.Pages.Facturas
{
    public class CrearFacturaModel : PageModel
    {
        public static readonly string NombrePagina = "/Facturas/CrearFactura";
        
        private readonly ServicioFactura _servicioFactura;

        public CrearFacturaModel(ServicioFactura servicioFactura)
        {
            _servicioFactura = servicioFactura;
        }

        public async Task<IActionResult> OnGet()
        {
            Editor =
                await _servicioFactura
                    .ObtenerEditorFacturaParaCrearNuevaFactura(serie: string.Empty, idCliente: IdCliente);
            
            return Page();
        }
        [BindProperty(SupportsGet = true)]
        public int? IdCliente { get; set; }

        [BindProperty]
        public EditorFactura Editor { get; set; } 



        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Editor.IdUsuario = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _servicioFactura.CrearFacturaAsync(Editor);
            return RedirectToAction("Detalles", new { Id = Editor.Id });
        }
        
    }
    
}
