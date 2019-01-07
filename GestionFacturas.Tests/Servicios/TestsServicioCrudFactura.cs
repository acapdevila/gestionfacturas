using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GestionFacturas.Modelos;
using System.Collections.Generic;
using GestionFacturas.Datos;
using System.Linq;
using GestionFacturas.Servicios;
using Omu.ValueInjecter;
using System.Threading.Tasks;

namespace GestionFacturas.Tests.Servicios
{
    [TestClass]
    public class TestsServicioCrudFactura
    {
        private readonly ContextoBaseDatos _contexto = new ContextoBaseDatos();

        [TestMethod]
        public async Task Crear_Obterer_Actualizar_Borrar_Factura()
        {
            var servicioFactura = new ServicioCrudFactura(_contexto);
            var editorFactura = DameEjemploEditorFacturaConTresLineas();

            // Crear
            await servicioFactura.CrearFacturaAsync(editorFactura);
            var facturaCreada = await servicioFactura.BuscarFacturaAsync(editorFactura.Id);

            Assert.IsNotNull(facturaCreada, "La factura no se ha creado en la base de datos");
            Assert.IsTrue(facturaCreada.Lineas.Any(), "No se han creado lineas de factura");
            Assert.IsTrue(facturaCreada.Lineas.Count == editorFactura.Lineas.Count, "Las lineas de factura creadas no coinciden con las lineas del editor de factura");


            // Actualizar
            editorFactura.Comentarios = editorFactura.Comentarios + " Ok";
            var cambios =  await servicioFactura.ActualizarFacturaAsync(editorFactura);
            var facturaActualizada = await servicioFactura.BuscarFacturaAsync(editorFactura.Id);

            Assert.IsTrue(cambios > 0, "La factura no se ha podido actualizar en la base de datos");
            Assert.IsNotNull(facturaActualizada, "La factura no se ha actualizado en la base de datos");
            Assert.IsTrue(facturaActualizada.Lineas.Count == editorFactura.Lineas.Count, "Las lineas de la factura actualizada no coinciden con las lineas del editor de factura");

            // Borrar
            await servicioFactura.EliminarFactura(editorFactura.Id);

            var facturaEliminada = await servicioFactura.BuscarFacturaAsync(editorFactura.Id);

            Assert.IsNull(facturaEliminada, "La factura no se ha podido eliminar de la base de datos");
        }



        private EditorFactura DameEjemploEditorFacturaConTresLineas()
        {
            var usuario = _contexto.Users.FirstOrDefault();
            var porcentajeIva = 21;

            return new EditorFactura
            {
                IdUsuario = usuario.Id,
                FormatoNumeroFactura = "{0}{1:1000}",
                SerieFactura = "AC / ",
                NumeracionFactura = 12,
                FechaEmisionFactura = DateTime.Now,
                FechaVencimientoFactura = DateTime.Now,
                Comentarios = "Test",
                Lineas = new List<EditorLineaFactura>
                {
                    new EditorLineaFactura { PorcentajeImpuesto = porcentajeIva, PrecioUnitario = 20, Cantidad = 2 },
                    new EditorLineaFactura { PorcentajeImpuesto = porcentajeIva, PrecioUnitario = (decimal)23.5, Cantidad = 1 },
                    new EditorLineaFactura { PorcentajeImpuesto = porcentajeIva, PrecioUnitario = 4, Cantidad = 2 }
                }
            };            
        }

    }
}
