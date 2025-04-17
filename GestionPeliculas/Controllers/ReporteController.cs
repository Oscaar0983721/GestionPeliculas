using System;
using System.Collections.Generic;
using System.Linq;
using GestionPeliculas.Models;

namespace GestionPeliculas.Controllers
{
    public class ReporteController
    {
        private readonly HistorialController _historialController;
        private readonly ContenidoController _contenidoController;
        private readonly UsuarioController _usuarioController;

        public ReporteController()
        {
            _historialController = new HistorialController();
            _contenidoController = new ContenidoController();
            _usuarioController = new UsuarioController();
        }

        public Dictionary<string, int> ObtenerContenidoPopular()
        {
            var historial = _historialController.ObtenerTodoHistorial();
            var popularidad = new Dictionary<string, int>();
            
            foreach (var entrada in historial)
            {
                string titulo = "";
                
                if (entrada.TipoContenido == "Pelicula")
                {
                    var pelicula = _contenidoController.ObtenerPeliculaPorId(entrada.ContenidoId);
                    if (pelicula != null)
                    {
                        titulo = pelicula.Titulo;
                    }
                }
                else if (entrada.TipoContenido == "Serie")
                {
                    var serie = _contenidoController.ObtenerSeriePorId(entrada.ContenidoId);
                    if (serie != null)
                    {
                        titulo = serie.Titulo;
                    }
                }
                
                if (!string.IsNullOrEmpty(titulo))
                {
                    if (popularidad.ContainsKey(titulo))
                    {
                        popularidad[titulo]++;
                    }
                    else
                    {
                        popularidad[titulo] = 1;
                    }
                }
            }
            
            return popularidad.OrderByDescending(p => p.Value).ToDictionary(p => p.Key, p => p.Value);
        }

        public Dictionary<string, int> ObtenerUsuariosActivos(int dias = 30)
        {
            var historial = _historialController.ObtenerTodoHistorial();
            var fechaLimite = DateTime.Now.AddDays(-dias);
            var actividadUsuarios = new Dictionary<int, int>();
            
            foreach (var entrada in historial)
            {
                if (entrada.FechaVisualizacion >= fechaLimite)
                {
                    if (actividadUsuarios.ContainsKey(entrada.UsuarioId))
                    {
                        actividadUsuarios[entrada.UsuarioId]++;
                    }
                    else
                    {
                        actividadUsuarios[entrada.UsuarioId] = 1;
                    }
                }
            }
            
            var resultado = new Dictionary<string, int>();
            
            foreach (var usuario in actividadUsuarios)
            {
                var datosUsuario = _usuarioController.ObtenerUsuarioPorId(usuario.Key);
                if (datosUsuario != null)
                {
                    resultado[datosUsuario.NombreUsuario] = usuario.Value;
                }
            }
            
            return resultado.OrderByDescending(u => u.Value).ToDictionary(u => u.Key, u => u.Value);
        }

        public Dictionary<string, int> ObtenerTiempoVisualizacionPorUsuario()
        {
            var usuarios = _usuarioController.ObtenerTodosUsuarios();
            var resultado = new Dictionary<string, int>();
            
            foreach (var usuario in usuarios)
            {
                int tiempoTotal = _historialController.ObtenerTiempoTotalVisualizacion(usuario.Id);
                resultado[usuario.NombreUsuario] = tiempoTotal;
            }
            
            return resultado.OrderByDescending(u => u.Value).ToDictionary(u => u.Key, u => u.Value);
        }

        public Dictionary<string, int> ObtenerDistribucionGeneros()
        {
            var peliculas = _contenidoController.ObtenerTodasPeliculas();
            var series = _contenidoController.ObtenerTodasSeries();
            var distribucion = new Dictionary<string, int>();
            
            foreach (var pelicula in peliculas)
            {
                foreach (var genero in pelicula.Generos)
                {
                    if (distribucion.ContainsKey(genero))
                    {
                        distribucion[genero]++;
                    }
                    else
                    {
                        distribucion[genero] = 1;
                    }
                }
            }
            
            foreach (var serie in series)
            {
                foreach (var genero in serie.Generos)
                {
                    if (distribucion.ContainsKey(genero))
                    {
                        distribucion[genero]++;
                    }
                    else
                    {
                        distribucion[genero] = 1;
                    }
                }
            }
            
            return distribucion.OrderByDescending(g => g.Value).ToDictionary(g => g.Key, g => g.Value);
        }

        public Dictionary<string, int> ObtenerDistribucionPlataformas()
        {
            var peliculas = _contenidoController.ObtenerTodasPeliculas();
            var series = _contenidoController.ObtenerTodasSeries();
            var distribucion = new Dictionary<string, int>();
            
            foreach (var pelicula in peliculas)
            {
                if (distribucion.ContainsKey(pelicula.Plataforma))
                {
                    distribucion[pelicula.Plataforma]++;
                }
                else
                {
                    distribucion[pelicula.Plataforma] = 1;
                }
            }
            
            foreach (var serie in series)
            {
                if (distribucion.ContainsKey(serie.Plataforma))
                {
                    distribucion[serie.Plataforma]++;
                }
                else
                {
                    distribucion[serie.Plataforma] = 1;
                }
            }
            
            return distribucion.OrderByDescending(p => p.Value).ToDictionary(p => p.Key, p => p.Value);
        }

        public Dictionary<int, int> ObtenerDistribucionAños()
        {
            var peliculas = _contenidoController.ObtenerTodasPeliculas();
            var series = _contenidoController.ObtenerTodasSeries();
            var distribucion = new Dictionary<int, int>();
            
            foreach (var pelicula in peliculas)
            {
                if (distribucion.ContainsKey(pelicula.Año))
                {
                    distribucion[pelicula.Año]++;
                }
                else
                {
                    distribucion[pelicula.Año] = 1;
                }
            }
            
            foreach (var serie in series)
            {
                if (distribucion.ContainsKey(serie.Año))
                {
                    distribucion[serie.Año]++;
                }
                else
                {
                    distribucion[serie.Año] = 1;
                }
            }
            
            return distribucion.OrderBy(a => a.Key).ToDictionary(a => a.Key, a => a.Value);
        }
    }
}
