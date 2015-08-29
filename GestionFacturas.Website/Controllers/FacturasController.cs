using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using GestionFacturas.Modelos;
using GestionFacturas.Servicios;
using GestionFacturas.Website.Viewmodels.Facturas;

namespace GestionFacturas.Website.Controllers
{
    [Authorize]
    public class FacturasController : Controller
    {
        private readonly ServicioFactura _servicioFactura;
      
        public FacturasController(ServicioFactura servicioFactura)
        {
            _servicioFactura = servicioFactura;
        }

        public ActionResult Index()
        {
            return RedirectToAction("ListaGestionFacturas");
        }

        public async Task<ActionResult> ListaGestionFacturas()
        {            
            var viewmodel = new ListaGestionFacturasViewModel {
                 ListaFacturas = await _servicioFactura.ListaGestionFacturasAsync()
            };

            return View("ListaGestionFacturas", viewmodel);
        }

        public async Task<ActionResult> Detalles(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            var viewmodel = new DetallesFacturaViewModel
            {
                Factura = await _servicioFactura.BuscarVisorFacturaAsync(id)
            };
            
            if (viewmodel.Factura == null) return HttpNotFound();

            return View(viewmodel);
        }

        public ActionResult Crear()
        {
            return View();
        }

        // POST: Facturas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Crear([Bind(Include = "Id,IdUsuario,SerieFactura,NumeracionFactura,FormatoNumeroFactura,FechaEmisionFactura,FechaVencimientoFactura,IdVendedor,VendedorNumeroIdentificacionFiscal,VendedorNombreOEmpresa,VendedorDireccion,VendedorLocalidad,VendedorProvincia,VendedorCodigoPostal,IdComprador,CompradorNumeroIdentificacionFiscal,CompradorNombreOEmpresa,CompradorDireccion,CompradorLocalidad,CompradorProvincia,CompradorCodigoPostal,EstadoFactura,Comentarios,ComentariosPie")] EditorFactura factura)
        {
            if (!ModelState.IsValid) return View(factura);

            await _servicioFactura.CrearFacturaAsync(factura);
            return RedirectToAction("Index");

        }

        public async Task<ActionResult> Editar(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var viewmodel = new EditarFacturaViewModel
            {
                Factura = await _servicioFactura.BuscaFacturaEditorAsync(id)
            };
            
            if (viewmodel.Factura == null) return HttpNotFound();
            
            return View(viewmodel);
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Editar(EditarFacturaViewModel viewmodel)
        {
            if (!ModelState.IsValid) return View(viewmodel);
            
            await _servicioFactura.ActualizarFacturaAsync(viewmodel.Factura);
            return RedirectToAction("Detalles", new { Id = viewmodel.Factura.Id });
        }

        public async Task<ActionResult> Eliminar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var factura = await _servicioFactura.BuscaFacturaEditorAsync(id.Value);
            if (factura == null)
            {
                return HttpNotFound();
            }
            return View(factura);
        }

        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EliminarConfirmacion(int id)
        {
            await _servicioFactura.EliminarFactura(id);
          
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _servicioFactura.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
