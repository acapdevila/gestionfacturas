using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GestionFacturas.Dominio;
using GestionFacturas.Aplicacion;
using static GestionFacturas.Dominio.CambiarEstadoFactura;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.InkML;
using GestionFacturas.AccesoDatosSql;
using Microsoft.EntityFrameworkCore;

namespace GestionFacturas.Web.Pages.Facturas
{
    public class CrearFacturaModel : PageModel
    {
        public static readonly string NombrePagina = "/Facturas/CrearFactura";

        private readonly ServicioCrudFactura _servicio;
        private readonly SqlDb _db;

        public string HrefCancelar { get; set; } = string.Empty;

        public CrearFacturaModel( SqlDb db, ServicioCrudFactura servicio)
        {
            _db = db;
            _servicio = servicio;
        }

        public async Task<IActionResult> OnGet(int? id)
        {
            if (id.HasValue)
            {
                var factura = await _db.Facturas
                    .Include(m => m.Lineas)
                    .FirstAsync(m => m.Id == id.Value);
                
                var ultimaFacturaSerie = await _db.ObtenerUlitmaFacturaDeLaSerie(factura.SerieFactura);
                var cliente = await _db.Clientes.FindAsync(factura.IdComprador);
                
                Editor = new EditorFactura();
                Editor.GenerarNuevoEditorFacturaDuplicado(factura, ultimaFacturaSerie, cliente!);
                
                HrefCancelar = Url.Page(DetallesFacturaModel.NombrePagina, new { id = id.Value })!;
            }
            else
            {
                var ultimaFacturaSerie = await _db.ObtenerUlitmaFacturaDeLaSerie(string.Empty);
                var cliente = await _db.Clientes.FindAsync(IdCliente);
                
                Editor.ObtenerEditorFacturaParaCrearNuevaFactura(ultimaFacturaSerie, cliente);
                HrefCancelar = Url.Page(ListaGestionFacturasModel.NombrePagina)!;
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
            var factura = await _servicio.CrearFacturaAsync(Editor);
            
            return RedirectToPage(DetallesFacturaModel.NombrePagina, new { factura.Id });
        }
        
    }
    
}
