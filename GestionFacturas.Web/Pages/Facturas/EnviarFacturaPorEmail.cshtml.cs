using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GestionFacturas.Aplicacion;
using System.Net;
using GestionFacturas.Dominio;
using GestionFacturas.Web.Pages.Shared;

namespace GestionFacturas.Web.Pages.Facturas
{
    
    public class EnviarFacturaPorEmailModel : PageModel
    {
        public const string NombrePagina = @"/Facturas/EnviarFacturaPorEmail";

        private readonly ServicioFactura _servicioFactura;
        private readonly IWebHostEnvironment _env;
        

        public EnviarFacturaPorEmailModel(ServicioFactura servicioFactura, IWebHostEnvironment env)
        {
            _servicioFactura = servicioFactura;
            _env = env;
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public string NumeroFactura { get; set; } = string.Empty;

        [BindProperty] public EditorEmail EditorEmail { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            var factura = await _servicioFactura.BuscaEditorFacturaAsync(Id);

            Id = factura.Id;

            NumeroFactura = factura.NumeroFactura;

            EditorEmail = new EditorEmail
            {
                Remitente = factura.VendedorEmail,
                Asunto = $"Factura {factura.NumeroFactura}",
                ContenidoHtml = @"Hola,",
                Destinatario = factura.CompradorEmail
            };
           

            return Page();
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var factura = await _servicioFactura.BuscarFacturaAsync(Id);

            if (factura is null)
                return NotFound();

            var mensaje = GenerarMensajeEmail(EditorEmail, factura);

            _servicioFactura.EnviarFacturaPorEmail(mensaje, factura);

            var numeroFacturaCodificada = WebUtility.UrlEncode(factura.NumeroFactura);

            return RedirectToPage(EnviarFacturaPorEmailModelConfirmadoModel.NombrePagina,new { numeroFacturaEnviada = numeroFacturaCodificada } );
        }

        private MensajeEmail GenerarMensajeEmail(EditorEmail editorEmail, Factura factura)
        {
            var informeLocal = GeneraLocalReportFactura.GenerarInformeLocalFactura(factura, _env.WebRootPath);

            byte[] pdf = informeLocal.Render("PDF");
            
            var mensaje = new MensajeEmail
            {
                Asunto = editorEmail.Asunto,
                Cuerpo = editorEmail.ContenidoHtml,
                DireccionRemitente = editorEmail.Remitente,
                NombreRemitente = factura.VendedorNombreOEmpresa,
                DireccionesDestinatarios = new List<string> { editorEmail.Destinatario },
                Adjuntos = new List<ArchivoAdjunto> {
                    new ArchivoAdjunto
                    {
                        Archivo = pdf,
                        MimeType = "application/pdf",
                        Nombre = factura.Titulo() + ".pdf"
                    }
                }
            };

            return mensaje;
        }
    }

    
}
