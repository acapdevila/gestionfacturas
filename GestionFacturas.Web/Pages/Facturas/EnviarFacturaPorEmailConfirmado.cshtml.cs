using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GestionFacturas.Aplicacion;
using System.Net;

namespace GestionFacturas.Web.Pages.Facturas
{
    
    public class EnviarFacturaPorEmailModelConfirmadoModel : PageModel
    {
        public const string NombrePagina = @"/Facturas/EnviarFacturaPorEmailConfirmado";

        private readonly ServicioCrudFactura _servicioFactura;

        public EnviarFacturaPorEmailModelConfirmadoModel(ServicioCrudFactura servicioFactura)
        {
            _servicioFactura = servicioFactura;
        }

        public string NumeroFacturaEnviada { get; set; } = string.Empty;

        public IActionResult OnGet(string numeroFacturaEnviada)
        {
            NumeroFacturaEnviada = WebUtility.UrlDecode(numeroFacturaEnviada);
            return Page();
        }

    }
}
