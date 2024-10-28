using CSharpFunctionalExtensions;

namespace GestionFacturas.Web.Framework
{
   public  static class ExtensionesInputsNumericos
    {
        public static string ToInputDecimal(this decimal valor)
        {
            return valor.ToString("G");
        }

        public static decimal FromInputConComaOPuntoToDecimal(this string inputDecimal, decimal defaultValue = 0)
        {
            if (string.IsNullOrEmpty(inputDecimal))
                return defaultValue;

            return inputDecimal.Replace(".", ",").FromInputToDecimal(defaultValue);

        }


        public static decimal FromInputToDecimal(this string inputDecimal, decimal defaultValue = 0)
        {
            if (string.IsNullOrEmpty(inputDecimal)) 
                    return defaultValue;
            return decimal.Parse(inputDecimal);
        }

        public static Maybe<decimal> FromNullableInputToDecimal(this string? inputDecimal)
        {
            if (string.IsNullOrEmpty(inputDecimal))
                return Maybe<decimal>.None;

            return decimal.Parse(inputDecimal);
        }

       
        public static decimal? FromNullableInputToNullableDecimal(this string? inputDecimal)
        {
            if (string.IsNullOrEmpty(inputDecimal))
                return null;

            _ = decimal.TryParse(inputDecimal, out var conversion);
            return conversion;
        }

    }
}
