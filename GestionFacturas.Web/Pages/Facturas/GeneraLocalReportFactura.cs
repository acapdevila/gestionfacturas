using GestionFacturas.Aplicacion;
using GestionFacturas.Dominio;
using Microsoft.Reporting.NETCore;

namespace GestionFacturas.Web.Pages.Facturas
{
    public class GeneraLocalReportFactura
    {
        
        public static LocalReport GenerarInformeLocalFactura(Factura factura, string webRootPath)
        {
            var rutaReport = Path.Combine(webRootPath, "Content\\informes", "Factura.rdlc");

            var informeLocal = new LocalReport
            {
                ReportPath = rutaReport,
                EnableExternalImages = true
            };
            
            var datasetFactura = factura.ConvertirADataSet(webRootPath);

            informeLocal.DataSources.Add(new ReportDataSource("Facturas", datasetFactura.Tables[0]));
            informeLocal.DataSources.Add(new ReportDataSource("Lineas", datasetFactura.Tables[1]));

            return informeLocal;
        }
    }
}
