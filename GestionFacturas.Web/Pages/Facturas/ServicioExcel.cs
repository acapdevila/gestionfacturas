﻿using ClosedXML.Excel;
using GestionFacturas.Aplicacion;
using GestionFacturas.Dominio;
using GestionFacturas.Dominio.Infra;

namespace GestionFacturas.Web.Pages.Facturas
{
    public static class ServicioExcel
    {
         public static XLWorkbook GenerarExcelFactura(GridParamsFacturas filtroBusqueda, IEnumerable<LineaListaGestionFacturas> facturas)
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Facturación");

            worksheet.Name = "Facturación";

            worksheet.Range("A1:E1").Style.Font.SetBold();

            if (!string.IsNullOrEmpty(filtroBusqueda.Desde) &&
                !string.IsNullOrEmpty(filtroBusqueda.Hasta))
                worksheet.Range("A1:E1").Merge().Value =
                    $"Facturación entre {filtroBusqueda.Desde} y {filtroBusqueda.Hasta}";


            //cabecera
            worksheet.Cell("A3").InsertData(new[]
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
            });

            worksheet.Range("A3:G3").Style
                 .Font.SetFontSize(13)
                 .Font.SetBold(true)
                 .Font.SetFontColor(XLColor.White)
                 .Fill.SetBackgroundColor(XLColor.Gray)
                 .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);


            //Lineas
            var row = 4;
            var col = 1;
            foreach (var factura in facturas.OrderBy(m => m.FechaEmisionFacturaDateTime))
            {

                worksheet.Cell(row, col).Value = factura.FechaEmisionFacturaDateTime.ToShortDateString();
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
                worksheet.Cell(row, col).Value = factura.DescripcionPrimeraLinea.TruncarConElipsis(70);
                col++;
                //worksheet.Cell(row, col).SetDataType() = XLDataType.Number;
                worksheet.Cell(row, col).Value = factura.BaseImponible;
                worksheet.Cell(row, col).Style.NumberFormat.SetFormat("#,##0.00 €");

                col++;
                //worksheet.Cell(row, col).DataType = XLDataType.Number;
                worksheet.Cell(row, col).Value = factura.Impuestos;
                worksheet.Cell(row, col).Style.NumberFormat.SetFormat("#,##0.00 €");
                col++;
                //worksheet.Cell(row, col).DataType = XLDataType.Number;
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