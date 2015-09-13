using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using GestionFacturas.Modelos;
using GestionFacturas.Servicios;
using System.Linq;
using GestionFacturas.Website.Viewmodels.Clientes;
using System.IO;
using System.Collections.Generic;

namespace GestionFacturas.Website.Controllers
{
    [Authorize]
    public class ClientesController : Controller
    {
        private readonly ServicioCliente _servicioCliente;

        public ClientesController(ServicioCliente servicioCliente)
        {
            _servicioCliente = servicioCliente;
        }

        public ActionResult Index()
        {
            return RedirectToAction("ListaGestionClientes");
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public async Task<ActionResult> ListaGestionClientes(FiltroBusquedaCliente filtroBusqueda)
        {
            if (!filtroBusqueda.TieneValores)
            {
                filtroBusqueda = FiltroBusquedaConValoresPorDefecto();
            }

            var viewmodel = new ListaGestionClientesViewModel
            {
                FiltroBusqueda = filtroBusqueda,
                ListaClientes = (await _servicioCliente.ListaGestionClientesAsync(filtroBusqueda))
                    .OrderBy(m => m.NombreOEmpresa)
            };

            return View("ListaGestionClientes", viewmodel);
        }

        public ActionResult Crear()
        {
            var viewmodel = new CrearClienteViewModel
            {
                Cliente = new EditorCliente()
            };
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Crear(CrearClienteViewModel viewmodel)
        {
            if (!ModelState.IsValid) return View(viewmodel);

            await _servicioCliente.CrearClienteAsync(viewmodel.Cliente);

            return RedirectToAction("ListaGestionClientes", new { Id = viewmodel.Cliente.Id });

        }

        public async Task<ActionResult> Editar(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var viewmodel = new EditarClienteViewModel
            {
                Cliente = await _servicioCliente.BuscaEditorClienteAsync(id)
            };

            if (viewmodel.Cliente == null) return HttpNotFound();

            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Editar(EditarClienteViewModel viewmodel)
        {
            if (!ModelState.IsValid) return View(viewmodel);

            await _servicioCliente.ActualizarClienteAsync(viewmodel.Cliente);
            return RedirectToAction("ListaGestionClientes", new { Id = viewmodel.Cliente.Id });
        }

        public async Task<ActionResult> Eliminar(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var viewmodel = new EliminarClienteViewModel
            {
                Cliente = await _servicioCliente.BuscaEditorClienteAsync(id.Value)
            };

            if (viewmodel.Cliente == null) return HttpNotFound();

            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Eliminar(EliminarClienteViewModel viewmodel)
        {
            if (!ModelState.IsValid) return View(viewmodel);

            await _servicioCliente.EliminarCliente(viewmodel.Cliente.Id);

            var nombreOEmpresaCodificado = WebUtility.UrlEncode(viewmodel.Cliente.NombreOEmpresa);

            return RedirectToAction("EliminarConfirmado", new { nombreOEmpresaEliminado = nombreOEmpresaCodificado });
        }

        public ActionResult EliminarConfirmado(string nombreOEmpresaEliminado)
        {
            if (string.IsNullOrEmpty(nombreOEmpresaEliminado)) return HttpNotFound();

            ViewBag.NombreOEmpresaEliminado = WebUtility.UrlDecode(nombreOEmpresaEliminado);

            return View("EliminarConfirmado");
        }

        public ActionResult Importar()
        {
            var viewmodel = new ImportarClientesViewModel {
                LetrasColumnasCliente = new EditorColumnasExcelCliente()
            };
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Importar(ImportarClientesViewModel viewmodel)
        {
            if (!ModelState.IsValid) return View(viewmodel);
            
            await _servicioCliente.ImportarClientesDeExcel(viewmodel.ArchivoExcelSeleccionado.InputStream, viewmodel.LetrasColumnasCliente);

            return RedirectToAction("ListaGestionClientes");

        }

        #region Acciones de autocompletar

        public async Task<ActionResult> AutocompletarPorNombre(string term)
        {
            var filtroBusqueda = new FiltroBusquedaCliente { NombreOEmpresa = term };
            var clientes = await _servicioCliente.ListaClientesAsync(filtroBusqueda, 1, 10);
            return Json(clientes, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> AutocompletarPorId(int term)
        {
            var filtroBusqueda = new FiltroBusquedaCliente { Id = term };
            var clientes = await _servicioCliente.ListaClientesAsync(filtroBusqueda, 1, 10);
            return Json(clientes, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> AutocompletarPorIdentificacionFiscal(string term)
        {
            var filtroBusqueda = new FiltroBusquedaCliente { IdentificacionFiscal = term };
            var clientes = await _servicioCliente.ListaClientesAsync(filtroBusqueda, 1, 10);
            return Json(clientes, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Métodos Privados

        private FiltroBusquedaCliente FiltroBusquedaConValoresPorDefecto()
        {
            return new FiltroBusquedaCliente();           
        }

        

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _servicioCliente.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
