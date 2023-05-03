using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GestionFacturas.Aplicacion;
using GestionFacturas.Dominio;
using static GestionFacturas.Dominio.CambiarEstadoFactura;

namespace GestionFacturas.Web.Pages.Facturas
{
    
    public class EditarFacturaModel : PageModel
    {
        public const string NombrePagina = @"/Facturas/EditarFactura";

        private readonly ServicioCrudFactura _servicioFactura;

        public EditarFacturaModel(ServicioCrudFactura servicioFactura)
        {
            _servicioFactura = servicioFactura;
        }

        [BindProperty]
        public EditorFactura Editor { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var factura = await _servicioFactura.BuscarFacturaAsync(id);
            this.Editor = new EditorFactura(factura);
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
