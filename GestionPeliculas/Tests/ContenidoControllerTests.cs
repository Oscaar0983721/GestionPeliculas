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
    public class ContenidoControllerTests
    {
        private Mock<JsonDataService> _mockDataService;
        private ContenidoController _controller;
        private List<Pelicula> _peliculasTest;
        private List<Serie> _seriesTest;

        [TestInitialize]
        public void Setup()
        {
            // Configurar datos de prueba
            _peliculasTest = new List<Pelicula>
            {
                new Pelicula { 
                    Id = 1, 
                    Titulo = "Película 1", 
                    Descripcion = "Descripción película 1", 
                    Año = 2020, 
                    Generos = new List<string> { "Acción", "Aventura" }, 
                    Plataforma = "Netflix", 
                    CalificacionPromedio = 4.5, 
                    NumeroCalificaciones = 100,
                    Duracion = 120,
                    Director = "Director 1"
                },
                new Pelicula { 
                    Id = 2, 
                    Titulo = "Película 2", 
                    Descripcion = "Descripción película 2", 
                    Año = 2021, 
                    Generos = new List<string> { "Comedia", "Romance" }, 
                    Plataforma = "Amazon Prime", 
                    CalificacionPromedio = 3.8, 
                    NumeroCalificaciones = 80,
                    Duracion = 105,
                    Director = "Director 2"
                }
            };

            _seriesTest = new List<Serie>
            {
                new Serie { 
                    Id = 1, 
                    Titulo = "Serie 1", 
                    Descripcion = "Descripción serie 1", 
                    Año = 2019, 
                    Generos = new List<string> { "Drama", "Thriller" }, 
                    Plataforma = "Netflix", 
                    CalificacionPromedio = 4.2, 
                    NumeroCalificaciones = 150,
                    NumeroTemporadas = 3,
                    NumeroEpisodiosTotales = 24,
                    Temporadas = new List<Temporada>
                    {
                        new Temporada
                        {
                            NumeroTemporada = 1,
                            Episodios = new List<Episodio>
                            {
                                new Episodio { Id = 1, NumeroEpisodio = 1, Titulo = "Episodio 1", Duracion = 45 },
                                new Episodio { Id = 2, NumeroEpisodio = 2, Titulo = "Episodio 2", Duracion = 42 }
                            }
                        }
                    }
                },
                new Serie { 
                    Id = 2, 
                    Titulo = "Serie 2", 
                    Descripcion = "Descripción serie 2", 
                    Año = 2022, 
                    Generos = new List<string> { "Ciencia Ficción", "Aventura" }, 
                    Plataforma = "Disney+", 
                    CalificacionPromedio = 4.7, 
                    NumeroCalificaciones = 200,
                    NumeroTemporadas = 2,
                    NumeroEpisodiosTotales = 16,
                    Temporadas = new List<Temporada>
                    {
                        new Temporada
                        {
                            NumeroTemporada = 1,
                            Episodios = new List<Episodio>
                            {
                                new Episodio { Id = 3, NumeroEpisodio = 1, Titulo = "Episodio 1", Duracion = 50 },
                                new Episodio { Id = 4, NumeroEpisodio = 2, Titulo = "Episodio 2", Duracion = 48 }
                            }
                        }
                    }
                }
            };

            // Configurar mock
            _mockDataService = new Mock<JsonDataService>();
            _mockDataService.Setup(m => m.CargarDatos<List<Pelicula>>("Peliculas.json")).Returns(_peliculasTest);
            _mockDataService.Setup(m => m.CargarDatos<List<Serie>>("Series.json")).Returns(_seriesTest);
            _mockDataService.Setup(m => m.GuardarDatos(It.IsAny<string>(), It.IsAny<object>())).Returns(true);

            // Inyectar mock en el controlador
            _controller = new ContenidoController(_mockDataService.Object);
        }

        [TestMethod]
        public void ObtenerTodasPeliculas_DebeRetornarListaDePeliculas()
        {
            // Act
            var resultado = _controller.ObtenerTodasPeliculas();

            // Assert
            Assert.IsNotNull(resultado);
            Assert.AreEqual(2, resultado.Count);
            Assert.AreEqual("Película 1", resultado[0].Titulo);
            Assert.AreEqual("Película 2", resultado[1].Titulo);
        }

        [TestMethod]
        public void ObtenerTodasSeries_DebeRetornarListaDeSeries()
        {
            // Act
            var resultado = _controller.ObtenerTodasSeries();

            // Assert
            Assert.IsNotNull(resultado);
            Assert.AreEqual(2, resultado.Count);
            Assert.AreEqual("Serie 1", resultado[0].Titulo);
            Assert.AreEqual("Serie 2", resultado[1].Titulo);
        }

        [TestMethod]
        public void ObtenerPeliculaPorId_ConIdExistente_DebeRetornarPelicula()
        {
            // Act
            var resultado = _controller.ObtenerPeliculaPorId(1);

            // Assert
            Assert.IsNotNull(resultado);
            Assert.AreEqual("Película 1", resultado.Titulo);
            Assert.AreEqual(2020, resultado.Año);
        }

        [TestMethod]
        public void ObtenerPeliculaPorId_ConIdInexistente_DebeRetornarNull()
        {
            // Act
            var resultado = _controller.ObtenerPeliculaPorId(99);

            // Assert
            Assert.IsNull(resultado);
        }

        [TestMethod]
        public void ObtenerSeriePorId_ConIdExistente_DebeRetornarSerie()
        {
            // Act
            var resultado = _controller.ObtenerSeriePorId(2);

            // Assert
            Assert.IsNotNull(resultado);
            Assert.AreEqual("Serie 2", resultado.Titulo);
            Assert.AreEqual(2022, resultado.Año);
        }

        [TestMethod]
        public void ObtenerSeriePorId_ConIdInexistente_DebeRetornarNull()
        {
            // Act
            var resultado = _controller.ObtenerSeriePorId(99);

            // Assert
            Assert.IsNull(resultado);
        }

        [TestMethod]
        public void AgregarPelicula_DebeRetornarTrueYAsignarId()
        {
            // Arrange
            var nuevaPelicula = new Pelicula
            {
                Titulo = "Nueva Película",
                Descripcion = "Descripción nueva película",
                Año = 2023,
                Generos = new List<string> { "Acción", "Comedia" },
                Plataforma = "HBO Max",
                Duracion = 110,
                Director = "Director Nuevo"
            };

            // Act
            var resultado = _controller.AgregarPelicula(nuevaPelicula);

            // Assert
            Assert.IsTrue(resultado);
            Assert.AreEqual(3, nuevaPelicula.Id); // Debe asignar ID 3
            _mockDataService.Verify(m => m.GuardarDatos("Peliculas.json", It.IsAny<List<Pelicula>>()), Times.Once);
        }

        [TestMethod]
        public void AgregarSerie_DebeRetornarTrueYAsignarId()
        {
            // Arrange
            var nuevaSerie = new Serie
            {
                Titulo = "Nueva Serie",
                Descripcion = "Descripción nueva serie",
                Año = 2023,
                Generos = new List<string> { "Drama", "Misterio" },
                Plataforma = "Apple TV+",
                NumeroTemporadas = 1,
                NumeroEpisodiosTotales = 8,
                Temporadas = new List<Temporada>
                {
                    new Temporada
                    {
                        NumeroTemporada = 1,
                        Episodios = new List<Episodio>
                        {
                            new Episodio { NumeroEpisodio = 1, Titulo = "Episodio 1", Duracion = 55 }
                        }
                    }
                }
            };

            // Act
            var resultado = _controller.AgregarSerie(nuevaSerie);

            // Assert
            Assert.IsTrue(resultado);
            Assert.AreEqual(3, nuevaSerie.Id); // Debe asignar ID 3
            _mockDataService.Verify(m => m.GuardarDatos("Series.json", It.IsAny<List<Serie>>()), Times.Once);
        }

        [TestMethod]
        public void ActualizarPelicula_ConPeliculaExistente_DebeRetornarTrue()
        {
            // Arrange
            var peliculaActualizada = new Pelicula
            {
                Id = 1,
                Titulo = "Película 1 Actualizada",
                Descripcion = "Descripción actualizada",
                Año = 2020,
                Generos = new List<string> { "Acción", "Aventura", "Comedia" },
                Plataforma = "Netflix",
                CalificacionPromedio = 4.5,
                NumeroCalificaciones = 100,
                Duracion = 120,
                Director = "Director 1"
            };

            // Act
            var resultado = _controller.ActualizarPelicula(peliculaActualizada);

            // Assert
            Assert.IsTrue(resultado);
            _mockDataService.Verify(m => m.GuardarDatos("Peliculas.json", It.IsAny<List<Pelicula>>()), Times.Once);
        }

        [TestMethod]
        public void ActualizarPelicula_ConPeliculaInexistente_DebeRetornarFalse()
        {
            // Arrange
            var peliculaInexistente = new Pelicula
            {
                Id = 99, // ID inexistente
                Titulo = "Película Inexistente",
                Descripcion = "Descripción",
                Año = 2023,
                Generos = new List<string> { "Acción" },
                Plataforma = "Netflix",
                Duracion = 100,
                Director = "Director"
            };

            // Act
            var resultado = _controller.ActualizarPelicula(peliculaInexistente);

            // Assert
            Assert.IsFalse(resultado);
            _mockDataService.Verify(m => m.GuardarDatos("Peliculas.json", It.IsAny<List<Pelicula>>()), Times.Never);
        }

        [TestMethod]
        public void EliminarPelicula_ConIdExistente_DebeRetornarTrue()
        {
            // Act
            var resultado = _controller.EliminarPelicula(1);

            // Assert
            Assert.IsTrue(resultado);
            _mockDataService.Verify(m => m.GuardarDatos("Peliculas.json", It.IsAny<List<Pelicula>>()), Times.Once);
        }

        [TestMethod]
        public void EliminarPelicula_ConIdInexistente_DebeRetornarFalse()
        {
            // Act
            var resultado = _controller.EliminarPelicula(99);

            // Assert
            Assert.IsFalse(resultado);
            _mockDataService.Verify(m => m.GuardarDatos("Peliculas.json", It.IsAny<List<Pelicula>>()), Times.Never);
        }

        [TestMethod]
        public void BuscarContenidoPorNombre_DebeRetornarContenidoCoincidente()
        {
            // Act
            var resultado = _controller.BuscarContenidoPorNombre("película");

            // Assert
            Assert.IsNotNull(resultado);
            Assert.AreEqual(2, resultado.Count);
            Assert.IsTrue(resultado.All(c => c is Pelicula));
            Assert.IsTrue(resultado.All(c => c.Titulo.ToLower().Contains("película")));
        }

        [TestMethod]
        public void FiltrarContenidoPorGenero_DebeRetornarContenidoCoincidente()
        {
            // Act
            var resultado = _controller.FiltrarContenidoPorGenero("Aventura");

            // Assert
            Assert.IsNotNull(resultado);
            Assert.AreEqual(2, resultado.Count); // Película 1 y Serie 2 tienen género Aventura
            Assert.IsTrue(resultado.Any(c => c is Pelicula && c.Titulo == "Película 1"));
            Assert.IsTrue(resultado.Any(c => c is Serie && c.Titulo == "Serie 2"));
        }

        [TestMethod]
        public void FiltrarContenidoPorPlataforma_DebeRetornarContenidoCoincidente()
        {
            // Act
            var resultado = _controller.FiltrarContenidoPorPlataforma("Netflix");

            // Assert
            Assert.IsNotNull(resultado);
            Assert.AreEqual(2, resultado.Count); // Película 1 y Serie 1 están en Netflix
            Assert.IsTrue(resultado.Any(  resultado.Count); // Película 1 y Serie 1 están en Netflix
            Assert.IsTrue(resultado.Any(c => c is Pelicula && c.Titulo == "Película 1"));
            Assert.IsTrue(resultado.Any(c => c is Serie && c.Titulo == "Serie 1"));
        }

        [TestMethod]
        public void ActualizarCalificacionPromedio_DebeActualizarCalificacionCorrectamente()
        {
            // Act
            _controller.ActualizarCalificacionPromedio(1, 5, "Pelicula");

            // Assert
            _mockDataService.Verify(m => m.GuardarDatos("Peliculas.json", It.IsAny<List<Pelicula>>()), Times.Once);
        }
    }
}
