using GestionFacturas.AccesoDatosSql;
using GestionFacturas.Aplicacion;
using GestionFacturas.Dominio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionFacturas.Web.Pages.Facturas
{
    public class CambiarEstadoFacturaController : Controller
    {
        private readonly SqlDb _db;

        public CambiarEstadoFacturaController(SqlDb context)
        {
            _db = context;
        }

        [HttpPost]
        public async Task<ActionResult> CambiarEstado(EditorEstadoFactura editorEstadoFactura)
        {
            if (ModelState.IsValid)
            {
                var factura = await _db.Facturas.FirstAsync(m => m.Id == editorEstadoFactura.IdFactura);
                factura.EstadoFactura = editorEstadoFactura.EstadoFactura;
                await _db.SaveChangesAsync();
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
