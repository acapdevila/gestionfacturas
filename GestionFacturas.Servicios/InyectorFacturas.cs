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
    }
}
