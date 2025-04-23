using System;
using System.Collections.Generic;

namespace GestionPeliculas.Models
{
    public abstract class Contenido // Clase base para los contenidos en el sistema GestionPeliculas
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public int Año { get; set; }
        public List<string> Generos { get; set; } = new List<string>();
        public string Plataforma { get; set; }
        public string ImagenUrl { get; set; }
        public double CalificacionPromedio { get; set; }
        public int NumeroCalificaciones { get; set; }
        public DateTime FechaAgregado { get; set; }

        public Contenido() //constructor por defecto
        // Inicializa la fecha de agregado al momento de crear el objeto
        
        {
            FechaAgregado = DateTime.Now;
        }
    }
}
