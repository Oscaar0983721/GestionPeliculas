using System;

namespace GestionPeliculas.Models
{
    public class Comentario
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string NombreUsuario { get; set; }
        public int ContenidoId { get; set; }
        public string TipoContenido { get; set; } // "Pelicula" o "Serie"
        public string Texto { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int? Calificacion { get; set; } // Opcional, puede ser null

        public Comentario()
        {
            FechaCreacion = DateTime.Now;
        }
    }
}
