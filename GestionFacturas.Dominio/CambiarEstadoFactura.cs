using CSharpFunctionalExtensions;

namespace GestionFacturas.Dominio
{
    public record CambiarEstadoFacturaComando(int Id, EstadoFacturaEnum Estado);

    public static class CambiarEstadoFactura
    {
        public record Factura(int Id, EstadoFacturaEnum Estado);

        public static Result<Factura> CambiarEstado(this Factura factura, EstadoFacturaEnum estado)
        {
            return ValidarEsPosibleCambiarEstadoFactura(factura)
                        .Map(() =>
                            factura with { Estado = estado });
        }

        private static Result ValidarEsPosibleCambiarEstadoFactura(Factura factura)
        {
            //if(factura.Estado == EstadoFacturaEnum.Enviada)
            //    return Result.Failure("Factura ya enviada");

            //if(factura.Estado == EstadoFacturaEnum.Cobrada)
            //    return Result.Failure("No es puede enviar una factura cobrada");

            return Result.Success();
        }
    }
}
