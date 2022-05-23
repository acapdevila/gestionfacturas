using System.Linq.Expressions;
using GestionFacturas.Dominio.Infra;

namespace GestionFacturas.Dominio.Clientes.Especificaciones;

public class BusquedaClientesEspecificacion : Specification<Cliente>
{
    private readonly string _search;

    public BusquedaClientesEspecificacion(string search)
    {
        _search = search;
    }

    public override Expression<Func<Cliente, bool>> ToExpression()
    {
        return cliente => 
            _search.Contains(cliente.NombreComercial) || 
            cliente.NombreComercial.Contains(_search) ||
            
            _search.Contains(cliente.NombreOEmpresa) || 
            cliente.NombreOEmpresa.Contains(_search);

    }
}