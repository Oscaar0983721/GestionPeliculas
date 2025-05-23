using System;
using System.Collections.Generic;
using System.Linq;
using GestionPeliculas.Models;
using GestionPeliculas.Services;

//se encarga de manejar el historial de visualizaci�n de los usuarios 

namespace GestionPeliculas.Controllers
{
   
    public class HistorialController
    {
        private readonly JsonDataService _dataService;
        private readonly UsuarioController _usuarioController;

        public HistorialController()
        {
            _dataService = new JsonDataService();
            _usuarioController = new UsuarioController();
        }

        // Constructor 
        public HistorialController(JsonDataService dataService, UsuarioController usuarioController)
        {
            _dataService = dataService;
            _usuarioController = usuarioController;
        }

        public virtual List<HistorialVisualizacion> ObtenerTodoHistorial()
        {
            return _dataService.CargarDatos<List<HistorialVisualizacion>>("HistorialVisualizacion.json") ?? new List<HistorialVisualizacion>();
        }

        public virtual List<HistorialVisualizacion> ObtenerHistorialUsuario(int usuarioId)
        {
            var historial = ObtenerTodoHistorial();
            return historial.Where(h => h.UsuarioId == usuarioId).ToList();
        }

        public virtual HistorialVisualizacion ObtenerEntradaHistorial(int usuarioId, int contenidoId, int? episodioId = null)
        {
            var historial = ObtenerTodoHistorial();
            return historial.FirstOrDefault(h =>
                h.UsuarioId == usuarioId &&
                h.ContenidoId == contenidoId &&
                (episodioId == null || h.EpisodioId == episodioId));
        }

        public virtual bool RegistrarVisualizacion(HistorialVisualizacion entrada)
        {
            var historial = ObtenerTodoHistorial();

            // Verificar si ya existe una entrada para este contenido/episodio
            var entradaExistente = ObtenerEntradaHistorial(entrada.UsuarioId, entrada.ContenidoId, entrada.EpisodioId);

            if (entradaExistente != null)
            {
                // Actualizar entrada existente
                entradaExistente.FechaVisualizacion = DateTime.Now;
                entradaExistente.Completado = entrada.Completado;
                entradaExistente.ProgresoMinutos = entrada.ProgresoMinutos;

                var index = historial.FindIndex(h => h.Id == entradaExistente.Id);
                historial[index] = entradaExistente;
            }
            else
            {
                // Asignar ID
                if (historial.Count > 0)
                {
                    entrada.Id = historial.Max(h => h.Id) + 1;
                }
                else
                {
                    entrada.Id = 1;
                }

                // Asegurar que la fecha est� establecida
                entrada.FechaVisualizacion = DateTime.Now;

                historial.Add(entrada);

                // Actualizar lista de contenido visto del usuario
                _usuarioController.AgregarContenidoVisto(entrada.UsuarioId, entrada.ContenidoId);
            }

            return _dataService.GuardarDatos("HistorialVisualizacion.json", historial);
        }

        public virtual bool EliminarEntradaHistorial(int id)
        {
            var historial = ObtenerTodoHistorial();
            var entrada = historial.FirstOrDefault(h => h.Id == id);

            if (entrada == null)
            {
                return false;
            }

            historial.Remove(entrada);
            return _dataService.GuardarDatos("HistorialVisualizacion.json", historial);
        }

        public virtual int ObtenerTiempoTotalVisualizacion(int usuarioId)
        {
            var historial = ObtenerHistorialUsuario(usuarioId);
            return historial.Sum(h => h.ProgresoMinutos);
        }

        public virtual Dictionary<string, int> ObtenerEstadisticasGenero(int usuarioId)
        {
            var historial = ObtenerHistorialUsuario(usuarioId);
            var estadisticas = new Dictionary<string, int>();

            foreach (var entrada in historial)
            {
                var contenidoController = new ContenidoController();
                List<string> generos = new List<string>();

                if (entrada.TipoContenido == "Pelicula")
                {
                    var pelicula = contenidoController.ObtenerPeliculaPorId(entrada.ContenidoId);
                    if (pelicula != null)
                    {
                        generos = pelicula.Generos;
                    }
                }
                else if (entrada.TipoContenido == "Serie")
                {
                    var serie = contenidoController.ObtenerSeriePorId(entrada.ContenidoId);
                    if (serie != null)
                    {
                        generos = serie.Generos;
                    }
                }

                foreach (var genero in generos)
                {
                    if (estadisticas.ContainsKey(genero))
                    {
                        estadisticas[genero]++;
                    }
                    else
                    {
                        estadisticas[genero] = 1;
                    }
                }
            }

            return estadisticas;
        }

        // M�todo para obtener historial entre fechas
        public virtual List<HistorialVisualizacion> ObtenerHistorialEntreFechas(DateTime fechaInicio, DateTime fechaFin)
        {
            var historial = ObtenerTodoHistorial();
            return historial.Where(h => h.FechaVisualizacion >= fechaInicio && h.FechaVisualizacion <= fechaFin).ToList();
        }
    }
}
