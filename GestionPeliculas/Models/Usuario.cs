using System;
using System.Collections.Generic;

namespace GestionPeliculas.Models
{
    public class Usuario
        //representa a una persona que usa la aplicación para ver películas y series, calificarlas, y posiblemente administrarla si tiene ese rol.
    {
        // Propiedades del usuario
        public int Id { get; set; }
        public string NombreUsuario { get; set; }
        public string Contraseña { get; set; }
        public string Email { get; set; }
        public string Rol { get; set; } // "Usuario" o "Administrador"
        public DateTime FechaRegistro { get; set; }
        public List<int> ContenidoVisto { get; set; } = new List<int>();
        public Dictionary<int, int> Calificaciones { get; set; } = new Dictionary<int, int>(); // ContenidoId, Calificación

        public Usuario()
        {
            FechaRegistro = DateTime.Now;
        }
    }
}
