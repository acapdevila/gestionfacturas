using ClosedXML.Excel;
using ClosedXML.Extensions;
using GestionFacturas.AccesoDatosSql;
using GestionFacturas.AccesoDatosSql.Filtros;
using GestionFacturas.Dominio.Clientes;
using GestionFacturas.Web.Framework.Grid;
using MailKit.Search;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionFacturas.Web.Pages.Clientes
{
    public class GridClientesController : Controller
    {
        private readonly SqlDb _db;

        public GridClientesController(SqlDb context)
        {
            _db = context;
        }

        public async Task<ActionResult<GridSource>> Buscar(
            [FromQuery] GridParamsCarteraClientes gridParams)
        {
            var consulta = _db.Clientes
                .AsNoTracking()
                .FiltrarPorParametros(gridParams)
                .Select(m => new LineaListaGestionClientesVm
                {
                    Id = m.Id,
                    NombreOEmpresa = m.NombreOEmpresa,
                    Nif = m.NumeroIdentificacionFiscal,
                    Email = m.Email,
                    NumFacturas = m.Facturas.Count
                })
                .OrderByDescending(m=>m.Id);


            var source = await consulta.ToGrid(gridParams);

            return source;
        }


        public async Task<ActionResult<GridSource>> ExportarExcel(
            [FromQuery] GridParamsCarteraClientes gridParams)
        {
            var clientes = await _db.Clientes
                .AsNoTracking()
                .FiltrarPorParametros(gridParams)
                .Select(m => new
                {
                    Id = m.Id,
                    NombreOEmpresa = m.NombreOEmpresa,
                    Nif = m.NumeroIdentificacionFiscal,
                    Email = m.Email,
                    NumFacturas = m.Facturas.Count
                }).ToListAsync(); 

            using var wb = new XLWorkbook();
            var worksheet = wb.Worksheets.Add("Clientes");
            worksheet.Cell(1, 1).InsertTable(clientes);
            worksheet.Columns().AdjustToContents();
            return wb.Deliver(@"Bahia_Clientes_" + DateTime.Now.ToString("yyyy_MM_dd") + ".xlsx");
        }
    }

    public class GridParamsCarteraClientes : GridParams
    {
        public string Nif { get; set; } = string.Empty;

        public string NombreOEmpresa { get; set; } = string.Empty;
        
        public int Ref { get; set; }

    }

    public static class GridParamsCarteraClientesExtensiones
    {
        public static IQueryable<Cliente> FiltrarPorParametros(
            this IQueryable<Cliente> consulta,
            GridParamsCarteraClientes gridParams)
        {
            return consulta
                .If(!string.IsNullOrEmpty(gridParams.NombreOEmpresa))
                .ThenWhere(m => m.NombreOEmpresa.Contains(gridParams.NombreOEmpresa))

                .If(!string.IsNullOrEmpty(gridParams.Nif))
                .ThenWhere(m => m.NumeroIdentificacionFiscal.Contains(gridParams.Nif))


                .If(0 < gridParams.Ref)
                .ThenWhere(m => m.Id == gridParams.Ref);

        }

    }
}
