using System;

namespace GestionPeliculas.Models
{
    public class Pelicula : Contenido
    {
        public int Duracion { get; set; } // En minutos
        public string Director { get; set; }
    }
}
