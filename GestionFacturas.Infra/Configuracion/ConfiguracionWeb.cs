using System.Configuration;

namespace GestionFacturas.Infra.Configuracion
{
    public class ConfiguracionWebConfig : IConfiguracion
    {
        public string NombreComercialEmpresa => ConfigurationManager.AppSettings["NombreComercialEmpresa"];
    }
}