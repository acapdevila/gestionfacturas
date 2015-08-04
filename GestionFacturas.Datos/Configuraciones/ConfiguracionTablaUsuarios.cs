using GestionFacturas.Modelos;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.ModelConfiguration;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GestionFacturas.Datos.Configuraciones
{
    public class ConfiguracionTablaUsuarios : EntityTypeConfiguration<Usuario>
    {
        public ConfiguracionTablaUsuarios()
        {

        }
    }
}