using GestionFacturas.Datos;
using GestionFacturas.Modelos;
using GestionFacturas.Servicios;
using GestionFacturas.Website.Controllers;
using GestionFacturas.Website.Viewmodels.Facturas;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GestionFacturas.Tests.UI
{
    [TestClass]
    public class TestFacturasController
    {
      
        [TestMethod]
        public void ListaGestionFacturas_HttpGet_EsOk()
        {
            // Arrange
            var controller = ObtenerFacturasControllador();
           
            // Act
            var result = controller.ListaGestionFacturas().Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ActionResult));
            Assert.IsInstanceOfType((FacturasIndexViewModel)((ViewResult)result).ViewData.Model, typeof(FacturasIndexViewModel));
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
            return new FacturasController(new ServicioFactura(new ContextoBaseDatos()));
        }

    }
}
