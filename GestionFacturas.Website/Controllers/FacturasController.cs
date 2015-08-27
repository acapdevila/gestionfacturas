using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using GestionFacturas.Modelos;
using Webdiyer.WebControls.Mvc;
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
        

        public async Task<ActionResult> Index()
        {
            var facturas = await _servicioFactura.ListaFacturasAsync();

            var viewmodel = new FacturasIndexViewModel {
                 ListaFacturas = facturas.ToPagedList(1, 20)
            };

            return View("Index", viewmodel);
        }

        public async Task<ActionResult> Detalles(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var factura = await _servicioFactura.ObtenerFacturaAsync(id.Value);
            if (factura == null)
            {
                return HttpNotFound();
            }
            return View(factura);
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
            if (ModelState.IsValid)
            {
                await _servicioFactura.CrearFacturaAsync(factura);
                return RedirectToAction("Index");
            }
            
            return View(factura);
        }

        public async Task<ActionResult> Editar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var factura = await _servicioFactura.BuscaFacturaEditor(id);
            if (factura == null)
            {
                return HttpNotFound();
            }
            return View(factura);
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Editar([Bind(Include = "Id,IdUsuario,SerieFactura,NumeracionFactura,FormatoNumeroFactura,FechaEmisionFactura,FechaVencimientoFactura,IdVendedor,VendedorNumeroIdentificacionFiscal,VendedorNombreOEmpresa,VendedorDireccion,VendedorLocalidad,VendedorProvincia,VendedorCodigoPostal,IdComprador,CompradorNumeroIdentificacionFiscal,CompradorNombreOEmpresa,CompradorDireccion,CompradorLocalidad,CompradorProvincia,CompradorCodigoPostal,EstadoFactura,Comentarios,ComentariosPie")] EditorFactura factura)
        {
            if (ModelState.IsValid)
            {
                await _servicioFactura.ActualizarFacturaAsync(factura);
                return RedirectToAction("Index");
            }
            return View(factura);
        }

        public async Task<ActionResult> Eliminar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var factura = await _servicioFactura.BuscaFacturaEditor(id.Value);
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
