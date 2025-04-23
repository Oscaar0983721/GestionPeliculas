using System;

namespace GestionPeliculas.Models
{
    //representa una pel�cula espec�fica en tu sistema de gesti�n.
    //Hereda de la clase Contenido, por lo que tambi�n incluye todas las propiedades generales de un contenido
    public class Pelicula : Contenido
    {
        public int Duracion { get; set; } // En minutos
        public string Director { get; set; }
    }
}
