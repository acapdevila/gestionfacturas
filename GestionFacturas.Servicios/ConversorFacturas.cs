using GestionFacturas.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Omu.ValueInjecter;

namespace GestionFacturas.Servicios
{
    public static class ConversorFacturas
    {
   
        public static DataSetFactura ConvertirADataSet(this Factura factura)
        {
            var datasetFactura = new DataSetFactura();

            datasetFactura.InyectarFactura(factura);

            return datasetFactura;
        }
    }
}
