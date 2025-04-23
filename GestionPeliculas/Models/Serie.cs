using System.Collections.Generic;

namespace GestionPeliculas.Models
{
    public class Serie : Contenido // modela una serie, sus temporadas y episodios dentro del proyecto GestionPeliculas.
    {
        // Propiedades de la serie
        public int NumeroTemporadas { get; set; }
        public int NumeroEpisodios { get; set; } = 0; // Valor por defecto
        public int NumeroEpisodiosTotales { get; set; }
        public string Creador { get; set; } = ""; // Valor por defecto
        public List<Temporada> Temporadas { get; set; } = new List<Temporada>();
    }

    public class Temporada // modela una temporada de una serie dentro del proyecto GestionPeliculas.
    {
        public int NumeroTemporada { get; set; }
        public List<Episodio> Episodios { get; set; } = new List<Episodio>();
    }

    public class Episodio // modela un episodio de una serie dentro del proyecto GestionPeliculas.
    {
        public int Id { get; set; }
        public int NumeroEpisodio { get; set; }
        public string Titulo { get; set; }
        public int Duracion { get; set; } // En minutos
        public string Descripcion { get; set; }
    }
}
