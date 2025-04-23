using System;

namespace GestionPeliculas.Models
{
    //representa una película específica en tu sistema de gestión.
    //Hereda de la clase Contenido, por lo que también incluye todas las propiedades generales de un contenido
    public class Pelicula : Contenido
    {
        public int Duracion { get; set; } // En minutos
        public string Director { get; set; }
    }
}
