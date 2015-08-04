using GestionFacturas.Modelos;
using Microsoft.AspNet.Identity.EntityFramework;

namespace GestionFacturas.Datos
{
 
    public class ContextoBaseDatos : IdentityDbContext<ApplicationUser>
    {
        public ContextoBaseDatos()
            : base("CadenaConexionGestionFacturas", throwIfV1Schema: false)
        {
            Configuration.LazyLoadingEnabled = false;
        }

        public static ContextoBaseDatos Create()
        {
            return new ContextoBaseDatos();
        }
    }
}
