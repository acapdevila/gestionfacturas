using GestionFacturas.Aplicacion;
using GestionFacturas.Dominio;
using Microsoft.Reporting.NETCore;

namespace GestionFacturas.Web.Pages.Facturas
{
    public class GeneraLocalReportFactura
    {
        
        public static LocalReport GenerarInformeLocalFactura(Factura factura, string webRootPath)
        {
            var rutaReport = Path.Combine(webRootPath, "informes", "Factura.rdlc");

            var informeLocal = new LocalReport
            {
                ReportPath = rutaReport,
                EnableExternalImages = true
            };
            
            var datasetFactura = ConvertirADataSet(factura, webRootPath);

            informeLocal.DataSources.Add(new ReportDataSource("Facturas", datasetFactura.Tables[0]));
            informeLocal.DataSources.Add(new ReportDataSource("Lineas", datasetFactura.Tables[1]));

            return informeLocal;
        }

        private static DataSetFactura ConvertirADataSet(Factura factura, string urlRaizWeb)
        {
            var datasetFactura = new DataSetFactura();

            datasetFactura.InyectarFactura(factura, urlRaizWeb);

            return datasetFactura;
        }
    }
}
