namespace GestionFacturas.Infra.Configuracion
{
    public class FactoriaConfiguracion
    {
        private static IConfiguracion _configuracon;

        public static void Inicializar(
                                      IConfiguracion configuracion)
        {
            _configuracon = configuracion;
        }

        public static IConfiguracion ObtenerConfiguracion()
        {
            return _configuracon;
        }
    }
}
