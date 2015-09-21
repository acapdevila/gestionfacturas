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
using System.IO;
using ClosedXML.Excel;

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
                    NombreComercial = m.NombreComercial,
                    NumFacturas = m.Facturas.Count                   
                });

            var facturas = await consultaClientes.ToListAsync();

            return facturas;
        }

        public async Task<IEnumerable<Cliente>> ListaClientesAsync(FiltroBusquedaCliente filtroBusqueda, int indicePagina, int registrosPorPagina)
        {
            var numPagina = indicePagina - 1;
            if (numPagina < 0) numPagina = 0;

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
                    consulta = consulta.Where(m => m.Id == filtroBusqueda.Id.Value);
                }
            }

            var clientes = await consulta.OrderBy(m => m.NombreOEmpresa).Skip(registrosPorPagina * numPagina).Take(registrosPorPagina).ToListAsync();

            return clientes;
        }


        public async Task<IEnumerable<Cliente>> ListaDiferentesClientesEnFacturasAsync(FiltroBusquedaCliente filtroBusqueda, int indicePagina, int registrosPorPagina)
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

        public async Task ImportarClientesDeExcel(Stream stream, SelectorColumnasExcelCliente columnas)
        {
            var clientesExistentes = await _contexto.Clientes.ToListAsync();

            var clientesExcel = ObtenerClientesDeExcel(stream, columnas);

            var clientesAImportar = QuitarClientesDuplicados(clientesExistentes, clientesExcel);

            await CrearClientesAsync(clientesAImportar);
        }

        private List<EditorCliente> ObtenerClientesDeExcel(Stream stream, SelectorColumnasExcelCliente columnas)
        {
            var workbook = new XLWorkbook(stream);
            var worksheet = workbook.Worksheets.First();

            var firstRow = worksheet.FirstRowUsed();
            var rowUsed = firstRow.RowUsed();
            rowUsed = rowUsed.RowBelow();

            var clientes = new List<EditorCliente>();                

            while (!rowUsed.Cell(columnas.LetraColumnaNombreOEmpresa).IsEmpty())
            {
                var cliente = new EditorCliente {
                    NombreOEmpresa = rowUsed.Cell(columnas.LetraColumnaNombreOEmpresa).GetString(),
                    NumeroIdentificacionFiscal = rowUsed.Cell(columnas.LetraColumnaNumeroIdentificacionFiscal).GetString(),
                    NombreComercial = string.IsNullOrEmpty(columnas.LetraColumnaNombreComercial)  ? null : rowUsed.Cell(columnas.LetraColumnaNombreComercial).GetString(),
                    PersonaContacto = string.IsNullOrEmpty(columnas.LetraColumnaPersonaContacto) ? null : rowUsed.Cell(columnas.LetraColumnaPersonaContacto).GetString(),
                    Email = string.IsNullOrEmpty(columnas.LetraColumnaEmail) ? null : rowUsed.Cell(columnas.LetraColumnaEmail).GetString(),
                    Direccion = string.IsNullOrEmpty(columnas.LetraColumnaDireccion) ? null : rowUsed.Cell(columnas.LetraColumnaDireccion).GetString(),
                    Localidad = string.IsNullOrEmpty(columnas.LetraColumnaLocalidad) ? null : rowUsed.Cell(columnas.LetraColumnaLocalidad).GetString(),
                    Provincia = string.IsNullOrEmpty(columnas.LetraColumnaProvincia) ? null : rowUsed.Cell(columnas.LetraColumnaProvincia).GetString(),
                    CodigoPostal = string.IsNullOrEmpty(columnas.LetraColumnaCodigoPostal) ? null : rowUsed.Cell(columnas.LetraColumnaCodigoPostal).GetString(),
                    ComentarioInterno = string.IsNullOrEmpty(columnas.LetraColumnaComentarioInterno) ? null : rowUsed.Cell(columnas.LetraColumnaComentarioInterno).GetString()
                };

                if(!clientes.Any(m=> m.NombreOEmpresa == cliente.NombreOEmpresa|| m.NumeroIdentificacionFiscal == cliente.NumeroIdentificacionFiscal))
                    clientes.Add(cliente);

                rowUsed = rowUsed.RowBelow();
            }

            return clientes.Distinct().ToList();
        }

        private List<EditorCliente> QuitarClientesDuplicados(List<Cliente> clientesExistentes, List<EditorCliente> clientesExcel)
        {
            var clientesAImportar = new List<EditorCliente>();

            foreach (var clienteExcel in clientesExcel)
            {
                var clienteExistente = clientesExistentes.FirstOrDefault(m => m.NumeroIdentificacionFiscal == clienteExcel.NumeroIdentificacionFiscal);

                if (clienteExistente == null)
                    clientesAImportar.Add(clienteExcel);
            }

            return clientesAImportar;
        }
    }
}
