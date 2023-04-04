using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GestionFacturas.AccesoDatosSql;
using GestionFacturas.Dominio;

namespace GestionFacturas.Web.Pages.Facturas
{
    public class CrearFacturaModel : PageModel
    {
        public static readonly string NombrePagina = "/Facturas/CrearFactura";

        private readonly GestionFacturas.AccesoDatosSql.SqlDb _context;

        public CrearFacturaModel(GestionFacturas.AccesoDatosSql.SqlDb context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["IdComprador"] = new SelectList(_context.Clientes, "Id", "CodigoPostal");
            return Page();
        }

        [BindProperty]
        public Factura Factura { get; set; } = default!;

        


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Facturas == null || Factura == null)
            {
                return Page();
            }

            _context.Facturas.Add(Factura);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
