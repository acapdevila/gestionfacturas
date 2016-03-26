using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GestionFacturas.Modelos;
using System.Collections.Generic;

namespace GestionFacturas.Tests.Modelos
{
    [TestClass]
    public class TestsFactura
    {        
        [TestMethod]
        public void CaluculaBaseImponible_FacturaConTresLineas()
        {
            var factura = FacturaEjemploConTresLineas();

            var baseImponible = factura.BaseImponible;

            Assert.AreEqual((decimal)71.5, baseImponible, @"La base imponible no se calcula correctamente");
        }

        [TestMethod]
        public void CaluculaImporteImpuestos_FacturaConTresLineas()
        {
            var factura = FacturaEjemploConTresLineas();

            var impuestos = factura.ImporteImpuestos;

            Assert.AreEqual((decimal)15.015, impuestos, @"Los impuestos de la factura no se calculan correctamente");
        }

        [TestMethod]
        public void CaluculaImporteTotal_FacturaConTresLineas()
        {
            var factura = FacturaEjemploConTresLineas();

            var importeTotal = factura.ImporteTotal;

            Assert.AreEqual((decimal)86.515, importeTotal, @"El importe total de la factura no se calcula correctamente");
        }

        [TestMethod]
        public void ElNumeroDeLaFactura_TieneUnFormatoCorrecto()
        {
            var factura = FacturaEjemploConTresLineas();

            var numeroFactura = factura.NumeroFactura;

            Assert.AreEqual("AC / 1012",numeroFactura, @"El número de la factura no se formatea correctamente");
        }


        private Factura FacturaEjemploConTresLineas()
        {
            var porcentajeIva = 21;

            return new Factura
            {
                FormatoNumeroFactura = "{0}{1:1000}",
                SerieFactura = "AC / ",
                NumeracionFactura = 12,
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
