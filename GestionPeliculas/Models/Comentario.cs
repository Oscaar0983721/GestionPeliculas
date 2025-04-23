using System;

namespace GestionPeliculas.Models
{
    public class Comentario //contiene la información asociada a un comentario hecho por un usuario sobre un contenido (película o serie).
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string NombreUsuario { get; set; }
        public int ContenidoId { get; set; }
        public string TipoContenido { get; set; } // "Pelicula" o "Serie"
        public string Texto { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int? Calificacion { get; set; } // Opcional, puede ser null

       
        public Comentario() //se ejecuta automáticamente al crear un nuevo Comentario.
        {
            FechaCreacion = DateTime.Now;
        }
    }
}
