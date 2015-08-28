using GestionFacturas.Datos;
using GestionFacturas.Modelos;
using GestionFacturas.Servicios;
using GestionFacturas.Website.Controllers;
using GestionFacturas.Website.Viewmodels.Facturas;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GestionFacturas.Tests.UI
{
    [TestClass]
    public class TestFacturasController
    {
        private ContextoBaseDatos _contexto;
      
        [TestMethod]
        public void Get_ListaGestionFacturas_EsOk()
        {
            // Arrange
            var controller = ObtenerFacturasControllador();
           
            // Act
            var result = controller.ListaGestionFacturas().Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ActionResult));
            Assert.IsInstanceOfType((ListaGestionFacturasViewModel)((ViewResult)result).ViewData.Model, typeof(ListaGestionFacturasViewModel));
        }

        [TestMethod]
        public void Get_Detalles_sin_indicar_idfactura_EsPeticionIncorrecta()
        {
            // Arrange
            var controller = ObtenerFacturasControllador();

            // Act
            var result = controller.Detalles(null).Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(((HttpStatusCodeResult)result).StatusCode == (int)HttpStatusCode.BadRequest);

            controller.Dispose();
        }

        [TestMethod]
        public void Get_Detalles_EsOk()
        {
            // Arrange
            var controller = ObtenerFacturasControllador();
            var primerFacturaConLineas = _contexto.Facturas.FirstOrDefault(m => m.Lineas.Any());

            if (primerFacturaConLineas == null) return;

            // Act
            var result = controller.Detalles(primerFacturaConLineas.Id).Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(((ViewResult)result).Model, typeof(DetallesFacturaViewModel));

            controller.Dispose();
        }

        [TestMethod]
        public void Crear_HttpGet_EsOk()
        {
            // Arrange
            var controller = ObtenerFacturasControllador();

            // Act
            var result = controller.Crear();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ActionResult));

            controller.Dispose();
        }

        [TestMethod]
        public void Editar_HttpGet_EsOk()
        {
            // Arrange
            var controller = ObtenerFacturasControllador();

            // Act
            var result = controller.Editar(1).Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ActionResult));
        }

        [TestMethod]
        public void Eliminar_HttpGet_EsOk()
        {
            // Arrange
            var controller = ObtenerFacturasControllador();

            // Act
            var result = controller.Eliminar(1).Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ActionResult));
        }
               

        private FacturasController ObtenerFacturasControllador()
        {
            _contexto = new ContextoBaseDatos();
            return new FacturasController(new ServicioFactura(_contexto));
        }

    }
}
