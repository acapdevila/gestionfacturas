using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GestionFacturas.Aplicacion;
using GestionFacturas.Dominio;

namespace GestionFacturas.Web.Pages.Facturas
{
    
    public class EditarFacturaModel : PageModel
    {
        public const string NombrePagina = @"/Facturas/EditarFactura";

        private readonly ServicioFactura _servicioFactura;

        public EditarFacturaModel(ServicioFactura servicioFactura)
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

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            

            await _servicioFactura.ActualizarFacturaAsync(Editor);
            return RedirectToPage(DetallesFacturaModel.NombrePagina, new { Editor.Id });
        }
    }
}
