using GestionFacturas.Dominio.Infra;

namespace GestionFacturas.Web.Framework.Grid
{
    public class GridParams
    {
        public string Sort { get; set; } = string.Empty;
        public string Order { get; set; } = string.Empty;
        public string BuscarPor { get; set; } = string.Empty;
        public int  Limit { get; set; } 
        public int Offset { get; set; }
        public string SearchColumns { get; set; } = string.Empty;

        public List<string> ListSearchColumns => string.IsNullOrEmpty(SearchColumns)
            ? new List<string>()
            : SearchColumns.Split(',').ToList();

        public Dictionary<string, string> SearchFields { get; set; } = new Dictionary<string, string>();

        public List<Campo> ListSearchFields => SearchFields == null
            ? new List<Campo>()
            : SearchFields.Select(m => new Campo { Nombre = m.Key, BuscarPor = m.Value }).ToList();


        public bool EsBuscar() => !string.IsNullOrEmpty(BuscarPor);
                                
        // ------------------------------------------------------------------------
        #region Metodes per obtenir els valors de filtre continguts en el camp SearchFields
        // ------------------------------------------------------------------------
        public string ObtenerValorFiltroTexto(string campo)
        {
            var endkey = "." + campo;
            var valor = SearchFields.FirstOrDefault(m => m.Key.EndsWith(endkey)).Value;

            if (!string.IsNullOrWhiteSpace(valor))
                return valor.Trim();

            return string.Empty;
        }
        public long? ObtenerValorFiltroNumerico(string campo)
        {
            var endkey = "." + campo;
            var valor = SearchFields.FirstOrDefault(m => m.Key.EndsWith(endkey)).Value;
            if (!string.IsNullOrWhiteSpace(valor))
            {
                if (long.TryParse(valor.Trim(), out long idFiltro))
                    return idFiltro;
            }
            return null;
        }
        public long[] ObtenerValorFiltroArrayNumerico(string campo)
        {
            var endkey = "." + campo;
            var valores = SearchFields.FirstOrDefault(m => m.Key.EndsWith(endkey)).Value;
            if (!string.IsNullOrWhiteSpace(valores))
            {
                List<long> idsFiltro = new ();
                var split = valores.Split('|');
                foreach (var valor in split)
                {
                    if (long.TryParse(valor.Trim(), out long idFiltro))
                        idsFiltro.Add(idFiltro);
                }
                if (idsFiltro.Count > 0)
                    return idsFiltro.ToArray();
                else
                    return new long[0];
            }
            return new long[0];
        }
        public DateTime? ObtenerValorFiltroDateTime(string campo)
        {
            var endkey = "." + campo;
            var valor = SearchFields.FirstOrDefault(m => m.Key.EndsWith(endkey)).Value;

            if (!string.IsNullOrWhiteSpace(valor))
            {
                if (DateTime.TryParse(valor.Trim(), out DateTime dtFiltro))
                    return dtFiltro;
            }
            return null;
        }
        public bool ObtenerValorFiltroBool(string campo)
        {
            var endkey = "." + campo;
            var valor = SearchFields.FirstOrDefault(m => m.Key.EndsWith(endkey)).Value;

            if (string.IsNullOrWhiteSpace(valor) || valor.Trim() == "false")
                return false;

            return true;
        }
        public bool? ObtenerValorFiltroBoolNullable(string campo)
        {
            var endkey = "." + campo;
            var valor = SearchFields.FirstOrDefault(m => m.Key.EndsWith(endkey)).Value;

            if (string.IsNullOrWhiteSpace(valor) ||
                (valor.Trim() != "false" && valor.Trim() != "true"))
                return null;
            if (valor.Trim() == "false")
                return false;
            if (valor.Trim() == "true")
                return true;

            return null;
        }

        #endregion
        // ------------------------------------------------------------------------
    }
}