using System;
using System.Collections.Generic;
using System.Linq;
using GestionPeliculas.Models;
using GestionPeliculas.Services;

//proporciona toda la lógica para administrar comentarios
namespace GestionPeliculas.Controllers
{
    public class ComentarioController
    {
        private readonly JsonDataService _dataService;

        public ComentarioController()
        {
            _dataService = new JsonDataService();
        }

        // Constructor
        public ComentarioController(JsonDataService dataService)
        {
            _dataService = dataService;
        }

        public List<Comentario> ObtenerTodosComentarios()
        {
            return _dataService.CargarDatos<List<Comentario>>("Comentarios.json") ?? new List<Comentario>();
        }

        public List<Comentario> ObtenerComentariosPorContenido(int contenidoId, string tipoContenido)
        {
            var comentarios = ObtenerTodosComentarios();
            return comentarios
                .Where(c => c.ContenidoId == contenidoId && c.TipoContenido == tipoContenido)
                .OrderByDescending(c => c.FechaCreacion)
                .ToList();
        }

        public List<Comentario> ObtenerComentariosPorUsuario(int usuarioId)
        {
            var comentarios = ObtenerTodosComentarios();
            return comentarios
                .Where(c => c.UsuarioId == usuarioId)
                .OrderByDescending(c => c.FechaCreacion)
                .ToList();
        }

        public Comentario ObtenerComentarioPorId(int id)
        {
            var comentarios = ObtenerTodosComentarios();
            return comentarios.FirstOrDefault(c => c.Id == id);
        }

        public bool AgregarComentario(Comentario comentario)
        {
            var comentarios = ObtenerTodosComentarios();

            // Asignar ID
            if (comentarios.Count > 0)
            {
                comentario.Id = comentarios.Max(c => c.Id) + 1;
            }
            else
            {
                comentario.Id = 1;
            }

            // Asegurar que la fecha de creación esté establecida
            comentario.FechaCreacion = DateTime.Now;

            comentarios.Add(comentario);
            return _dataService.GuardarDatos("Comentarios.json", comentarios);
        }

        public bool ActualizarComentario(Comentario comentario)
        {
            var comentarios = ObtenerTodosComentarios();
            var index = comentarios.FindIndex(c => c.Id == comentario.Id);

            if (index == -1)
            {
                return false;
            }

            comentarios[index] = comentario;
            return _dataService.GuardarDatos("Comentarios.json", comentarios);
        }

        public bool EliminarComentario(int id)
        {
            var comentarios = ObtenerTodosComentarios();
            var comentario = comentarios.FirstOrDefault(c => c.Id == id);

            if (comentario == null)
            {
                return false;
            }

            comentarios.Remove(comentario);
            return _dataService.GuardarDatos("Comentarios.json", comentarios);
        }
    }
}
