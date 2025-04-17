using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using GestionPeliculas.Controllers;
using GestionPeliculas.Models;
using GestionPeliculas.Services;

namespace GestionPeliculas.Tests
{
    [TestClass]
    public class UsuarioControllerTests
    {
        private Mock<JsonDataService> _mockDataService;
        private UsuarioController _controller;
        private List<Usuario> _usuariosTest;

        [TestInitialize]
        public void Setup()
        {
            // Configurar datos de prueba
            _usuariosTest = new List<Usuario>
            {
                new Usuario { Id = 1, NombreUsuario = "usuario1", Contraseña = "7b3d979ca8330a94fa7e9e1b466d8b99e0bcdea1ec90596c0dcc8d68c76be8a8", Email = "usuario1@test.com", Rol = "Usuario" },
                new Usuario { Id = 2, NombreUsuario = "usuario2", Contraseña = "6025d18fe48abd45168528f18a82e265dd98d421a7084aa09f61b341703901a3", Email = "usuario2@test.com", Rol = "Usuario" },
                new Usuario { Id = 3, NombreUsuario = "admin", Contraseña = "240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9", Email = "admin@test.com", Rol = "Administrador" }
            };

            // Configurar mock
            _mockDataService = new Mock<JsonDataService>();
            _mockDataService.Setup(m => m.CargarDatos<List<Usuario>>(It.IsAny<string>())).Returns(_usuariosTest);
            _mockDataService.Setup(m => m.GuardarDatos(It.IsAny<string>(), It.IsAny<List<Usuario>>())).Returns(true);

            // Inyectar mock en el controlador
            _controller = new UsuarioController(_mockDataService.Object);
        }

        [TestMethod]
        public void ObtenerTodosUsuarios_DebeRetornarListaDeUsuarios()
        {
            // Act
            var resultado = _controller.ObtenerTodosUsuarios();

            // Assert
            Assert.IsNotNull(resultado);
            Assert.AreEqual(3, resultado.Count);
            Assert.AreEqual("usuario1", resultado[0].NombreUsuario);
            Assert.AreEqual("admin", resultado[2].NombreUsuario);
        }

        [TestMethod]
        public void ObtenerUsuarioPorId_ConIdExistente_DebeRetornarUsuario()
        {
            // Act
            var resultado = _controller.ObtenerUsuarioPorId(2);

            // Assert
            Assert.IsNotNull(resultado);
            Assert.AreEqual("usuario2", resultado.NombreUsuario);
            Assert.AreEqual("usuario2@test.com", resultado.Email);
        }

        [TestMethod]
        public void ObtenerUsuarioPorId_ConIdInexistente_DebeRetornarNull()
        {
            // Act
            var resultado = _controller.ObtenerUsuarioPorId(99);

            // Assert
            Assert.IsNull(resultado);
        }

        [TestMethod]
        public void ObtenerUsuarioPorNombre_ConNombreExistente_DebeRetornarUsuario()
        {
            // Act
            var resultado = _controller.ObtenerUsuarioPorNombre("admin");

            // Assert
            Assert.IsNotNull(resultado);
            Assert.AreEqual(3, resultado.Id);
            Assert.AreEqual("Administrador", resultado.Rol);
        }

        [TestMethod]
        public void ObtenerUsuarioPorNombre_ConNombreInexistente_DebeRetornarNull()
        {
            // Act
            var resultado = _controller.ObtenerUsuarioPorNombre("noexiste");

            // Assert
            Assert.IsNull(resultado);
        }

        [TestMethod]
        public void RegistrarUsuario_ConUsuarioNuevo_DebeRetornarTrue()
        {
            // Arrange
            var nuevoUsuario = new Usuario
            {
                NombreUsuario = "nuevo",
                Contraseña = "password123",
                Email = "nuevo@test.com",
                Rol = "Usuario"
            };

            // Act
            var resultado = _controller.RegistrarUsuario(nuevoUsuario);

            // Assert
            Assert.IsTrue(resultado);
            Assert.AreEqual(4, nuevoUsuario.Id); // Debe asignar ID 4
            _mockDataService.Verify(m => m.GuardarDatos(It.IsAny<string>(), It.IsAny<List<Usuario>>()), Times.Once);
        }

        [TestMethod]
        public void RegistrarUsuario_ConNombreExistente_DebeRetornarFalse()
        {
            // Arrange
            var usuarioExistente = new Usuario
            {
                NombreUsuario = "usuario1", // Nombre ya existente
                Contraseña = "password123",
                Email = "otro@test.com",
                Rol = "Usuario"
            };

            // Act
            var resultado = _controller.RegistrarUsuario(usuarioExistente);

            // Assert
            Assert.IsFalse(resultado);
            _mockDataService.Verify(m => m.GuardarDatos(It.IsAny<string>(), It.IsAny<List<Usuario>>()), Times.Never);
        }

        [TestMethod]
        public void ActualizarUsuario_ConUsuarioExistente_DebeRetornarTrue()
        {
            // Arrange
            var usuarioActualizado = new Usuario
            {
                Id = 2,
                NombreUsuario = "usuario2",
                Contraseña = "nuevacontraseña",
                Email = "actualizado@test.com",
                Rol = "Usuario"
            };

            // Act
            var resultado = _controller.ActualizarUsuario(usuarioActualizado);

            // Assert
            Assert.IsTrue(resultado);
            _mockDataService.Verify(m => m.GuardarDatos(It.IsAny<string>(), It.IsAny<List<Usuario>>()), Times.Once);
        }

        [TestMethod]
        public void ActualizarUsuario_ConUsuarioInexistente_DebeRetornarFalse()
        {
            // Arrange
            var usuarioInexistente = new Usuario
            {
                Id = 99, // ID inexistente
                NombreUsuario = "noexiste",
                Contraseña = "password",
                Email = "noexiste@test.com",
                Rol = "Usuario"
            };

            // Act
            var resultado = _controller.ActualizarUsuario(usuarioInexistente);

            // Assert
            Assert.IsFalse(resultado);
            _mockDataService.Verify(m => m.GuardarDatos(It.IsAny<string>(), It.IsAny<List<Usuario>>()), Times.Never);
        }

        [TestMethod]
        public void EliminarUsuario_ConIdExistente_DebeRetornarTrue()
        {
            // Act
            var resultado = _controller.EliminarUsuario(1);

            // Assert
            Assert.IsTrue(resultado);
            _mockDataService.Verify(m => m.GuardarDatos(It.IsAny<string>(), It.IsAny<List<Usuario>>()), Times.Once);
        }

        [TestMethod]
        public void EliminarUsuario_ConIdInexistente_DebeRetornarFalse()
        {
            // Act
            var resultado = _controller.EliminarUsuario(99);

            // Assert
            Assert.IsFalse(resultado);
            _mockDataService.Verify(m => m.GuardarDatos(It.IsAny<string>(), It.IsAny<List<Usuario>>()), Times.Never);
        }

        [TestMethod]
        public void AutenticarUsuario_ConCredencialesCorrectas_DebeRetornarTrue()
        {
            // Arrange - La contraseña "admin123" tiene el hash "240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9"
            
            // Act
            var resultado = _controller.AutenticarUsuario("admin", "admin123");

            // Assert
            Assert.IsTrue(resultado);
        }

        [TestMethod]
        public void AutenticarUsuario_ConContraseñaIncorrecta_DebeRetornarFalse()
        {
            // Act
            var resultado = _controller.AutenticarUsuario("admin", "contraseñaincorrecta");

            // Assert
            Assert.IsFalse(resultado);
        }

        [TestMethod]
        public void AutenticarUsuario_ConUsuarioInexistente_DebeRetornarFalse()
        {
            // Act
            var resultado = _controller.AutenticarUsuario("noexiste", "cualquiercontraseña");

            // Assert
            Assert.IsFalse(resultado);
        }
    }
}
