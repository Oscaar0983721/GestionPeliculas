using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using GestionPeliculas.Models;
using GestionPeliculas.Controllers;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace GestionPeliculas.Services
{
    public class ReportesPdfService // Esta clase se encarga de generar reportes en formato PDF
    {
        private readonly UsuarioController _usuarioController;
        private readonly ContenidoController _contenidoController;
        private readonly HistorialController _historialController;
        private readonly ComentarioController _comentarioController;

        public ReportesPdfService() // Constructor

        {
            _usuarioController = new UsuarioController();
            _contenidoController = new ContenidoController();
            _historialController = new HistorialController();
            _comentarioController = new ComentarioController();
        }

       
        public ReportesPdfService(
            // Constructor con inyección de dependencias
            UsuarioController usuarioController,
            ContenidoController contenidoController,
            HistorialController historialController,
            ComentarioController comentarioController)
        {
            _usuarioController = usuarioController;
            _contenidoController = contenidoController;
            _historialController = historialController;
            _comentarioController = comentarioController;
        }

        public string GenerarReporteUsuariosSuscritos(DateTime fechaInicio, DateTime fechaFin) // Genera un reporte de usuarios suscritos en formato PDF
        {
            try
            {
                // Crear documento PDF
                string rutaArchivo = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ReporteUsuariosSuscritos.pdf");
                Document documento = new Document(PageSize.A4, 50, 50, 50, 50);
                PdfWriter writer = PdfWriter.GetInstance(documento, new FileStream(rutaArchivo, FileMode.Create));

                documento.Open();

                // Título del reporte
                Paragraph titulo = new Paragraph("Reporte de Usuarios Suscritos", new Font(Font.FontFamily.HELVETICA, 18, Font.BOLD));
                titulo.Alignment = Element.ALIGN_CENTER;
                titulo.SpacingAfter = 20;
                documento.Add(titulo);

                // Información del período
                Paragraph periodo = new Paragraph($"Período: {fechaInicio:dd/MM/yyyy} - {fechaFin:dd/MM/yyyy}", new Font(Font.FontFamily.HELVETICA, 12));
                periodo.SpacingAfter = 20;
                documento.Add(periodo);

                // Obtener usuarios
                var usuarios = _usuarioController.ObtenerTodosUsuarios();
                var usuariosFiltrados = usuarios.Where(u => u.FechaRegistro >= fechaInicio && u.FechaRegistro <= fechaFin).ToList();

                // Tabla de usuarios
                PdfPTable tabla = new PdfPTable(4);
                tabla.WidthPercentage = 100;
                tabla.SetWidths(new float[] { 1, 3, 3, 2 });

                // Encabezados
                tabla.AddCell(new PdfPCell(new Phrase("ID", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD))) { HorizontalAlignment = Element.ALIGN_CENTER });
                tabla.AddCell(new PdfPCell(new Phrase("Nombre de Usuario", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD))) { HorizontalAlignment = Element.ALIGN_CENTER });
                tabla.AddCell(new PdfPCell(new Phrase("Email", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD))) { HorizontalAlignment = Element.ALIGN_CENTER });
                tabla.AddCell(new PdfPCell(new Phrase("Fecha de Registro", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD))) { HorizontalAlignment = Element.ALIGN_CENTER });

                // Datos
                foreach (var usuario in usuariosFiltrados)
                {
                    tabla.AddCell(new PdfPCell(new Phrase(usuario.Id.ToString())) { HorizontalAlignment = Element.ALIGN_CENTER });
                    tabla.AddCell(new PdfPCell(new Phrase(usuario.NombreUsuario)) { HorizontalAlignment = Element.ALIGN_LEFT });
                    tabla.AddCell(new PdfPCell(new Phrase(usuario.Email)) { HorizontalAlignment = Element.ALIGN_LEFT });
                    tabla.AddCell(new PdfPCell(new Phrase(usuario.FechaRegistro.ToString("dd/MM/yyyy"))) { HorizontalAlignment = Element.ALIGN_CENTER });
                }

                documento.Add(tabla);

                // Resumen
                Paragraph resumen = new Paragraph($"\nTotal de usuarios suscritos en el período: {usuariosFiltrados.Count}", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD));
                resumen.SpacingBefore = 20;
                documento.Add(resumen);

                // Fecha de generación
                Paragraph fechaGeneracion = new Paragraph($"\nReporte generado el {DateTime.Now:dd/MM/yyyy HH:mm:ss}", new Font(Font.FontFamily.HELVETICA, 10, Font.ITALIC));
                fechaGeneracion.SpacingBefore = 30;
                documento.Add(fechaGeneracion);

                documento.Close();
                return rutaArchivo;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al generar el reporte de usuarios suscritos: {ex.Message}");
            }
        }

        public string GenerarReporteInteraccionUsuarios(DateTime fechaInicio, DateTime fechaFin) // Genera un reporte de interacción de usuarios en formato PDF
        {
            try
            {
                // Crear documento PDF
                string rutaArchivo = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ReporteInteraccionUsuarios.pdf");
                Document documento = new Document(PageSize.A4, 50, 50, 50, 50);
                PdfWriter writer = PdfWriter.GetInstance(documento, new FileStream(rutaArchivo, FileMode.Create));

                documento.Open();

                // Título del reporte
                Paragraph titulo = new Paragraph("Reporte de Interacción de Usuarios", new Font(Font.FontFamily.HELVETICA, 18, Font.BOLD));
                titulo.Alignment = Element.ALIGN_CENTER;
                titulo.SpacingAfter = 20;
                documento.Add(titulo);

                // Información del período
                Paragraph periodo = new Paragraph($"Período: {fechaInicio:dd/MM/yyyy} - {fechaFin:dd/MM/yyyy}", new Font(Font.FontFamily.HELVETICA, 12));
                periodo.SpacingAfter = 20;
                documento.Add(periodo);

                // Obtener datos de interacción
                var historial = _historialController.ObtenerHistorialEntreFechas(fechaInicio, fechaFin);
                var comentarios = _comentarioController.ObtenerTodosComentarios().Where(c => c.FechaCreacion >= fechaInicio && c.FechaCreacion <= fechaFin).ToList();

                // Agrupar por usuario
                var interaccionesPorUsuario = new Dictionary<int, (int Visualizaciones, int Comentarios, int MinutosVistos)>();

                foreach (var entrada in historial)
                {
                    if (!interaccionesPorUsuario.ContainsKey(entrada.UsuarioId))
                    {
                        interaccionesPorUsuario[entrada.UsuarioId] = (0, 0, 0);
                    }

                    var actual = interaccionesPorUsuario[entrada.UsuarioId];
                    interaccionesPorUsuario[entrada.UsuarioId] = (actual.Visualizaciones + 1, actual.Comentarios, actual.MinutosVistos + entrada.ProgresoMinutos);
                }

                foreach (var comentario in comentarios)
                {
                    if (!interaccionesPorUsuario.ContainsKey(comentario.UsuarioId))
                    {
                        interaccionesPorUsuario[comentario.UsuarioId] = (0, 1, 0);
                    }
                    else
                    {
                        var actual = interaccionesPorUsuario[comentario.UsuarioId];
                        interaccionesPorUsuario[comentario.UsuarioId] = (actual.Visualizaciones, actual.Comentarios + 1, actual.MinutosVistos);
                    }
                }

                // Tabla de interacciones
                PdfPTable tabla = new PdfPTable(5);
                tabla.WidthPercentage = 100;
                tabla.SetWidths(new float[] { 1, 3, 2, 2, 2 });

                // Encabezados
                tabla.AddCell(new PdfPCell(new Phrase("ID", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD))) { HorizontalAlignment = Element.ALIGN_CENTER });
                tabla.AddCell(new PdfPCell(new Phrase("Usuario", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD))) { HorizontalAlignment = Element.ALIGN_CENTER });
                tabla.AddCell(new PdfPCell(new Phrase("Visualizaciones", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD))) { HorizontalAlignment = Element.ALIGN_CENTER });
                tabla.AddCell(new PdfPCell(new Phrase("Comentarios", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD))) { HorizontalAlignment = Element.ALIGN_CENTER });
                tabla.AddCell(new PdfPCell(new Phrase("Minutos Vistos", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD))) { HorizontalAlignment = Element.ALIGN_CENTER });

                // Datos
                foreach (var interaccion in interaccionesPorUsuario)
                {
                    var usuario = _usuarioController.ObtenerUsuarioPorId(interaccion.Key);
                    if (usuario != null)
                    {
                        tabla.AddCell(new PdfPCell(new Phrase(usuario.Id.ToString())) { HorizontalAlignment = Element.ALIGN_CENTER });
                        tabla.AddCell(new PdfPCell(new Phrase(usuario.NombreUsuario)) { HorizontalAlignment = Element.ALIGN_LEFT });
                        tabla.AddCell(new PdfPCell(new Phrase(interaccion.Value.Visualizaciones.ToString())) { HorizontalAlignment = Element.ALIGN_CENTER });
                        tabla.AddCell(new PdfPCell(new Phrase(interaccion.Value.Comentarios.ToString())) { HorizontalAlignment = Element.ALIGN_CENTER });
                        tabla.AddCell(new PdfPCell(new Phrase(interaccion.Value.MinutosVistos.ToString())) { HorizontalAlignment = Element.ALIGN_CENTER });
                    }
                }

                documento.Add(tabla);

                // Resumen
                Paragraph resumen = new Paragraph($"\nTotal de usuarios activos en el período: {interaccionesPorUsuario.Count}", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD));
                resumen.SpacingBefore = 20;
                documento.Add(resumen);

                Paragraph totalVisualizaciones = new Paragraph($"Total de visualizaciones: {historial.Count}", new Font(Font.FontFamily.HELVETICA, 12));
                documento.Add(totalVisualizaciones);

                Paragraph totalComentarios = new Paragraph($"Total de comentarios: {comentarios.Count}", new Font(Font.FontFamily.HELVETICA, 12));
                documento.Add(totalComentarios);

                Paragraph totalMinutos = new Paragraph($"Total de minutos vistos: {historial.Sum(h => h.ProgresoMinutos)}", new Font(Font.FontFamily.HELVETICA, 12));
                documento.Add(totalMinutos);

                // Fecha de generación
                Paragraph fechaGeneracion = new Paragraph($"\nReporte generado el {DateTime.Now:dd/MM/yyyy HH:mm:ss}", new Font(Font.FontFamily.HELVETICA, 10, Font.ITALIC));
                fechaGeneracion.SpacingBefore = 30;
                documento.Add(fechaGeneracion);

                documento.Close();
                return rutaArchivo;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al generar el reporte de interacción de usuarios: {ex.Message}");
            }
        }

        public string GenerarReporteContenidoPopular(DateTime fechaInicio, DateTime fechaFin) // Genera un reporte de contenido popular en formato PDF
        {
            try
            {
                // Crear documento PDF
                string rutaArchivo = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ReporteContenidoPopular.pdf");
                Document documento = new Document(PageSize.A4, 50, 50, 50, 50);
                PdfWriter writer = PdfWriter.GetInstance(documento, new FileStream(rutaArchivo, FileMode.Create));

                documento.Open();

                // Título del reporte
                Paragraph titulo = new Paragraph("Reporte de Contenido Popular", new Font(Font.FontFamily.HELVETICA, 18, Font.BOLD));
                titulo.Alignment = Element.ALIGN_CENTER;
                titulo.SpacingAfter = 20;
                documento.Add(titulo);

                // Información del período
                Paragraph periodo = new Paragraph($"Período: {fechaInicio:dd/MM/yyyy} - {fechaFin:dd/MM/yyyy}", new Font(Font.FontFamily.HELVETICA, 12));
                periodo.SpacingAfter = 20;
                documento.Add(periodo);

                // Obtener datos de visualizaciones
                var historial = _historialController.ObtenerHistorialEntreFechas(fechaInicio, fechaFin);

                // Películas más populares
                var peliculasPopulares = historial
                    .Where(h => h.TipoContenido == "Pelicula")
                    .GroupBy(h => h.ContenidoId)
                    .Select(g => new { ContenidoId = g.Key, Visualizaciones = g.Count() })
                    .OrderByDescending(x => x.Visualizaciones)
                    .Take(10)
                    .ToList();

                // Series más populares
                var seriesPopulares = historial
                    .Where(h => h.TipoContenido == "Serie")
                    .GroupBy(h => h.ContenidoId)
                    .Select(g => new { ContenidoId = g.Key, Visualizaciones = g.Count() })
                    .OrderByDescending(x => x.Visualizaciones)
                    .Take(10)
                    .ToList();

                // Géneros más populares
                var generosPopulares = new Dictionary<string, int>();
                foreach (var entrada in historial)
                {
                    List<string> generos = new List<string>();

                    if (entrada.TipoContenido == "Pelicula")
                    {
                        var pelicula = _contenidoController.ObtenerPeliculaPorId(entrada.ContenidoId);
                        if (pelicula != null)
                        {
                            generos = pelicula.Generos;
                        }
                    }
                    else if (entrada.TipoContenido == "Serie")
                    {
                        var serie = _contenidoController.ObtenerSeriePorId(entrada.ContenidoId);
                        if (serie != null)
                        {
                            generos = serie.Generos;
                        }
                    }

                    foreach (var genero in generos)
                    {
                        if (generosPopulares.ContainsKey(genero))
                        {
                            generosPopulares[genero]++;
                        }
                        else
                        {
                            generosPopulares[genero] = 1;
                        }
                    }
                }

                var generosOrdenados = generosPopulares.OrderByDescending(x => x.Value).Take(10).ToList();

                // Sección de películas populares
                Paragraph seccionPeliculas = new Paragraph("Películas Más Populares", new Font(Font.FontFamily.HELVETICA, 14, Font.BOLD));
                seccionPeliculas.SpacingBefore = 20;
                seccionPeliculas.SpacingAfter = 10;
                documento.Add(seccionPeliculas);

                // Tabla de películas
                PdfPTable tablaPeliculas = new PdfPTable(4);
                tablaPeliculas.WidthPercentage = 100;
                tablaPeliculas.SetWidths(new float[] { 1, 4, 2, 2 });

                // Encabezados
                tablaPeliculas.AddCell(new PdfPCell(new Phrase("ID", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD))) { HorizontalAlignment = Element.ALIGN_CENTER });
                tablaPeliculas.AddCell(new PdfPCell(new Phrase("Título", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD))) { HorizontalAlignment = Element.ALIGN_CENTER });
                tablaPeliculas.AddCell(new PdfPCell(new Phrase("Visualizaciones", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD))) { HorizontalAlignment = Element.ALIGN_CENTER });
                tablaPeliculas.AddCell(new PdfPCell(new Phrase("Calificación", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD))) { HorizontalAlignment = Element.ALIGN_CENTER });

                // Datos de películas
                foreach (var pelicula in peliculasPopulares)
                {
                    var peliculaInfo = _contenidoController.ObtenerPeliculaPorId(pelicula.ContenidoId);
                    if (peliculaInfo != null)
                    {
                        tablaPeliculas.AddCell(new PdfPCell(new Phrase(peliculaInfo.Id.ToString())) { HorizontalAlignment = Element.ALIGN_CENTER });
                        tablaPeliculas.AddCell(new PdfPCell(new Phrase(peliculaInfo.Titulo)) { HorizontalAlignment = Element.ALIGN_LEFT });
                        tablaPeliculas.AddCell(new PdfPCell(new Phrase(pelicula.Visualizaciones.ToString())) { HorizontalAlignment = Element.ALIGN_CENTER });
                        tablaPeliculas.AddCell(new PdfPCell(new Phrase(peliculaInfo.CalificacionPromedio.ToString("F1"))) { HorizontalAlignment = Element.ALIGN_CENTER });
                    }
                }

                documento.Add(tablaPeliculas);

                // Sección de series populares
                Paragraph seccionSeries = new Paragraph("Series Más Populares", new Font(Font.FontFamily.HELVETICA, 14, Font.BOLD));
                seccionSeries.SpacingBefore = 20;
                seccionSeries.SpacingAfter = 10;
                documento.Add(seccionSeries);

                // Tabla de series
                PdfPTable tablaSeries = new PdfPTable(4);
                tablaSeries.WidthPercentage = 100;
                tablaSeries.SetWidths(new float[] { 1, 4, 2, 2 });

                // Encabezados
                tablaSeries.AddCell(new PdfPCell(new Phrase("ID", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD))) { HorizontalAlignment = Element.ALIGN_CENTER });
                tablaSeries.AddCell(new PdfPCell(new Phrase("Título", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD))) { HorizontalAlignment = Element.ALIGN_CENTER });
                tablaSeries.AddCell(new PdfPCell(new Phrase("Visualizaciones", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD))) { HorizontalAlignment = Element.ALIGN_CENTER });
                tablaSeries.AddCell(new PdfPCell(new Phrase("Calificación", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD))) { HorizontalAlignment = Element.ALIGN_CENTER });

                // Datos de series
                foreach (var serie in seriesPopulares)
                {
                    var serieInfo = _contenidoController.ObtenerSeriePorId(serie.ContenidoId);
                    if (serieInfo != null)
                    {
                        tablaSeries.AddCell(new PdfPCell(new Phrase(serieInfo.Id.ToString())) { HorizontalAlignment = Element.ALIGN_CENTER });
                        tablaSeries.AddCell(new PdfPCell(new Phrase(serieInfo.Titulo)) { HorizontalAlignment = Element.ALIGN_LEFT });
                        tablaSeries.AddCell(new PdfPCell(new Phrase(serie.Visualizaciones.ToString())) { HorizontalAlignment = Element.ALIGN_CENTER });
                        tablaSeries.AddCell(new PdfPCell(new Phrase(serieInfo.CalificacionPromedio.ToString("F1"))) { HorizontalAlignment = Element.ALIGN_CENTER });
                    }
                }

                documento.Add(tablaSeries);

                // Sección de géneros populares
                Paragraph seccionGeneros = new Paragraph("Géneros Más Populares", new Font(Font.FontFamily.HELVETICA, 14, Font.BOLD));
                seccionGeneros.SpacingBefore = 20;
                seccionGeneros.SpacingAfter = 10;
                documento.Add(seccionGeneros);

                // Tabla de géneros
                PdfPTable tablaGeneros = new PdfPTable(2);
                tablaGeneros.WidthPercentage = 100;
                tablaGeneros.SetWidths(new float[] { 3, 1 });

                // Encabezados
                tablaGeneros.AddCell(new PdfPCell(new Phrase("Género", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD))) { HorizontalAlignment = Element.ALIGN_CENTER });
                tablaGeneros.AddCell(new PdfPCell(new Phrase("Visualizaciones", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD))) { HorizontalAlignment = Element.ALIGN_CENTER });

                // Datos de géneros
                foreach (var genero in generosOrdenados)
                {
                    tablaGeneros.AddCell(new PdfPCell(new Phrase(genero.Key)) { HorizontalAlignment = Element.ALIGN_LEFT });
                    tablaGeneros.AddCell(new PdfPCell(new Phrase(genero.Value.ToString())) { HorizontalAlignment = Element.ALIGN_CENTER });
                }

                documento.Add(tablaGeneros);

                // Fecha de generación
                Paragraph fechaGeneracion = new Paragraph($"\nReporte generado el {DateTime.Now:dd/MM/yyyy HH:mm:ss}", new Font(Font.FontFamily.HELVETICA, 10, Font.ITALIC));
                fechaGeneracion.SpacingBefore = 30;
                documento.Add(fechaGeneracion);

                documento.Close();
                return rutaArchivo;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al generar el reporte de contenido popular: {ex.Message}");
            }
        }
    }
}
