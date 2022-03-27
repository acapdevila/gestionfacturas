using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using GestionFacturas.Dominio.Infra;

namespace GestionFacturas.AccesoDatosSql.Filtros
{
    public static class GridOrdenarQueryExtensions
    {
        public static IOrderedQueryable<T> OrdenarPor<T>(
            this IQueryable<T> consulta, List<Orden> ordenCampos)
        {
            foreach (var orden in ordenCampos)
                consulta = consulta.OrdenarPor(orden);

            return consulta as IOrderedQueryable<T> ?? throw new NullReferenceException("consulta no puede ser nulo");
        }

        public static IOrderedQueryable<T> OrdenarPor<T>(this IQueryable<T> items, Orden criteriosOrdenacion)
        {
            if (items.Expression.Type == typeof(IOrderedQueryable<T>))
                return ((IOrderedQueryable<T>)items).ThenBy($"{criteriosOrdenacion.Campo} {criteriosOrdenacion.Direccion}");
            else
                return ((IOrderedQueryable<T>)items).OrderBy($"{criteriosOrdenacion.Campo} {criteriosOrdenacion.Direccion}");
        }

        public static IOrderedQueryable<T> OrdenarPor<T>(this IQueryable<T> items, string campoOrden, string ascDesc)
        {
            if (string.IsNullOrEmpty(campoOrden)) return (IOrderedQueryable<T>)items;

            if (items.Expression.Type == typeof(IOrderedQueryable<T>))
                return ((IOrderedQueryable<T>)items).ThenBy($"{campoOrden} {ascDesc}");
            else
                return ((IOrderedQueryable<T>)items).OrderBy($"{campoOrden} {ascDesc}");
        }

        //public static IQueryable<T> If<T>(this IQueryable<T> consulta, 
        //        bool condidicion, 
        //        Expression<Func<T, bool>> predicate)
        //{
        //    if(condidicion)
        //    {
        //        consulta = consulta.Where(predicate);
        //    }


        //    return consulta;
        //}

        public static IfThenDto<T> If<T>(
             this IQueryable<T> consulta,
             bool condicion)
        {
             return new IfThenDto<T>(condicion, consulta);
        }

        public static IQueryable<T> ThenWhere<T>(
             this IfThenDto<T> dto, Expression<Func<T, bool>> predicate)
        {
            if(dto.Condicion)
            {
                return dto.Consulta.Where(predicate);
            }
            return dto.Consulta;
        }

        public static IQueryable<T> ThenTake<T>(
            this IfThenDto<T> dto, int count)
        {
            if (dto.Condicion)
            {
                return dto.Consulta.Take(count);
            }
            return dto.Consulta;
        }

        public static IQueryable<T> ThenOrderBy<T, TKey>(
             this IfThenDto<T> dto, Expression<Func<T, TKey>> predicate)
        {
            if (dto.Condicion)
            {
                return dto.Consulta.OrderBy(predicate);
            }
            return dto.Consulta;
        }

        public static IQueryable<T> ThenOrderByDescending<T, TKey>(
            this IfThenDto<T> dto, Expression<Func<T, TKey>> predicate)
        {
            if (dto.Condicion)
            {
                return dto.Consulta.OrderByDescending(predicate);
            }
            return dto.Consulta;
        }

    } 

    public class IfThenDto<T>
    {
        public IfThenDto(bool condicion, IQueryable<T> consulta)
        {
            Condicion = condicion;
            Consulta = consulta;
        }
        public bool Condicion { get; }
        public IQueryable<T> Consulta { get; }
    }
}
