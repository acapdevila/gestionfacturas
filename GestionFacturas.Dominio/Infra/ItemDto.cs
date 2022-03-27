namespace GestionFacturas.Dominio.Infra
{
    public class ItemDto
    {
        public ItemDto()
        {

        }

        public ItemDto(long id, string descripcion)
        {
            Id = id;
            Descripcion = descripcion;
        }

        public long Id { get; set; }
        public string Descripcion { get; set; } = string.Empty;
    }


    public static class ExtensionItemDto
    {

        public static IQueryable<ItemDto> Items(this IQueryable<Cliente> consulta)
        {
            return consulta.Select(m => new ItemDto
            {
                Id = m.Id,
                Descripcion = m.NombreComercial
            });
        }

        

    }
}
