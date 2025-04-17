using System;
using System.Collections.Generic;
using System.Linq;
using GestionPeliculas.Models;
using GestionPeliculas.Services;

namespace GestionPeliculas.Controllers
{
    public class ContenidoController
    {
        private readonly JsonDataService _dataService;

        public ContenidoController()
        {
            _dataService = new JsonDataService();
        }

        // Constructor para inyección de dependencias (para testing)
        public ContenidoController(JsonDataService dataService)
        {
            _dataService = dataService;
        }

        #region Películas
        public virtual List<Pelicula> ObtenerTodasPeliculas()
        {
            return _dataService.CargarDatos<List<Pelicula>>("Peliculas.json") ?? new List<Pelicula>();
        }

        public virtual Pelicula ObtenerPeliculaPorId(int id)
        {
            var peliculas = ObtenerTodasPeliculas();
            return peliculas.FirstOrDefault(p => p.Id == id);
        }

        public virtual bool AgregarPelicula(Pelicula pelicula)
        {
            var peliculas = ObtenerTodasPeliculas();

            // Asignar ID
            if (peliculas.Count > 0)
            {
                pelicula.Id = peliculas.Max(p => p.Id) + 1;
            }
            else
            {
                pelicula.Id = 1;
            }

            peliculas.Add(pelicula);
            return _dataService.GuardarDatos("Peliculas.json", peliculas);
        }

        public virtual bool ActualizarPelicula(Pelicula pelicula)
        {
            var peliculas = ObtenerTodasPeliculas();
            var index = peliculas.FindIndex(p => p.Id == pelicula.Id);

            if (index == -1)
            {
                return false;
            }

            peliculas[index] = pelicula;
            return _dataService.GuardarDatos("Peliculas.json", peliculas);
        }

        public virtual bool EliminarPelicula(int id)
        {
            var peliculas = ObtenerTodasPeliculas();
            var pelicula = peliculas.FirstOrDefault(p => p.Id == id);

            if (pelicula == null)
            {
                return false;
            }

            peliculas.Remove(pelicula);
            return _dataService.GuardarDatos("Peliculas.json", peliculas);
        }
        #endregion

        #region Series
        public virtual List<Serie> ObtenerTodasSeries()
        {
            return _dataService.CargarDatos<List<Serie>>("Series.json") ?? new List<Serie>();
        }

        public virtual Serie ObtenerSeriePorId(int id)
        {
            var series = ObtenerTodasSeries();
            return series.FirstOrDefault(s => s.Id == id);
        }

        public virtual bool AgregarSerie(Serie serie)
        {
            var series = ObtenerTodasSeries();

            // Asignar ID
            if (series.Count > 0)
            {
                serie.Id = series.Max(s => s.Id) + 1;
            }
            else
            {
                serie.Id = 1;
            }

            // Asignar IDs a episodios
            int episodioId = 1;
            foreach (var temporada in serie.Temporadas)
            {
                foreach (var episodio in temporada.Episodios)
                {
                    episodio.Id = episodioId++;
                }
            }

            series.Add(serie);
            return _dataService.GuardarDatos("Series.json", series);
        }

        public virtual bool ActualizarSerie(Serie serie)
        {
            var series = ObtenerTodasSeries();
            var index = series.FindIndex(s => s.Id == serie.Id);

            if (index == -1)
            {
                return false;
            }

            series[index] = serie;
            return _dataService.GuardarDatos("Series.json", series);
        }

        public virtual bool EliminarSerie(int id)
        {
            var series = ObtenerTodasSeries();
            var serie = series.FirstOrDefault(s => s.Id == id);

            if (serie == null)
            {
                return false;
            }

            series.Remove(serie);
            return _dataService.GuardarDatos("Series.json", series);
        }
        #endregion

        #region Filtros
        public virtual List<Contenido> BuscarContenidoPorNombre(string nombre)
        {
            var resultado = new List<Contenido>();

            var peliculas = ObtenerTodasPeliculas()
                .Where(p => p.Titulo.ToLower().Contains(nombre.ToLower()))
                .Cast<Contenido>();

            var series = ObtenerTodasSeries()
                .Where(s => s.Titulo.ToLower().Contains(nombre.ToLower()))
                .Cast<Contenido>();

            resultado.AddRange(peliculas);
            resultado.AddRange(series);

            return resultado;
        }

        public virtual List<Contenido> FiltrarContenidoPorGenero(string genero)
        {
            var resultado = new List<Contenido>();

            var peliculas = ObtenerTodasPeliculas()
                .Where(p => p.Generos.Any(g => g.Equals(genero, StringComparison.OrdinalIgnoreCase)))
                .Cast<Contenido>();

            var series = ObtenerTodasSeries()
                .Where(s => s.Generos.Any(g => g.Equals(genero, StringComparison.OrdinalIgnoreCase)))
                .Cast<Contenido>();

            resultado.AddRange(peliculas);
            resultado.AddRange(series);

            return resultado;
        }

        public virtual List<Contenido> FiltrarContenidoPorPlataforma(string plataforma)
        {
            var resultado = new List<Contenido>();

            var peliculas = ObtenerTodasPeliculas()
                .Where(p => p.Plataforma.Equals(plataforma, StringComparison.OrdinalIgnoreCase))
                .Cast<Contenido>();

            var series = ObtenerTodasSeries()
                .Where(s => s.Plataforma.Equals(plataforma, StringComparison.OrdinalIgnoreCase))
                .Cast<Contenido>();

            resultado.AddRange(peliculas);
            resultado.AddRange(series);

            return resultado;
        }
        #endregion

        #region Calificaciones
        public virtual void ActualizarCalificacionPromedio(int contenidoId, int calificacion, string tipoContenido)
        {
            if (tipoContenido == "Pelicula")
            {
                var pelicula = ObtenerPeliculaPorId(contenidoId);
                if (pelicula != null)
                {
                    double nuevaCalificacion = (pelicula.CalificacionPromedio * pelicula.NumeroCalificaciones + calificacion) / (pelicula.NumeroCalificaciones + 1);
                    pelicula.CalificacionPromedio = Math.Round(nuevaCalificacion, 1);
                    pelicula.NumeroCalificaciones++;
                    ActualizarPelicula(pelicula);
                }
            }
            else if (tipoContenido == "Serie")
            {
                var serie = ObtenerSeriePorId(contenidoId);
                if (serie != null)
                {
                    double nuevaCalificacion = (serie.CalificacionPromedio * serie.NumeroCalificaciones + calificacion) / (serie.NumeroCalificaciones + 1);
                    serie.CalificacionPromedio = Math.Round(nuevaCalificacion, 1);
                    serie.NumeroCalificaciones++;
                    ActualizarSerie(serie);
                }
            }
        }
        #endregion

        #region Reportes
        public virtual Dictionary<string, int> ObtenerGenerosMasPopulares(DateTime fechaInicio, DateTime fechaFin)
        {
            var historialController = new HistorialController();
            var historial = historialController.ObtenerHistorialEntreFechas(fechaInicio, fechaFin);

            Dictionary<string, int> conteoGeneros = new Dictionary<string, int>();

            foreach (var registro in historial)
            {
                Contenido contenido = null;

                if (registro.TipoContenido == "Pelicula")
                {
                    contenido = ObtenerPeliculaPorId(registro.ContenidoId);
                }
                else if (registro.TipoContenido == "Serie")
                {
                    contenido = ObtenerSeriePorId(registro.ContenidoId);
                }

                if (contenido != null)
                {
                    foreach (var genero in contenido.Generos)
                    {
                        if (conteoGeneros.ContainsKey(genero))
                        {
                            conteoGeneros[genero]++;
                        }
                        else
                        {
                            conteoGeneros[genero] = 1;
                        }
                    }
                }
            }

            // Ordenar por popularidad (mayor a menor)
            return conteoGeneros.OrderByDescending(x => x.Value)
                               .ToDictionary(x => x.Key, x => x.Value);
        }

        public virtual List<Serie> ObtenerSeriesMasPopulares(DateTime fechaInicio, DateTime fechaFin, int limite = 10)
        {
            var historialController = new HistorialController();
            var historial = historialController.ObtenerHistorialEntreFechas(fechaInicio, fechaFin)
                                              .Where(h => h.TipoContenido == "Serie");

            // Contar visualizaciones por serie
            var conteoSeries = historial.GroupBy(h => h.ContenidoId)
                                       .Select(g => new { SerieId = g.Key, Visualizaciones = g.Count() })
                                       .OrderByDescending(x => x.Visualizaciones)
                                       .Take(limite)
                                       .ToList();

            // Obtener detalles de las series
            List<Serie> seriesPopulares = new List<Serie>();
            foreach (var item in conteoSeries)
            {
                var serie = ObtenerSeriePorId(item.SerieId);
                if (serie != null)
                {
                    seriesPopulares.Add(serie);
                }
            }

            return seriesPopulares;
        }

        public virtual List<Pelicula> ObtenerPeliculasMasPopulares(DateTime fechaInicio, DateTime fechaFin, int limite = 10)
        {
            var historialController = new HistorialController();
            var historial = historialController.ObtenerHistorialEntreFechas(fechaInicio, fechaFin)
                                              .Where(h => h.TipoContenido == "Pelicula");

            // Contar visualizaciones por película
            var conteoPeliculas = historial.GroupBy(h => h.ContenidoId)
                                          .Select(g => new { PeliculaId = g.Key, Visualizaciones = g.Count() })
                                          .OrderByDescending(x => x.Visualizaciones)
                                          .Take(limite)
                                          .ToList();

            // Obtener detalles de las películas
            List<Pelicula> peliculasPopulares = new List<Pelicula>();
            foreach (var item in conteoPeliculas)
            {
                var pelicula = ObtenerPeliculaPorId(item.PeliculaId);
                if (pelicula != null)
                {
                    peliculasPopulares.Add(pelicula);
                }
            }

            return peliculasPopulares;
        }
        #endregion

        public void InicializarDatosContenido()
        {
            InicializarPeliculas();
            InicializarSeries();
        }

        private void InicializarPeliculas()
        {
            var peliculas = ObtenerTodasPeliculas();

            if (peliculas.Count == 0)
            {
                // Lista de géneros para usar aleatoriamente
                string[] generos = { "Acción", "Aventura", "Comedia", "Drama", "Ciencia Ficción", "Terror", "Romance", "Animación", "Documental", "Thriller" };

                // Lista de plataformas para usar aleatoriamente
                string[] plataformas = { "Netflix", "Amazon Prime", "Disney+", "HBO Max", "Apple TV+", "Hulu", "Paramount+" };

                // Lista de directores para usar aleatoriamente
                string[] directores = { "Christopher Nolan", "Steven Spielberg", "Martin Scorsese", "Quentin Tarantino", "James Cameron", "Guillermo del Toro", "Denis Villeneuve", "Greta Gerwig", "Ava DuVernay", "Taika Waititi" };

                // Crear 50 películas de prueba
                Random random = new Random();

                for (int i = 1; i <= 50; i++)
                {
                    // Seleccionar géneros aleatorios (entre 1 y 3)
                    List<string> generosSeleccionados = new List<string>();
                    int numGeneros = random.Next(1, 4);

                    for (int j = 0; j < numGeneros; j++)
                    {
                        string genero = generos[random.Next(generos.Length)];
                        if (!generosSeleccionados.Contains(genero))
                        {
                            generosSeleccionados.Add(genero);
                        }
                    }

                    var pelicula = new Pelicula
                    {
                        Titulo = $"Película {i}",
                        Descripcion = $"Esta es la descripción de la película {i}. Una emocionante historia que cautivará a la audiencia.",
                        Año = random.Next(1990, 2024),
                        Generos = generosSeleccionados,
                        Plataforma = plataformas[random.Next(plataformas.Length)],
                        ImagenUrl = $"/imagenes/pelicula{i}.jpg",
                        CalificacionPromedio = Math.Round(random.NextDouble() * 5, 1),
                        NumeroCalificaciones = random.Next(10, 1000),
                        Duracion = random.Next(90, 180),
                        Director = directores[random.Next(directores.Length)]
                    };

                    AgregarPelicula(pelicula);
                }
            }
        }

        private void InicializarSeries()
        {
            var series = ObtenerTodasSeries();

            if (series.Count == 0)
            {
                // Lista de géneros para usar aleatoriamente
                string[] generos = { "Acción", "Aventura", "Comedia", "Drama", "Ciencia Ficción", "Terror", "Romance", "Animación", "Documental", "Thriller" };

                // Lista de plataformas para usar aleatoriamente
                string[] plataformas = { "Netflix", "Amazon Prime", "Disney+", "HBO Max", "Apple TV+", "Hulu", "Paramount+" };

                // Crear 50 series de prueba
                Random random = new Random();

                for (int i = 1; i <= 50; i++)
                {
                    // Seleccionar géneros aleatorios (entre 1 y 3)
                    List<string> generosSeleccionados = new List<string>();
                    int numGeneros = random.Next(1, 4);

                    for (int j = 0; j < numGeneros; j++)
                    {
                        string genero = generos[random.Next(generos.Length)];
                        if (!generosSeleccionados.Contains(genero))
                        {
                            generosSeleccionados.Add(genero);
                        }
                    }

                    // Crear temporadas y episodios
                    int numTemporadas = random.Next(1, 6);
                    List<Temporada> temporadas = new List<Temporada>();
                    int totalEpisodios = 0;

                    for (int t = 1; t <= numTemporadas; t++)
                    {
                        int numEpisodios = random.Next(8, 16);
                        List<Episodio> episodios = new List<Episodio>();

                        for (int e = 1; e <= numEpisodios; e++)
                        {
                            episodios.Add(new Episodio
                            {
                                NumeroEpisodio = e,
                                Titulo = $"Episodio {e}",
                                Duracion = random.Next(30, 61),
                                Descripcion = $"Descripción del episodio {e} de la temporada {t}."
                            });

                            totalEpisodios++;
                        }

                        temporadas.Add(new Temporada
                        {
                            NumeroTemporada = t,
                            Episodios = episodios
                        });
                    }

                    var serie = new Serie
                    {
                        Titulo = $"Serie {i}",
                        Descripcion = $"Esta es la descripción de la serie {i}. Una emocionante historia que se desarrolla a lo largo de varias temporadas.",
                        Año = random.Next(1990, 2024),
                        Generos = generosSeleccionados,
                        Plataforma = plataformas[random.Next(plataformas.Length)],
                        ImagenUrl = $"/imagenes/serie{i}.jpg",
                        CalificacionPromedio = Math.Round(random.NextDouble() * 5, 1),
                        NumeroCalificaciones = random.Next(10, 1000),
                        NumeroTemporadas = numTemporadas,
                        NumeroEpisodiosTotales = totalEpisodios,
                        Temporadas = temporadas
                    };

                    AgregarSerie(serie);
                }
            }
        }
    }
}
