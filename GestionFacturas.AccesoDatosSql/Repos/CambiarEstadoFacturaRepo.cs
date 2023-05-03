using CSharpFunctionalExtensions;
using GestionFacturas.Dominio;
using Microsoft.EntityFrameworkCore;

namespace GestionFacturas.AccesoDatosSql.Repos
{
    public class CambiarEstadoFacturaRepo //: IRepo<CambiarEstadoFactura.Factura>
    {
        private readonly SqlDb _db;

        public CambiarEstadoFacturaRepo(SqlDb db)
        {
            _db = db;
        }

        public async Task<Maybe<CambiarEstadoFactura.Factura>> GetById(int id)
        {
            var select = await _db.Facturas
                .Select(m => new
                {
                    m.Id,
                    m.EstadoFactura
                }).FirstOrDefaultAsync(m => m.Id == id);


            if (select is null) return Maybe.None;


            return new CambiarEstadoFactura.Factura(select.Id, select.EstadoFactura);

        }

        public async Task Update(CambiarEstadoFactura.Factura entity)
        {
            await _db.Database.ExecuteSqlInterpolatedAsync(
                    $"UPDATE GestionFacturas.Facturas SET EstadoFactura={entity.Estado} WHERE Id={entity.Id}");
            
      
        }
    }
}
