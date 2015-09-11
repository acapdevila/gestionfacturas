using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using GestionFacturas.Modelos;
using GestionFacturas.Servicios;
using GestionFacturas.Website.Viewmodels.Facturas;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using System;
using Microsoft.Reporting.WebForms;
using GestionFacturas.Website.Helpers;
using System.Linq;
using System.IO;
using Ionic.Zip;
using GestionFacturas.Website.Viewmodels.Email;
using Elmah;

namespace GestionFacturas.Website.Controllers
{
    [Authorize]
    public class ClientesController : Controller
    {
        private readonly ServicioCliente _servicioCliente;

        public ClientesController(ServicioCliente servicioFactura)
        {
            _servicioCliente = servicioFactura;
        }
        
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
