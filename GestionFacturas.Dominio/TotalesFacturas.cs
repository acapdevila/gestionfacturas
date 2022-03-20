namespace GestionFacturas.Dominio;

public class TotalesFacturas
{        
    public decimal TotalBaseImponible { get; set; }
    public decimal TotalImpuestos { get; set; }
    public decimal TotalImporte {
        get { return TotalBaseImponible + TotalImpuestos;  }
    }
}