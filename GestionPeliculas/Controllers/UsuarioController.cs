using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using GestionPeliculas.Models;
using GestionPeliculas.Services;

namespace GestionPeliculas.Controllers
{
    public class UsuarioController
    {
        private readonly JsonDataService _dataService;

        public UsuarioController()
        {
            _dataService = new JsonDataService();
        }

        // Constructor para inyección de dependencias (para testing)
        public UsuarioController(JsonDataService dataService)
        {
            _dataService = dataService;
        }

        public List<Usuario> ObtenerTodosUsuarios()
        {
            return _dataService.CargarDatos<List<Usuario>>("Usuarios.json") ?? new List<Usuario>();
        }

        public Usuario ObtenerUsuarioPorId(int id)
        {
            var usuarios = ObtenerTodosUsuarios();
            return usuarios.FirstOrDefault(u => u.Id == id);
        }

        public Usuario ObtenerUsuarioPorNombre(string nombreUsuario)
        {
            var usuarios = ObtenerTodosUsuarios();
            return usuarios.FirstOrDefault(u => u.NombreUsuario.Equals(nombreUsuario, StringComparison.OrdinalIgnoreCase));
        }

        public bool RegistrarUsuario(Usuario usuario)
        {
            var usuarios = ObtenerTodosUsuarios();

            // Verificar si ya existe el nombre de usuario
            if (usuarios.Any(u => u.NombreUsuario.Equals(usuario.NombreUsuario, StringComparison.OrdinalIgnoreCase)))
            {
                return false;
            }

            // Asignar ID
            if (usuarios.Count > 0)
            {
                usuario.Id = usuarios.Max(u => u.Id) + 1;
            }
            else
            {
                usuario.Id = 1;
            }

            // Encriptar contraseña
            usuario.Contraseña = HashContraseña(usuario.Contraseña);

            // Por defecto, asignar rol de usuario
            if (string.IsNullOrEmpty(usuario.Rol))
            {
                usuario.Rol = "Usuario";
            }

            usuarios.Add(usuario);
            return _dataService.GuardarDatos("Usuarios.json", usuarios);
        }

        public bool ActualizarUsuario(Usuario usuario)
        {
            var usuarios = ObtenerTodosUsuarios();
            var index = usuarios.FindIndex(u => u.Id == usuario.Id);

            if (index == -1)
            {
                return false;
            }

            usuarios[index] = usuario;
            return _dataService.GuardarDatos("Usuarios.json", usuarios);
        }

        public bool EliminarUsuario(int id)
        {
            var usuarios = ObtenerTodosUsuarios();
            var usuario = usuarios.FirstOrDefault(u => u.Id == id);

            if (usuario == null)
            {
                return false;
            }

            usuarios.Remove(usuario);
            return _dataService.GuardarDatos("Usuarios.json", usuarios);
        }

        public bool AutenticarUsuario(string nombreUsuario, string contraseña)
        {
            var usuario = ObtenerUsuarioPorNombre(nombreUsuario);

            if (usuario == null)
            {
                return false;
            }

            return VerificarContraseña(contraseña, usuario.Contraseña);
        }

        public void AgregarContenidoVisto(int usuarioId, int contenidoId)
        {
            var usuario = ObtenerUsuarioPorId(usuarioId);

            if (usuario != null && !usuario.ContenidoVisto.Contains(contenidoId))
            {
                usuario.ContenidoVisto.Add(contenidoId);
                ActualizarUsuario(usuario);
            }
        }

        public void CalificarContenido(int usuarioId, int contenidoId, int calificacion)
        {
            var usuario = ObtenerUsuarioPorId(usuarioId);

            if (usuario != null)
            {
                if (usuario.Calificaciones.ContainsKey(contenidoId))
                {
                    usuario.Calificaciones[contenidoId] = calificacion;
                }
                else
                {
                    usuario.Calificaciones.Add(contenidoId, calificacion);
                }

                ActualizarUsuario(usuario);
            }
        }

        private string HashContraseña(string contraseña)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(contraseña));
                StringBuilder builder = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

        private bool VerificarContraseña(string contraseñaIngresada, string contraseñaAlmacenada)
        {
            string hashContraseñaIngresada = HashContraseña(contraseñaIngresada);
            return hashContraseñaIngresada.Equals(contraseñaAlmacenada);
        }

        public void InicializarDatosUsuario()
        {
            var usuarios = ObtenerTodosUsuarios();

            if (usuarios.Count == 0)
            {
                // Crear administrador
                var admin = new Usuario
                {
                    NombreUsuario = "admin",
                    Contraseña = "admin123", // Se encriptará al registrar
                    Email = "admin@sistema.com",
                    Rol = "Administrador"
                };

                RegistrarUsuario(admin);

                // Crear 100 usuarios de prueba
                for (int i = 1; i <= 100; i++)
                {
                    var usuario = new Usuario
                    {
                        NombreUsuario = $"usuario{i}",
                        Contraseña = $"pass{i}", // Se encriptará al registrar
                        Email = $"usuario{i}@ejemplo.com",
                        Rol = "Usuario"
                    };

                    RegistrarUsuario(usuario);
                }
            }
        }
    }
}
