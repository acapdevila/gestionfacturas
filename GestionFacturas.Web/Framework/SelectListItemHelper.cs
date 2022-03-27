using GestionFacturas.Dominio.Infra;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GestionFacturas.Web.Framework
{
    public static class SelectListItemHelper
    {
        public static List<SelectListItem> ToSelectListItems(this IEnumerable<ItemDto> elementos)
        {
            return elementos
                    .Select(ItemDtoToSelectItem())
                    .ToList();
        }

        public static async Task<List<SelectListItem>> ToSelectListItemsAsync(this IQueryable<ItemDto> elementos)
        {
            return (await elementos.ToListAsync())
                    .Select(ItemDtoToSelectItem())
                    .ToList();
        }


        private static Func<ItemDto, SelectListItem> ItemDtoToSelectItem()
        {
            return p => new SelectListItem(p.Descripcion, Convert.ToString(p.Id));
        }
    }
}
