using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GestionFacturas.Modelos;
using System.Collections.Generic;
using GestionFacturas.Website;
using GestionFacturas.Datos;
using System.Data.Entity;
using System.Linq;
using GestionFacturas.Servicios;

namespace GestionFacturas.Tests.Servicios
{
    [TestClass]
    public class TestsServicioCrudFactura
    {
        private readonly ContextoBaseDatos _contexto = new ContextoBaseDatos();

        [TestMethod]
        public void Crear_Obterer_Actualizar_Borrar_Factura()
        {           
            var servicioFactura = new ServicioCrudFactura(_contexto);                       
            var factura = FacturaEjemploConTresLineas();

            // Crear
            servicioFactura.Crear(factura);
            var idFacturaCreada = factura.Id;
            
            Assert.IsTrue(idFacturaCreada > 0, "La factura no se ha creado en la base de datos");


            // Actualizar
            factura.FechaVencimientoFactura = factura.FechaVencimientoFactura.AddMonths(1);
            var cambios = servicioFactura.GuardarCambios();

            Assert.IsTrue(cambios > 0, "La factura no se ha podido actualizar en la base de datos");

            // Borrar
            servicioFactura.Eliminar(factura.Id);

            var facturaEliminada = servicioFactura.Obtener(factura.Id);

            Assert.IsNull(facturaEliminada, "La facrura no se ha podido eliminar de la base de datos");
        }
        
      

        private Factura FacturaEjemploConTresLineas()
        {
            var usuario = _contexto.Users.FirstOrDefault();
            var porcentajeIva = 21;

            return new Factura
            {
                IdUsuario = usuario.Id,
                FormatoNumeroFactura = "{0}{1:1000}",
                SerieFactura = "AC / ",
                NumeracionFactura = 12,
                FechaEmisionFactura = DateTime.Now,
                FechaVencimientoFactura = DateTime.Now,
                Lineas = new List<LineaFactura>
                {
                    new LineaFactura { PorcentajeImpuesto = porcentajeIva, PrecioUnitario = 20, Cantidad = 2 },
                    new LineaFactura { PorcentajeImpuesto = porcentajeIva, PrecioUnitario = (decimal)23.5, Cantidad = 1 },
                    new LineaFactura { PorcentajeImpuesto = porcentajeIva, PrecioUnitario = 4, Cantidad = 2 }
                }
            };
        }

    }
}
