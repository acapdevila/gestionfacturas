using GestionFacturas.Aplicacion;
using GestionFacturas.Dominio;
using Microsoft.Reporting.NETCore;

namespace GestionFacturas.Web.Pages.Facturas
{
    public static class GeneraLocalReportFactura
    {
        
        public static LocalReport GenerarInformeLocalFactura(Factura factura, string webRootPath)
        {
            var plantillaRdlc = string.IsNullOrEmpty(factura.NombreArchivoPlantillaInforme)
                                                    ? "Factura.rdlc"
                                                    : factura.NombreArchivoPlantillaInforme;

            var rutaReport = Path.Combine(webRootPath, "informes", plantillaRdlc);

            var informeLocal = new LocalReport
            {
                ReportPath = rutaReport,
                EnableExternalImages = true
            };
            
            var datasetFactura = ConvertirADataSet(factura);

            informeLocal.DataSources.Add(new ReportDataSource("Facturas", datasetFactura.Tables[0]));
            informeLocal.DataSources.Add(new ReportDataSource("Lineas", datasetFactura.Tables[1]));

            return informeLocal;
        }

        private static DataSetFactura ConvertirADataSet(Factura factura)
        {
            var datasetFactura = new DataSetFactura();
            
            var filaDatasetFactura = datasetFactura.Facturas.NewFacturasRow();

            filaDatasetFactura.InyectarFactura(factura);

            datasetFactura.Facturas.AddFacturasRow(filaDatasetFactura);

            foreach (var linea in factura.Lineas)
            {
                var filaDatasetLineaFactura = datasetFactura.FacturasLineas.NewFacturasLineasRow();

                filaDatasetLineaFactura.Id = linea.Id;
                filaDatasetLineaFactura.IdFactura = linea.IdFactura;
                filaDatasetLineaFactura.Descripcion = linea.Descripcion;
                filaDatasetLineaFactura.Cantidad = linea.Cantidad;
                filaDatasetLineaFactura.PrecioUnitario = linea.PrecioUnitario;
                filaDatasetLineaFactura.PorcentajeImpuesto = linea.PorcentajeImpuesto;

                datasetFactura.FacturasLineas.AddFacturasLineasRow(filaDatasetLineaFactura);
            }

            return datasetFactura;
        }
    }
}
