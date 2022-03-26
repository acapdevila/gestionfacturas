using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GestionFacturas.Dominio;
using Omu.ValueInjecter;

namespace GestionFacturas.Aplicacion
{
    public static class ConversorFacturas
    {
   
        public static DataSetFactura ConvertirADataSet(this Factura factura, string urlRaizWeb)
        {
            var datasetFactura = new DataSetFactura();

            datasetFactura.InyectarFactura(factura, urlRaizWeb);         

            return datasetFactura;
        }
    }
}
