using GestionFacturas.AccesoDatosSql;
using GestionFacturas.Dominio.Clientes;
using GestionFacturas.Web.Framework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Omu.ValueInjecter;

namespace GestionFacturas.Web.Pages.Clientes;

[BindProperties]
public class CrearClienteModel : PageModel
{
    
    public static string NombrePagina = "/" + nameof(Clientes) +
                                        "/" + nameof(CrearClienteModel)
                                                .QuitarStringModel();

    private readonly SqlDb _db;
    
    public CrearClienteModel(SqlDb db)
    {
        _db = db;
    }

    public EditorClienteVm Editor { get; set; } = new();
    
    public void OnGet()
    {
       
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        var cliente = new Cliente();

        cliente.InjectFrom(Editor);

        _db.Clientes.Add(cliente);

        await  _db.SaveChangesAsync();

        return RedirectToAction(ListaGestionClientesModel.NombrePagina);
    }
    
}