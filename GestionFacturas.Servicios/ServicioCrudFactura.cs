using GestionFacturas.Datos;
using GestionFacturas.Modelos;
using System.Linq;
using System.Data.Entity;

namespace GestionFacturas.Servicios
{
    public class ServicioCrudFactura
    {
        private readonly ContextoBaseDatos _contexto;

        public ServicioCrudFactura(ContextoBaseDatos contexto)
        {
            _contexto = contexto;
        }
        public void Crear(Factura factura)
        {
            _contexto.Facturas.Add(factura);
            _contexto.SaveChanges();
        }

        public Factura Obtener(int idFactura) {
            return _contexto.Facturas.Include(m=>m.Lineas).FirstOrDefault(m=>m.Id == idFactura);
        }

        public void Eliminar(int idFactura)
        {
            var factura = Obtener(idFactura);

            while (factura.Lineas.Any())
            {
                var linea = factura.Lineas.First();
                _contexto.LineasFacturas.Remove(linea);
            }

            _contexto.Facturas.Remove(factura);

            _contexto.SaveChanges();
        }

        public int GuardarCambios()
        {
            return _contexto.SaveChanges();
        }
    }
}
