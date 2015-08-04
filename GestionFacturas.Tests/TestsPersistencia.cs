using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GestionFacturas.Modelos;
using System.Collections.Generic;
using GestionFacturas.Website;
using GestionFacturas.Datos;
using System.Data.Entity;
using System.Linq;
using GestionFacturas.Servicios;

namespace GestionFacturas.Tests
{
    [TestClass]
    public class TestsPersistencia
    {

        private int PorcentajeIva { get { return 21; } }
        
        private Factura FacturaEjemploConTresLineas(Usuario usuario)
        {
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
                    new LineaFactura { PorcentajeImpuesto = PorcentajeIva, PrecioUnitario = 20, Cantidad = 2 },
                    new LineaFactura { PorcentajeImpuesto = PorcentajeIva, PrecioUnitario = (decimal)23.5, Cantidad = 1 },
                    new LineaFactura { PorcentajeImpuesto = PorcentajeIva, PrecioUnitario = 4, Cantidad = 2 }
                }
            };
        }

        [TestMethod]
        public void Crear_Obterer_Actualizar_Borrar_Factura()
        {
            var contexto = new ContextoBaseDatos();
            var servicioFactura = new ServicioCrudFactura(contexto);

            var usuario = contexto.Users.FirstOrDefault();
            var factura = FacturaEjemploConTresLineas(usuario);

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




    }
}
