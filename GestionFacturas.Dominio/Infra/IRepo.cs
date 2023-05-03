using CSharpFunctionalExtensions;

namespace GestionFacturas.Dominio.Infra;

public interface IRepo<T>
{
    Task<Maybe<T>> GetById(int id);
    Task Update(T entity);

}