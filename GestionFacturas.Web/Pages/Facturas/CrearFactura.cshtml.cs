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

        public async Task<IActionResult> OnGet(int? id)
        {
            if (id.HasValue)
            {
                Editor = await _servicioFactura.GenerarNuevoEditorFacturaDuplicado(id.Value);
            }
            else
            {
                Editor =
                    await _servicioFactura
                        .ObtenerEditorFacturaParaCrearNuevaFactura(serie: string.Empty, idCliente: IdCliente);
            }

            Editor.IdUsuario = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return Page();
        }
        [BindProperty(SupportsGet = true)]
        public int? IdCliente { get; set; }

        [BindProperty]
        public EditorFactura Editor { get; set; }  = new ();



        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

           

            await _servicioFactura.CrearFacturaAsync(Editor);
            return RedirectToPage(DetallesFacturaModel.NombrePagina, new { Editor.Id });
        }
        
    }
    
}
