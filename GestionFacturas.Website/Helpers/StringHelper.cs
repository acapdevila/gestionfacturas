using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionFacturas.Website.Helpers
{
    public static class StringHelper
    {
        public static string TruncarConElipsis(this string value, int maxLength)
        {
            return value.Length <= maxLength ? value : value.Substring(0, maxLength) + " ..";
         
        }
    }
}
