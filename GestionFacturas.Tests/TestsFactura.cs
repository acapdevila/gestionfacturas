using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GestionFacturas.Modelos;
using System.Collections.Generic;

namespace GestionFacturas.Tests
{
    [TestClass]
    public class TestsFactura
    {
        private int PorcentajeIva { get { return 21; } }


        private Factura CreaFacturaConTresLineas()
        {
            return new Factura
            {
                FormatoNumeroFactura = "{0}{1:1000}",
                SerieFactura = "AC / ",
                NumeracionFactura = 12,
                Lineas = new List<LineaFactura>
                {
                    new LineaFactura { PorcentajeImpuesto = PorcentajeIva, PrecioUnitario = 20, Cantidad = 2 },
                    new LineaFactura { PorcentajeImpuesto = PorcentajeIva, PrecioUnitario = (decimal)23.5, Cantidad = 1 },
                    new LineaFactura { PorcentajeImpuesto = PorcentajeIva, PrecioUnitario = 4, Cantidad = 2 }
                }
            };
        }

        [TestMethod]
        public void CaluculaBaseImponible_FacturaConTresLineas()
        {
            var factura = CreaFacturaConTresLineas();

            var baseImponible = factura.BaseImponible();

            Assert.AreEqual((decimal)71.5, baseImponible, @"La base imponible no se calcula correctamente");
        }

        [TestMethod]
        public void CaluculaImporteImpuestos_FacturaConTresLineas()
        {
            var factura = CreaFacturaConTresLineas();

            var impuestos = factura.ImporteImpuestos();

            Assert.AreEqual((decimal)15.015, impuestos, @"Los impuestos de la factura no se calculan correctamente");
        }

        [TestMethod]
        public void CaluculaImporteTotal_FacturaConTresLineas()
        {
            var factura = CreaFacturaConTresLineas();

            var importeTotal = factura.ImporteTotal();

            Assert.AreEqual((decimal)86.515, importeTotal, @"El importe total de la factura no se calcula correctamente");
        }

        [TestMethod]
        public void ElNumeroDeLaFactura_TieneUnFormatoCorrecto()
        {
            var factura = CreaFacturaConTresLineas();

            var numeroFactura = factura.NumeroFactura;

            Assert.AreEqual("AC / 1012",numeroFactura, @"El número de la factura no se formatea correctamente");
        }

    }
}
