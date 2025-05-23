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
    public class HistorialControllerTests
    {
        private Mock<JsonDataService> _mockDataService;
        private Mock<UsuarioController> _mockUsuarioController;
        private HistorialController _controller;
        private List<HistorialVisualizacion> _historialTest;

        [TestInitialize]
        public void Setup()
        {
            // Configurar datos de prueba
            _historialTest = new List<HistorialVisualizacion>
            {
                new HistorialVisualizacion {
                    Id = 1,
                    UsuarioId = 1,
                    ContenidoId = 1,
                    TipoContenido = "Pelicula",
                    FechaVisualizacion = DateTime.Now.AddDays(-5),
                    Completado = true,
                    ProgresoMinutos = 120
                },
                new HistorialVisualizacion {
                    Id = 2,
                    UsuarioId = 1,
                    ContenidoId = 2,
                    TipoContenido = "Serie",
                    EpisodioId = 1,
                    FechaVisualizacion = DateTime.Now.AddDays(-2),
                    Completado = true,
                    ProgresoMinutos = 45
                },
                new HistorialVisualizacion {
                    Id = 3,
                    UsuarioId = 2,
                    ContenidoId = 1,
                    TipoContenido = "Pelicula",
                    FechaVisualizacion = DateTime.Now.AddDays(-1),
                    Completado = false,
                    ProgresoMinutos = 60
                }
            };

            // Configurar mock de JsonDataService
            _mockDataService = new Mock<JsonDataService>();

            // Configuración específica para CargarDatos
            _mockDataService
                .Setup(m => m.CargarDatos<List<HistorialVisualizacion>>(It.IsAny<string>()))
                .Returns(_historialTest);

            // Configuración para GuardarDatos con cualquier tipo
            _mockDataService
                .Setup(m => m.GuardarDatos(It.IsAny<string>(), It.IsAny<object>()))
                .Returns(true);

            // Configuración específica para GuardarDatos con List<HistorialVisualizacion>
            _mockDataService
                .Setup(m => m.GuardarDatos(It.IsAny<string>(), It.IsAny<List<HistorialVisualizacion>>()))
                .Returns(true);

            // Configurar mock de UsuarioController
            _mockUsuarioController = new Mock<UsuarioController>(MockBehavior.Loose, _mockDataService.Object);
            _mockUsuarioController
                .Setup(m => m.AgregarContenidoVisto(It.IsAny<int>(), It.IsAny<int>()));

            // Crear controlador con mocks
            _controller = new HistorialController(_mockDataService.Object, _mockUsuarioController.Object);
        }

        [TestMethod]
        public void ObtenerTodoHistorial_DebeRetornarListaDeHistorial()
        {
            // Act
            var resultado = _controller.ObtenerTodoHistorial();

            // Assert
            Assert.IsNotNull(resultado);
            Assert.AreEqual(3, resultado.Count);
        }

        [TestMethod]
        public void ObtenerHistorialUsuario_ConUsuarioExistente_DebeRetornarHistorialDelUsuario()
        {
            // Act
            var resultado = _controller.ObtenerHistorialUsuario(1);

            // Assert
            Assert.IsNotNull(resultado);
            Assert.AreEqual(2, resultado.Count);
            Assert.IsTrue(resultado.All(h => h.UsuarioId == 1));
        }

        [TestMethod]
        public void ObtenerHistorialUsuario_ConUsuarioSinHistorial_DebeRetornarListaVacia()
        {
            // Act
            var resultado = _controller.ObtenerHistorialUsuario(99);

            // Assert
            Assert.IsNotNull(resultado);
            Assert.AreEqual(0, resultado.Count);
        }

        [TestMethod]
        public void ObtenerEntradaHistorial_ConParametrosExistentes_DebeRetornarEntrada()
        {
            // Act
            var resultado = _controller.ObtenerEntradaHistorial(1, 2, 1);

            // Assert
            Assert.IsNotNull(resultado);
            Assert.AreEqual(2, resultado.Id);
            Assert.AreEqual("Serie", resultado.TipoContenido);
        }

        [TestMethod]
        public void ObtenerEntradaHistorial_ConParametrosInexistentes_DebeRetornarNull()
        {
            // Act
            var resultado = _controller.ObtenerEntradaHistorial(99, 99);

            // Assert
            Assert.IsNull(resultado);
        }

        [TestMethod]
        public void RegistrarVisualizacion_ConEntradaNueva_DebeAgregarEntrada()
        {
            // Arrange
            var nuevaEntrada = new HistorialVisualizacion
            {
                UsuarioId = 3,
                ContenidoId = 3,
                TipoContenido = "Pelicula",
                Completado = true,
                ProgresoMinutos = 95
            };

            // Act
            var resultado = _controller.RegistrarVisualizacion(nuevaEntrada);

            // Assert
            Assert.IsTrue(resultado);
            Assert.AreEqual(4, nuevaEntrada.Id); // Debe asignar ID 4
        }

        [TestMethod]
        public void RegistrarVisualizacion_ConEntradaExistente_DebeActualizarEntrada()
        {
            // Arrange
            var entradaExistente = new HistorialVisualizacion
            {
                UsuarioId = 1,
                ContenidoId = 1,
                TipoContenido = "Pelicula",
                Completado = false,
                ProgresoMinutos = 90
            };

            // Act
            var resultado = _controller.RegistrarVisualizacion(entradaExistente);

            // Assert
            Assert.IsTrue(resultado);
        }

        [TestMethod]
        public void EliminarEntradaHistorial_ConIdExistente_DebeRetornarTrue()
        {
            // Act
            var resultado = _controller.EliminarEntradaHistorial(1);

            // Assert
            Assert.IsTrue(resultado);
        }

        [TestMethod]
        public void EliminarEntradaHistorial_ConIdInexistente_DebeRetornarFalse()
        {
            // Act
            var resultado = _controller.EliminarEntradaHistorial(99);

            // Assert
            Assert.IsFalse(resultado);
        }

        [TestMethod]
        public void ObtenerTiempoTotalVisualizacion_DebeRetornarSumaDeMinutos()
        {
            // Act
            var resultado = _controller.ObtenerTiempoTotalVisualizacion(1);

            // Assert
            Assert.AreEqual(165, resultado); // 120 + 45 = 165 minutos
        }

        [TestMethod]
        public void ObtenerHistorialEntreFechas_DebeRetornarHistorialEntreFechas()
        {
            // Arrange
            var fechaInicio = DateTime.Now.AddDays(-3);
            var fechaFin = DateTime.Now;

            // Act
            var resultado = _controller.ObtenerHistorialEntreFechas(fechaInicio, fechaFin);

            // Assert
            Assert.IsNotNull(resultado);
            Assert.AreEqual(2, resultado.Count);
        }
    }
}
