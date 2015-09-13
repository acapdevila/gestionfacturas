using GestionFacturas.Datos;
using GestionFacturas.Modelos;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using Microsoft.Reporting.WebForms;

namespace GestionFacturas.Servicios
{
    public class ServicioCliente : ServicioCrudCliente
    {

        public ServicioCliente(ContextoBaseDatos contexto) : base(contexto)
        {

        }

        public async Task<IEnumerable<LineaListaGestionClientes>> ListaGestionClientesAsync(FiltroBusquedaCliente filtroBusqueda)
        {
            var consulta = _contexto.Clientes.AsQueryable();

            if (filtroBusqueda.TieneValores)
            {
                if (!string.IsNullOrEmpty(filtroBusqueda.NombreOEmpresa))
                {
                    consulta = consulta.Where(m => m.NombreOEmpresa.Contains(filtroBusqueda.NombreOEmpresa));
                }
                if (!string.IsNullOrEmpty(filtroBusqueda.IdentificacionFiscal))
                {
                    consulta = consulta.Where(m => m.NumeroIdentificacionFiscal.Contains(filtroBusqueda.IdentificacionFiscal));
                }

                if (filtroBusqueda.Id.HasValue)
                {
                    consulta = consulta.Where(m => m.Id== filtroBusqueda.Id.Value);
                }

            }

            var consultaClientes = consulta
                .Select(m => new LineaListaGestionClientes
                {
                    Id = m.Id,
                    NombreOEmpresa = m.NombreOEmpresa,
                    NumeroIdentificacionFiscal = m.NumeroIdentificacionFiscal,
                    Email = m.Email,
                    NumFacturas = m.Facturas.Count                   
                });

            var facturas = await consultaClientes.ToListAsync();

            return facturas;
        }
        
        public async Task<IEnumerable<Cliente>> ListaClientesAsync(FiltroBusquedaCliente filtroBusqueda, int indicePagina, int registrosPorPagina)
        {
            var numPagina = indicePagina - 1;
            if (numPagina < 0) numPagina = 0;

            var consulta = _contexto.Facturas.Where(m=>m.IdComprador != null);

            if (filtroBusqueda.TieneValores)
            {
                if (!string.IsNullOrEmpty(filtroBusqueda.NombreOEmpresa))
                {
                    consulta = consulta.Where(m => m.CompradorNombreOEmpresa.Contains(filtroBusqueda.NombreOEmpresa));
                }

                if (!string.IsNullOrEmpty(filtroBusqueda.IdentificacionFiscal))
                {
                    consulta = consulta.Where(m => m.CompradorNumeroIdentificacionFiscal.Contains(filtroBusqueda.IdentificacionFiscal));
                }

                if (filtroBusqueda.Id.HasValue)
                {
                    consulta = consulta.Where(m => m.IdComprador == filtroBusqueda.Id.Value);
                }
            }

            var consultaClientes = consulta
                .Select(m => new Cliente
                {
                    Id = m.IdComprador.Value,
                    CodigoPostal = m.CompradorCodigoPostal,
                    Direccion = m.CompradorDireccion,
                    Email = m.CompradorEmail,
                    Localidad = m.CompradorLocalidad,
                    NombreOEmpresa = m.CompradorNombreOEmpresa,
                    NumeroIdentificacionFiscal = m.CompradorNumeroIdentificacionFiscal,
                    Provincia = m.CompradorProvincia
                }).Distinct();

            var clientes = await consultaClientes.OrderBy(m=>m.NombreOEmpresa).Skip(registrosPorPagina * numPagina).Take(registrosPorPagina).ToListAsync();

            return clientes;
        }


        public async Task<EditorCliente> BuscaEditorClienteAsync(int? idCliente)
        {
            var cliente = await BuscarClienteAsync(idCliente);
            var editor = new EditorCliente();
            editor.InyectarCliente(cliente);
            return editor;
        }
    }
}
