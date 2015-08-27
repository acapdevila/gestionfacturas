using GestionFacturas.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;

namespace GestionFacturas.Servicios
{
    public static class InyectorFacturas
    {
        public static void InyectaEditorFactura(this Factura factura, EditorFactura editor)
        {
            factura.InjectFrom(editor);

            foreach (var lineaEditor in editor.Lineas)
            {
                var lineaFactura = factura.Lineas.FirstOrDefault(m=> m.Id == lineaEditor.Id);

                if (lineaFactura != null)
                {
                    lineaFactura.InjectFrom(lineaEditor);
                }
                else
                {
                    lineaFactura = new LineaFactura();
                    lineaFactura.InjectFrom(lineaEditor);
                    factura.Lineas.Add(lineaFactura);
                }
            }
        }

        public static void InyectaFactura(this EditorFactura editor, Factura factura)
        {
            editor.InjectFrom(factura);

            editor.BorrarLineasFactura();           

            foreach (var lineaFactura in factura.Lineas)
            {
                var lineaEditor = new LineaEditorFactura();
                lineaEditor.InjectFrom(lineaFactura);
                editor.Lineas.Add(lineaEditor);
            }
        }
    }
}
