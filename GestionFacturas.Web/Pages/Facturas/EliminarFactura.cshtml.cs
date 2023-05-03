using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GestionFacturas.Aplicacion;
using GestionFacturas.Dominio;

namespace GestionFacturas.Web.Pages.Facturas
{
    
    public class EliminarFacturaModel : PageModel
    {
        public const string NombrePagina = @"/Facturas/EliminarFactura";

        private readonly ServicioCrudFactura _servicioFactura;

        public EliminarFacturaModel(ServicioCrudFactura servicioFactura)
        {
            _servicioFactura = servicioFactura;
        }

        [BindProperty]
        public EditorFactura Editor { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var factura = await _servicioFactura.BuscarFacturaAsync(id);
            Editor = new EditorFactura(factura);
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
