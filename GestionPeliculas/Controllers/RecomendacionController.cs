using System;
using System.Collections.Generic;
using System.Linq;
using GestionPeliculas.Models;
using GestionPeliculas.Services;

//Genera, guarda y recupera recomendaciones personalizadas de contenido (películas o series)
//para los usuarios, basándose en su historial de visualización y preferencias de género.
namespace GestionPeliculas.Controllers
{
    public class RecomendacionController
    {
        private readonly JsonDataService _dataService;
        private readonly HistorialController _historialController;
        private readonly ContenidoController _contenidoController;

        public RecomendacionController()
        {
            _dataService = new JsonDataService();
            _historialController = new HistorialController();
            _contenidoController = new ContenidoController();
        }

        public List<Recomendacion> ObtenerTodasRecomendaciones()
        {
            return _dataService.CargarDatos<List<Recomendacion>>("Recomendaciones.json") ?? new List<Recomendacion>();
        }

        public Recomendacion ObtenerRecomendacionUsuario(int usuarioId)
        {
            var recomendaciones = ObtenerTodasRecomendaciones();
            return recomendaciones.FirstOrDefault(r => r.UsuarioId == usuarioId);
        }

        public bool GuardarRecomendacion(Recomendacion recomendacion)
        {
            var recomendaciones = ObtenerTodasRecomendaciones();
            var recomendacionExistente = recomendaciones.FirstOrDefault(r => r.UsuarioId == recomendacion.UsuarioId);

            if (recomendacionExistente != null)
            {
                // Actualizar recomendación existente
                var index = recomendaciones.FindIndex(r => r.Id == recomendacionExistente.Id);
                recomendaciones[index] = recomendacion;
            }
            else
            {
                // Asignar ID
                if (recomendaciones.Count > 0)
                {
                    recomendacion.Id = recomendaciones.Max(r => r.Id) + 1;
                }
                else
                {
                    recomendacion.Id = 1;
                }

                recomendaciones.Add(recomendacion);
            }

            return _dataService.GuardarDatos("Recomendaciones.json", recomendaciones);
        }

        public List<Contenido> GenerarRecomendaciones(int usuarioId)
        {
            try
            {
                // Obtener estadísticas de géneros del usuario
                var estadisticasGenero = _historialController.ObtenerEstadisticasGenero(usuarioId);

                if (estadisticasGenero == null || estadisticasGenero.Count == 0)
                {
                    // Si no hay historial, devolver contenido popular
                    return ObtenerContenidoPopular();
                }

                // Ordenar géneros por frecuencia
                var generosOrdenados = estadisticasGenero.OrderByDescending(g => g.Value).Select(g => g.Key).ToList();

                // Obtener todo el contenido
                var peliculas = _contenidoController.ObtenerTodasPeliculas().Cast<Contenido>().ToList();
                var series = _contenidoController.ObtenerTodasSeries().Cast<Contenido>().ToList();
                var todoContenido = peliculas.Concat(series).ToList();

                if (todoContenido.Count == 0)
                {
                    return new List<Contenido>();
                }

                // Obtener historial del usuario para excluir contenido ya visto
                var historialUsuario = _historialController.ObtenerHistorialUsuario(usuarioId);
                var contenidoVisto = historialUsuario.Select(h => h.ContenidoId).Distinct().ToList();

                // Filtrar contenido no visto
                var contenidoNoVisto = todoContenido.Where(c => !contenidoVisto.Contains(c.Id)).ToList();

                if (contenidoNoVisto.Count == 0)
                {
                    // Si el usuario ha visto todo el contenido, devolver contenido popular
                    return ObtenerContenidoPopular();
                }

                // Puntuar contenido según géneros preferidos
                var contenidoPuntuado = new Dictionary<Contenido, int>();

                foreach (var contenido in contenidoNoVisto)
                {
                    int puntuacion = 0;

                    for (int i = 0; i < generosOrdenados.Count; i++)
                    {
                        if (contenido.Generos.Contains(generosOrdenados[i]))
                        {
                            // Dar más peso a los géneros más frecuentes
                            puntuacion += (generosOrdenados.Count - i);
                        }
                    }

                    // Añadir puntos por calificación
                    puntuacion += (int)(contenido.CalificacionPromedio * 2);

                    contenidoPuntuado[contenido] = puntuacion;
                }

                // Ordenar por puntuación y tomar los 10 mejores
                var recomendaciones = contenidoPuntuado.OrderByDescending(c => c.Value)
                    .Take(10)
                    .Select(c => c.Key)
                    .ToList();

                // Guardar recomendaciones
                var recomendacion = new Recomendacion
                {
                    UsuarioId = usuarioId,
                    ContenidosRecomendados = recomendaciones.Select(c => c.Id).ToList()
                };

                GuardarRecomendacion(recomendacion);

                return recomendaciones;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al generar recomendaciones: {ex.Message}");
                return new List<Contenido>();
            }
        }

        private List<Contenido> ObtenerContenidoPopular()
        {
            // Obtener todo el contenido
            var peliculas = _contenidoController.ObtenerTodasPeliculas().Cast<Contenido>().ToList();
            var series = _contenidoController.ObtenerTodasSeries().Cast<Contenido>().ToList();
            var todoContenido = peliculas.Concat(series).ToList();

            // Ordenar por calificación promedio
            return todoContenido.OrderByDescending(c => c.CalificacionPromedio).Take(10).ToList();
        }
    }
}
