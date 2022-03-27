using GestionFacturas.AccesoDatosSql;
using GestionFacturas.Aplicacion;
using GestionFacturas.Dominio;
using Microsoft.AspNetCore.Mvc.RazorPages;
using X.PagedList;

namespace GestionFacturas.Web.Pages.Clientes
{
    public class ListaGestionClientesModel : PageModel
    {
        public const string NombrePagina = $"/{nameof(Clientes)}/ListaGestionClientes";

        private readonly SqlDb _db;

        public ListaGestionClientesModel(SqlDb db)
        {
            _db = db;
        }

        public FiltroBusquedaCliente FiltroBusqueda { get; set; } = new();

        public IPagedList<LineaListaGestionClientes> ListaClientes { get; set; } =
            new List<LineaListaGestionClientes>().ToPagedList(1, 1);

        public async Task OnGetAsync(FiltroBusquedaCliente filtroBusqueda, int? pagina)
        {
            var consulta = _db.Clientes.AsQueryable();

            if (filtroBusqueda.TieneFiltrosBusqueda)
            {
                consulta = consulta.Where_FiltroBusqueda(filtroBusqueda);
            }

            var consultaOrdenada = consulta.OrderBy_OrdenarPor(filtroBusqueda.OrdenarPorEnum);

            var consultaClientes = consultaOrdenada
                .Select(m => new LineaListaGestionClientes
                {
                    Id = m.Id,
                    NombreOEmpresa = m.NombreOEmpresa,
                    NumeroIdentificacionFiscal = m.NumeroIdentificacionFiscal,
                    Email = m.Email,
                    Direccion = m.Direccion,
                    NombreComercial = m.NombreComercial,
                    NumFacturas = m.Facturas.Count
                });


            ListaClientes = await consultaClientes.ToPagedListAsync(filtroBusqueda.IndicePagina, filtroBusqueda.LineasPorPagina);
            FiltroBusqueda = filtroBusqueda;
        }
    }
}
