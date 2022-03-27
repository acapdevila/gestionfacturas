using System.Collections;

namespace GestionFacturas.Web.Framework.Grid
{
#pragma warning disable IDE1006 // Estilos de nombres
    public class GridSource
    {
        public GridSource()
        {
            rows = new List<int>();
        }


        public int total { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Las propiedades de colección deben ser de solo lectura", Justification = "Si es necesaria en cliente javascript")]
        public IList rows { get;  set; }
    }
#pragma warning restore IDE1006 // Estilos de nombres
}