using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GestionFacturas.Aplicacion;
using System.Net;
using CSharpFunctionalExtensions;
using GestionFacturas.AccesoDatosSql;
using GestionFacturas.Dominio;
using GestionFacturas.Web.Pages.Shared;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GestionFacturas.Web.Pages.Facturas
{
    
    public class EnviarFacturaPorEmailModel : PageModel
    {
        public const string NombrePagina = @"/Facturas/EnviarFacturaPorEmail";
        
        private readonly IWebHostEnvironment _env;
        private readonly SqlDb _db;
        private readonly MailSettings _mailSettings;
        private readonly IServicioEmail _email;
        

        public EnviarFacturaPorEmailModel(IWebHostEnvironment env, MailSettings mailSettings, SqlDb db, IServicioEmail email)
        {
            _env = env;
            _mailSettings = mailSettings;
            _db = db;
            _email = email;
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public string NumeroFactura { get; set; } = string.Empty;

        [BindProperty] public EditorEmail EditorEmail { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {

            var factura = await _db.Facturas
                .Include(m => m.Lineas)
                .FirstOrDefaultAsync(m => m.Id == Id);

            if (factura == null)
            {
                return NotFound();
            }
            

            Id = factura.Id;

            NumeroFactura = factura.NumeroFactura;

            EditorEmail = new EditorEmail
            {
                Remitente = factura.VendedorEmail ?? string.Empty,
                Asunto = $"Factura {factura.NumeroFactura}",
                ContenidoHtml = @"Hola,",
                Destinatarios = factura.CompradorEmail ?? string.Empty
            };

            CargarCombos();

            return Page();
        }

        private void CargarCombos()
        {
            EditorEmail.Remitentes = _mailSettings.ReplyToList().Select(m => new SelectListItem(m, m)).ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                CargarCombos();
                return Page();
            }

            var factura = await _db.Facturas
                .Include(m => m.Lineas)
                .FirstOrDefaultAsync(m => m.Id == Id);

            if (factura is null)
                return NotFound();

            var mensaje = GenerarMensajeEmail(EditorEmail, factura);

            if (mensaje.IsFailure)
            {
                ModelState.AddModelError("", mensaje.Error);
                CargarCombos();
                return Page();
            }

            await _email.EnviarMensajeAsync(mensaje.Value);

            factura.EstadoFactura = EstadoFacturaEnum.Enviada;
            await _db.SaveChangesAsync();
            

            var numeroFacturaCodificada = WebUtility.UrlEncode(factura.NumeroFactura);

            return RedirectToPage(EnviarFacturaPorEmailModelConfirmadoModel.NombrePagina,new { numeroFacturaEnviada = numeroFacturaCodificada } );
        }

        private Result<MensajeEmail> GenerarMensajeEmail(EditorEmail editorEmail, Factura factura)
        {
            var informeLocal = GeneraLocalReportFactura.GenerarInformeLocalFactura(factura, _env.WebRootPath);

            byte[] pdf = informeLocal.Render("PDF");
            
            var mensaje = new MensajeEmail(
                nombreRemitente: factura.VendedorNombreOEmpresa,
                direccionRemitente: editorEmail.Remitente,
                asunto: editorEmail.Asunto,
                cuerpo: editorEmail.ContenidoHtml
                )
            {
                Adjuntos = new List<ArchivoAdjunto> {
                    new ()
                    {
                        Archivo = pdf,
                        MimeType = "application/pdf",
                        Nombre = factura.Titulo() + ".pdf"
                    }
                }
            };

            var validacionDestinatarios = 
                            mensaje.AñadirDestinatarios(
                                       editorEmail.Destinatarios
                                           .Split(new[] { ';' },
                                               StringSplitOptions.RemoveEmptyEntries | 
                                                      StringSplitOptions.TrimEntries )
                                           .ToList());


            if (validacionDestinatarios.IsFailure) 
                return validacionDestinatarios.ConvertFailure<MensajeEmail>();

            return mensaje;
        }
    }

    
}
