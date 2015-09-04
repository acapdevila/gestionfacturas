using GestionFacturas.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Omu.ValueInjecter;

namespace GestionFacturas.Servicios
{
    public static class InyectorFacturas
    {

        public static void InyectarFactura(this EditorFactura editor, Factura factura)
        {
            editor.InjectFrom(factura);

            editor.PorcentajeIvaPorDefecto = 21;

            editor.BorrarLineasFactura();

            foreach (var lineaFactura in factura.Lineas)
            {
                var lineaEditor = new EditorLineaFactura();
                lineaEditor.InjectFrom(lineaFactura);
                editor.Lineas.Add(lineaEditor);
            }
        }

        public static void InyectarFactura(this VisorFactura visor, Factura factura)
        {
            visor.InjectFrom(factura);

            visor.BorrarLineasFactura();

            foreach (var lineaFactura in factura.Lineas)
            {
                var lineaVisor = new LineaVisorFactura();
                lineaVisor.InjectFrom(lineaFactura);
                visor.Lineas.Add(lineaVisor);
            }
        }

        public static void InyectarFactura(this DataSetFactura datasetFactura, Factura factura, string urlRaizWeb)
        {
            var filaDatasetFactura = datasetFactura.Facturas.NewFacturasRow();

            filaDatasetFactura.InyectarFactura(factura);

            if (string.IsNullOrEmpty(factura.NombreArchivoLogo))
                filaDatasetFactura.RutaLogo = string.Concat(urlRaizWeb, "Content/Logos/LogoGF.jpg");
            else
                filaDatasetFactura.RutaLogo = string.Concat(urlRaizWeb, "Uploads/Logos/", factura.NombreArchivoLogo);


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
        }

        public static void InyectarFactura(this DataSetFactura.FacturasRow fila, Factura factura)
        {            
            fila.Id = factura.Id;
            fila.RutaLogo = "/Content/Logos/LogoGF.jpg";

            fila.NumeroFactura = factura.NumeroFactura;
            fila.FechaEmisionFactura = factura.FechaEmisionFactura;

            if (factura.FechaVencimientoFactura.HasValue)
                fila.FechaVencimientoFactura = factura.FechaVencimientoFactura.Value;
            else
                fila.SetFechaVencimientoFacturaNull();

            fila.VendedorNumeroIdentificacionFiscal = factura.VendedorNumeroIdentificacionFiscal;
            fila.VendedorNombreOEmpresa= factura.VendedorNombreOEmpresa;
            fila.VendedorDireccion = factura.VendedorDireccion;
            fila.VendedorLocalidad = factura.VendedorLocalidad;
            fila.VendedorProvincia = factura.VendedorProvincia;
            fila.VendedorCodigoPostal = factura.VendedorCodigoPostal;

            fila.CompradorNumeroIdentificacionFiscal = factura.CompradorNumeroIdentificacionFiscal;
            fila.CompradorNombreOEmpresa = factura.CompradorNombreOEmpresa;
            fila.CompradorDireccion = factura.CompradorDireccion;
            fila.CompradorLocalidad = factura.CompradorLocalidad;
            fila.CompradorProvincia = factura.CompradorProvincia;
            fila.CompradorCodigoPostal = factura.CompradorCodigoPostal;

            fila.Comentarios = factura.Comentarios;
            fila.ComentariosPie = factura.ComentariosPie;

            fila.FormaPago = factura.FormaPago.ObtenerNombreAtributoDisplay();
            fila.FormaPagoDetalles = factura.FormaPagoDetalles;

            fila.BaseImponible = factura.BaseImponible();
            fila.ImporteImpuestos = factura.ImporteImpuestos();
            fila.ImporteTotal = factura.ImporteTotal();
        }
    }
}
