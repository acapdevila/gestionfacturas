using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GestionFacturas.Aplicacion;
using GestionFacturas.Dominio;

namespace GestionFacturas.Web.Pages.Facturas
{
    
    public class EliminarFacturaModel : PageModel
    {
        public const string NombrePagina = @"/Facturas/EliminarFactura";

        private readonly ServicioFactura _servicioFactura;

        public EliminarFacturaModel(ServicioFactura servicioFactura)
        {
            _servicioFactura = servicioFactura;
        }

        [BindProperty]
        public EditorFactura Editor { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Editor = await _servicioFactura.BuscaEditorFacturaAsync(id);
            return Page();
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            

            await _servicioFactura.EliminarFactura(Editor.Id);
            return RedirectToPage("/Facturas/EliminarFacturaConfirmado");
        }
    }
}
