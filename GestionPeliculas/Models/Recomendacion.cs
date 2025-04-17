using System;
using System.Collections.Generic;

namespace GestionPeliculas.Models
{
    public class Recomendacion
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public List<int> ContenidosRecomendados { get; set; } = new List<int>();
        public DateTime FechaGeneracion { get; set; }

        public Recomendacion()
        {
            FechaGeneracion = DateTime.Now;
        }
    }
}
