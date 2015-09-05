using Microsoft.Reporting.WebForms;
using System;

namespace GestionFacturas.Servicios
{
    public class ServicioPdf
    {
        public ServicioPdf()
        {
            
        }

        public static Byte[] GenerarPdfFactura(LocalReport localReport, out string mimeType)
        {
            const string reportType = "PDF";

            string encoding;
            string fileNameExtension;

            //The DeviceInfo settings should be changed based on the reportType
            //http://msdn2.microsoft.com/en-us/library/ms155397.aspx
            const string deviceInfo = "<DeviceInfo>" +
                                      "  <OutputFormat>PDF</OutputFormat>" +
                                      "  <PageWidth>21cm</PageWidth>" +
                                      "  <PageHeight>25.7cm</PageHeight>" +
                                      "  <MarginTop>1cm</MarginTop>" +
                                      "  <MarginLeft>2cm</MarginLeft>" +
                                      "  <MarginRight>2cm</MarginRight>" +
                                      "  <MarginBottom>1cm</MarginBottom>" +
                                      "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;

            //Render the report
            return localReport.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);

        }
    }
}
