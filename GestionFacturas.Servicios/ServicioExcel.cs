using System.Globalization;
using System.Linq;
using ClosedXML.Excel;
using System.Collections.Generic;
using GestionFacturas.Modelos;

namespace GestionFacturas.Servicios
{
    public static class ServicioExcel
    {
         public static XLWorkbook GenerarExcelFactura(FiltroBusquedaFactura filtroBusqueda, IEnumerable<LineaListaGestionFacturas> facturas)
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Facturación");

            worksheet.Name = "Facturación";

            worksheet.Range("A1:E1").Style.Font.SetBold();

            if (filtroBusqueda.FechaDesde.HasValue && filtroBusqueda.FechaHasta.HasValue)
                worksheet.Range("A1:E1").Merge().Value = string.Format("Facturación entre {0} y {1}",
                    filtroBusqueda.FechaDesde.Value.ToShortDateString(),
                    filtroBusqueda.FechaHasta.Value.ToShortDateString());


            //cabecera
            worksheet.Cell("A3").Value = new[]
            {
              new
              {
                  Fecha="FECHA",
                  //Dia="DIA",
                  //Mes = "MES",
                  //Año = "AÑO",
                  NumFactura = "Nº FACTURA",
                  Cliente = "NOMBRE",
                  Titulo ="CONCEPTO",
                  BaseImponible = "BASE IMPONIBLE",
                  CuotaIva ="CUOTA IVA",
                  Total ="TOTAL FACTURA"
              }  
            };

            worksheet.Range("A3:G3").Style
                 .Font.SetFontSize(13)
                 .Font.SetBold(true)
                 .Font.SetFontColor(XLColor.White)
                 .Fill.SetBackgroundColor(XLColor.Gray)
                 .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);


            //Lineas
            var row = 4;
            var col = 1;
            foreach (var factura in facturas.OrderBy(m => m.FechaEmisionFactura))
            {

                worksheet.Cell(row, col).Value = factura.FechaEmisionFactura.ToShortDateString();
                //col++;
                //worksheet.Cell(row, col).Value = factura.FechaEmisionFactura.Day;
                //col++;
                //worksheet.Cell(row, col).Value = factura.FechaEmisionFactura.Month;
                //col++;
                //worksheet.Cell(row, col).Value = factura.FechaEmisionFactura.Year;

                col++;
                worksheet.Cell(row, col).Value = factura.NumeroFactura;
                col++;
                worksheet.Cell(row, col).Value = factura.CompradorNombreOEmpresa;
                col++;
                worksheet.Cell(row, col).Value = factura.Conceptos.TruncarConElipsis(70);
                worksheet.Cell(row, col).Comment.AddText(factura.Conceptos);
                col++;
                worksheet.Cell(row, col).DataType = XLCellValues.Number;
                worksheet.Cell(row, col).Value = factura.BaseImponible;
                worksheet.Cell(row, col).Style.NumberFormat.SetFormat("#,##0.00 €");

                col++;
                worksheet.Cell(row, col).DataType = XLCellValues.Number;
                worksheet.Cell(row, col).Value = factura.Impuestos;
                worksheet.Cell(row, col).Style.NumberFormat.SetFormat("#,##0.00 €");
                col++;
                worksheet.Cell(row, col).DataType = XLCellValues.Number;
                worksheet.Cell(row, col).Value = factura.ImporteTotal;
                worksheet.Cell(row, col).Style.NumberFormat.SetFormat("#,##0.00 €");
                row++;
                col = 1;
            }

            worksheet.Columns().AdjustToContents().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            worksheet.Column("D").Style
               .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

            return workbook;

        }
    }
}