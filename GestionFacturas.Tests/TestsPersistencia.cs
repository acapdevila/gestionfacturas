using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GestionFacturas.Modelos;
using System.Collections.Generic;

namespace GestionFacturas.Tests
{
    [TestClass]
    public class TestsPersistencia
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
        public void Crear_Obterer_Actualizar_Borrar_Factura()
        {

        }




    }
}
