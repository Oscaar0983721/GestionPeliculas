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
    public class ComentarioControllerTests
    {
        private Mock<JsonDataService> _mockDataService;
        private ComentarioController _controller;
        private List<Comentario> _comentariosTest;

        [TestInitialize]
        public void Setup()
        {
            // Configurar datos de prueba
            _comentariosTest = new List<Comentario>
            {
                new Comentario {
                    Id = 1,
                    UsuarioId = 1,
                    NombreUsuario = "usuario1",
                    ContenidoId = 1,
                    TipoContenido = "Pelicula",
                    Texto = "Excelente película",
                    FechaCreacion = DateTime.Now.AddDays(-5)
                },
                new Comentario {
                    Id = 2,
                    UsuarioId = 1,
                    NombreUsuario = "usuario1",
                    ContenidoId = 2,
                    TipoContenido = "Serie",
                    Texto = "Me encantó esta serie",
                    FechaCreacion = DateTime.Now.AddDays(-2)
                },
                new Comentario {
                    Id = 3,
                    UsuarioId = 2,
                    NombreUsuario = "usuario2",
                    ContenidoId = 1,
                    TipoContenido = "Pelicula",
                    Texto = "No me gustó mucho",
                    FechaCreacion = DateTime.Now.AddDays(-1)
                }
            };

            // Configurar mock
            _mockDataService = new Mock<JsonDataService>();
            _mockDataService.Setup(m => m.CargarDatos<List<Comentario>>("Comentarios.json")).Returns(_comentariosTest);
            _mockDataService.Setup(m => m.GuardarDatos(It.IsAny<string>(), It.IsAny<List<Comentario>>())).Returns(true);

            // Crear controlador con mock
            _controller = new ComentarioController(_mockDataService.Object);
        }

        [TestMethod]
        public void ObtenerTodosComentarios_DebeRetornarListaDeComentarios()
        {
            // Act
            var resultado = _controller.ObtenerTodosComentarios();

            // Assert
            Assert.IsNotNull(resultado);
            Assert.AreEqual(3, resultado.Count);
        }

        [TestMethod]
        public void ObtenerComentariosPorContenido_ConContenidoExistente_DebeRetornarComentariosDelContenido()
        {
            // Act
            var resultado = _controller.ObtenerComentariosPorContenido(1, "Pelicula");

            // Assert
            Assert.IsNotNull(resultado);
            Assert.AreEqual(2, resultado.Count);
            Assert.IsTrue(resultado.All(c => c.ContenidoId == 1 && c.TipoContenido == "Pelicula"));
        }

        [TestMethod]
        public void ObtenerComentariosPorContenido_ConContenidoSinComentarios_DebeRetornarListaVacia()
        {
            // Act
            var resultado = _controller.ObtenerComentariosPorContenido(99, "Pelicula");

            // Assert
            Assert.IsNotNull(resultado);
            Assert.AreEqual(0, resultado.Count);
        }

        [TestMethod]
        public void ObtenerComentariosPorUsuario_ConUsuarioExistente_DebeRetornarComentariosDelUsuario()
        {
            // Act
            var resultado = _controller.ObtenerComentariosPorUsuario(1);

            // Assert
            Assert.IsNotNull(resultado);
            Assert.AreEqual(2, resultado.Count);
            Assert.IsTrue(resultado.All(c => c.UsuarioId == 1));
        }

        [TestMethod]
        public void ObtenerComentariosPorUsuario_ConUsuarioSinComentarios_DebeRetornarListaVacia()
        {
            // Act
            var resultado = _controller.ObtenerComentariosPorUsuario(99);

            // Assert
            Assert.IsNotNull(resultado);
            Assert.AreEqual(0, resultado.Count);
        }

        [TestMethod]
        public void AgregarComentario_ConComentarioNuevo_DebeAgregarComentario()
        {
            // Arrange
            var nuevoComentario = new Comentario
            {
                UsuarioId = 3,
                NombreUsuario = "usuario3",
                ContenidoId = 3,
                TipoContenido = "Serie",
                Texto = "Comentario de prueba"
            };

            // Act
            var resultado = _controller.AgregarComentario(nuevoComentario);

            // Assert
            Assert.IsTrue(resultado);
            Assert.AreEqual(4, nuevoComentario.Id); // Debe asignar ID 4
            _mockDataService.Verify(m => m.GuardarDatos("Comentarios.json", It.IsAny<List<Comentario>>()), Times.Once);
        }

        [TestMethod]
        public void ActualizarComentario_ConComentarioExistente_DebeActualizarComentario()
        {
            // Arrange
            var comentarioActualizado = new Comentario
            {
                Id = 1,
                UsuarioId = 1,
                NombreUsuario = "usuario1",
                ContenidoId = 1,
                TipoContenido = "Pelicula",
                Texto = "Texto actualizado",
                FechaCreacion = DateTime.Now
            };

            // Act
            var resultado = _controller.ActualizarComentario(comentarioActualizado);

            // Assert
            Assert.IsTrue(resultado);
            _mockDataService.Verify(m => m.GuardarDatos("Comentarios.json", It.IsAny<List<Comentario>>()), Times.Once);
        }

        [TestMethod]
        public void ActualizarComentario_ConComentarioInexistente_DebeRetornarFalse()
        {
            // Arrange
            var comentarioInexistente = new Comentario
            {
                Id = 99, // ID inexistente
                UsuarioId = 1,
                NombreUsuario = "usuario1",
                ContenidoId = 1,
                TipoContenido = "Pelicula",
                Texto = "Texto actualizado"
            };

            // Act
            var resultado = _controller.ActualizarComentario(comentarioInexistente);

            // Assert
            Assert.IsFalse(resultado);
            _mockDataService.Verify(m => m.GuardarDatos("Comentarios.json", It.IsAny<List<Comentario>>()), Times.Never);
        }

        [TestMethod]
        public void EliminarComentario_ConIdExistente_DebeRetornarTrue()
        {
            // Act
            var resultado = _controller.EliminarComentario(1);

            // Assert
            Assert.IsTrue(resultado);
            _mockDataService.Verify(m => m.GuardarDatos("Comentarios.json", It.IsAny<List<Comentario>>()), Times.Once);
        }

        [TestMethod]
        public void EliminarComentario_ConIdInexistente_DebeRetornarFalse()
        {
            // Act
            var resultado = _controller.EliminarComentario(99);

            // Assert
            Assert.IsFalse(resultado);
            _mockDataService.Verify(m => m.GuardarDatos("Comentarios.json", It.IsAny<List<Comentario>>()), Times.Never);
        }

        [TestMethod]
        public void ObtenerComentarioPorId_ConIdExistente_DebeRetornarComentario()
        {
            // Act
            var resultado = _controller.ObtenerComentarioPorId(1);

            // Assert
            Assert.IsNotNull(resultado);
            Assert.AreEqual(1, resultado.Id);
            Assert.AreEqual("Excelente película", resultado.Texto);
        }

        [TestMethod]
        public void ObtenerComentarioPorId_ConIdInexistente_DebeRetornarNull()
        {
            // Act
            var resultado = _controller.ObtenerComentarioPorId(99);

            // Assert
            Assert.IsNull(resultado);
        }
    }
}
