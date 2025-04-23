using System;

namespace GestionPeliculas.Models
{
    //almacenar cuándo, cuánto y qué vio un usuario, y si lo terminó o no.
    public class HistorialVisualizacion
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int ContenidoId { get; set; }
        public string TipoContenido { get; set; } // "Pelicula" o "Serie"
        public int? EpisodioId { get; set; } // Null si es película
        public DateTime FechaVisualizacion { get; set; } // Renombrado para mantener consistencia con el resto del código
        public bool Completado { get; set; }
        public int ProgresoMinutos { get; set; }

        public HistorialVisualizacion()
        {
            FechaVisualizacion = DateTime.Now; 
        }
    }
}
