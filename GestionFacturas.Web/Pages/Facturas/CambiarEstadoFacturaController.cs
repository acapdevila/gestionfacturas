using GestionFacturas.Aplicacion;
using GestionFacturas.Dominio;
using GestionFacturas.Dominio.Infra;
using GestionFacturas.Web.Pages.Facturas.EditorTemplates;
using Microsoft.AspNetCore.Mvc;

namespace GestionFacturas.Web.Pages.Facturas
{
    public class CambiarEstadoFacturaController : Controller
    {
        private readonly CambiarEstadoFacturaServicio _app;

        public CambiarEstadoFacturaController(CambiarEstadoFacturaServicio context)
        {
            _app = context;
        }

        [HttpPost]
        public async Task<ActionResult> CambiarEstado(EditorEstadoFactura editorEstadoFactura)
        {
            if (ModelState.IsValid)
            {
                var comando = new CambiarEstadoFacturaComando(
                    editorEstadoFactura.IdFactura, 
                    editorEstadoFactura.EstadoFactura);

               var ejecucion = await _app.Ejecutar(comando);

               if (ejecucion.IsFailure)
               {
                   return Json(new
                   {
                       editorEstadoFactura.IdFactura,
                       editorEstadoFactura.NumeroFactura,
                       editorEstadoFactura.EstadoFactura,
                       TextoEstadoFactura = "error"
                   });
                }
            }

            return Json(new
            {
                editorEstadoFactura.IdFactura,
                editorEstadoFactura.NumeroFactura,
                editorEstadoFactura.EstadoFactura,
                TextoEstadoFactura = editorEstadoFactura.EstadoFactura.ObtenerNombreAtributoDisplay()
            });
        }

    }
}
