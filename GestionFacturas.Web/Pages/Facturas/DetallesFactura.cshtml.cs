using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GestionFacturas.Dominio;
using GestionFacturas.Aplicacion;

namespace GestionFacturas.Web.Pages.Facturas
{
    public class DetallesFacturaModel : PageModel
    {
        public const string NombrePagina = @"/Facturas/DetallesFactura";

        private readonly ServicioFactura _servicioFactura;

        public DetallesFacturaModel(ServicioFactura servicioFactura)
        {
            _servicioFactura = servicioFactura;
        }

      public VisorFactura Factura { get; set; } = default!;
   

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Factura = await _servicioFactura.BuscarVisorFacturaAsync(id);
            return Page();

        }
    }
}
