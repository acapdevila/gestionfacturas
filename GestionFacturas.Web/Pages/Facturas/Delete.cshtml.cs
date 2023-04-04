using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GestionFacturas.AccesoDatosSql;
using GestionFacturas.Dominio;

namespace GestionFacturas.Web.Pages.Facturas
{
    public class DeleteModel : PageModel
    {
        private readonly GestionFacturas.AccesoDatosSql.SqlDb _context;

        public DeleteModel(GestionFacturas.AccesoDatosSql.SqlDb context)
        {
            _context = context;
        }

        [BindProperty]
      public Factura Factura { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Facturas == null)
            {
                return NotFound();
            }

            var factura = await _context.Facturas.FirstOrDefaultAsync(m => m.Id == id);

            if (factura == null)
            {
                return NotFound();
            }
            else 
            {
                Factura = factura;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Facturas == null)
            {
                return NotFound();
            }
            var factura = await _context.Facturas.FindAsync(id);

            if (factura != null)
            {
                Factura = factura;
                _context.Facturas.Remove(Factura);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
