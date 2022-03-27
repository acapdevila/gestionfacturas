using GestionFacturas.AccesoDatosSql.Filtros;
using Microsoft.EntityFrameworkCore;

namespace GestionFacturas.Web.Framework.Grid
{
    public static class GridDataLoader
    {
        public static GridSource LoadData<T>(IQueryable<T> consulta, GridParams gridParams) where T : new()
        {
            if(gridParams == null) throw new ArgumentNullException(nameof(gridParams));

                consulta = consulta
                    .BuscarPor(gridParams.BuscarPor, gridParams.ListSearchColumns);

            
            var gridSource = new GridSource
            {
                total = consulta.Count(),

                rows = consulta
                    .OrdenarPor(gridParams.Sort, gridParams.Order)
                    .Skip(gridParams.Offset)
                    .Take(gridParams.Limit)
                    .ToList()
            };


            return gridSource;
        }

        public static async Task<GridSource> LoadDataAsync<T>(IQueryable<T> consulta, GridParams gridParams)
            where T : new()
        {
            if (gridParams == null) throw new ArgumentNullException(nameof(gridParams));

            consulta = consulta
                     .BuscarPor(gridParams.BuscarPor, gridParams.ListSearchColumns);

            var consultaFilas = consulta
                .OrdenarPor(gridParams.Sort, gridParams.Order)
                    .Skip(gridParams.Offset);

            if (0 < gridParams.Limit)
                consultaFilas = consultaFilas.Take(gridParams.Limit);

            
                var gridSource = new GridSource
                {
                    total = await consulta.CountAsync().ConfigureAwait(false),

                    rows = await consultaFilas
                    .ToListAsync()
                    .ConfigureAwait(false),
                };

                return gridSource;
            

            

        }

    }
}
