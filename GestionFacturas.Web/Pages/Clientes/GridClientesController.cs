using GestionFacturas.AccesoDatosSql;
using GestionFacturas.AccesoDatosSql.Filtros;
using GestionFacturas.Dominio.Clientes.Especificaciones;
using GestionFacturas.Web.Framework.Grid;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionFacturas.Web.Pages.Clientes;

public class GridClientesController : Controller
{
    private readonly SqlDb _db;

    public GridClientesController(SqlDb db)
    {
        _db = db;
    }

    public async Task<ActionResult<GridSource>> Get([FromQuery] GridParams gridParams)
    {
        var busquedaClientes = new BusquedaClientesEspecificacion(gridParams.Search);
        
        var consulta = _db.Clientes.AsNoTracking()
            .If(!string.IsNullOrEmpty(gridParams.Search) )
                    .ThenWhere(busquedaClientes.ToExpression())
            .Select(m => new LineaListaGestionClientesVm
            {
                Id = m.Id,
                NombreOEmpresa = m.NombreOEmpresa,
                NumeroIdentificacionFiscal = m.NumeroIdentificacionFiscal,
                Email = m.Email,
                Direccion = m.Direccion,
                NombreComercial = m.NombreComercial,
                NumFacturas = m.Facturas.Count
            });

        var source = await consulta.ToGridSourceAsync(gridParams);
        return source;
    }
}