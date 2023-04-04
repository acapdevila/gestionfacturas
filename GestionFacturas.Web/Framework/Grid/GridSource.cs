using System.Collections;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;

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


    public static class GridSourceExtensiones
    {

        public static async Task<GridSource> ToGrid<T>(
            this IQueryable<T> consulta, 
                GridParams gridParams)
        {
            var source = new GridSource
            {
                total = await consulta.CountAsync(),

                rows = await consulta
                    .OrdenarPor(gridParams.Sort, gridParams.Order)
                    .Skip(gridParams.Offset)
                    .Take(gridParams.Limit)
                    .ToListAsync()
            };

            return source;
        }
        
        private static IOrderedQueryable<T> OrdenarPor<T>(
            this IQueryable<T> items, string campoOrden, string ascDesc)
        {
            if (string.IsNullOrEmpty(campoOrden)) return (IOrderedQueryable<T>)items;

            if (items.Expression.Type == typeof(IOrderedQueryable<T>))
                return ((IOrderedQueryable<T>)items).ThenBy($"{campoOrden} {ascDesc}");
            else
                return ((IOrderedQueryable<T>)items).OrderBy($"{campoOrden} {ascDesc}");
        }
    }

}