using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GestionFacturas.Website;
using Microsoft.AspNet.Identity.EntityFramework;
using GestionFacturas.Modelos;
using GestionFacturas.Datos;
using GestionFacturas.Website.Models;

namespace GestionFacturas.Tests.Servicios
{
    [TestClass]
    public class TestsApplicationUserManager
    {        
        [TestMethod]
        public void Registrar_Obterer_CambiarContraseña_Borrar_Uasuario()
        {
            var servicioUsuario = new ApplicationUserManager(new UserStore<Usuario>(new ContextoBaseDatos()));

            // Crear
            var modeloRegistro = new RegisterViewModel {
                Email = "test@usuario.com",
                Password ="password!",
                ConfirmPassword = "password!",
            };

            var usuario = new Usuario {
                UserName = modeloRegistro.Email,
                Email = modeloRegistro.Email               
            };
                    
            // Registrar
            var resultadoAccion = servicioUsuario.CreateAsync(usuario, modeloRegistro.Password).Result;

            Assert.IsTrue(resultadoAccion.Succeeded);

            // Obtener
            var usuarioCreado = servicioUsuario.FindByNameAsync(modeloRegistro.Email).Result;
            Assert.AreEqual(modeloRegistro.Email, usuarioCreado.Email);

            // Cambiar contraseña
            resultadoAccion = servicioUsuario.ChangePasswordAsync(usuarioCreado.Id, modeloRegistro.Password, "nuevoPassword!").Result;
            Assert.IsTrue(resultadoAccion.Succeeded);

            // Borrar
            resultadoAccion = servicioUsuario.DeleteAsync(usuarioCreado).Result;

            Assert.IsTrue(resultadoAccion.Succeeded);

            var usuarioEliminado = servicioUsuario.FindByNameAsync(modeloRegistro.Email).Result;
            Assert.IsNull(usuarioEliminado);
        }
    }
}
