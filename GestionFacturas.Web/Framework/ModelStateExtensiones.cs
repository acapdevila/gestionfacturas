using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GestionFacturas.Web.Framework
{
    public static class ModelStateExtensiones
    {
        public static bool TieneErrores(this ModelStateDictionary modelStateDictionary)
        {
            return modelStateDictionary.Any(x => x.Value != null && x.Value.Errors.Any());
        }
    }
}
