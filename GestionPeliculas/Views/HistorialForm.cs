using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using GestionPeliculas.Controllers;
using GestionPeliculas.Models;

namespace GestionPeliculas.Views
{
    public partial class HistorialForm : Form
    {
        private readonly Usuario _usuarioActual;
        private readonly HistorialController _historialController;
        private readonly ContenidoController _contenidoController;
        private List<HistorialVisualizacion> _historial;

        public HistorialForm(Usuario usuario)
        {
            _usuarioActual = usuario;
            _historialController = new HistorialController();
            _contenidoController = new ContenidoController();
            _historial = _historialController.ObtenerHistorialUsuario(usuario.Id);
            
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // HistorialForm
            // 
            this.ClientSize = new System.Drawing.Size(800, 500);
            this.Name = "HistorialForm";
            this.Text = "Historial de Visualización - Sistema de Gestión de Series y Películas";
            this.Load += new System.EventHandler(this.HistorialForm_Load);
            this.ResumeLayout(false);

            // Panel principal
            Panel panelPrincipal = new Panel();
            panelPrincipal.Dock = DockStyle.Fill;
            this.Controls.Add(panelPrincipal);

            // Título
            Label lblTitulo = new Label();
            lblTitulo.Text = "Historial de Visualización";
            lblTitulo.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point(20, 20);
            panelPrincipal.Controls.Add(lblTitulo);

            // Estadísticas
            Panel panelEstadisticas = new Panel();
            panelEstadisticas.BorderStyle = BorderStyle.FixedSingle;
            panelEstadisticas.Location = new Point(20, 60);
            panelEstadisticas.Size = new Size(760, 60);
            panelPrincipal.Controls.Add(panelEstadisticas);

            // Tiempo total de visualización
            int tiempoTotal = _historialController.ObtenerTiempoTotalVisualizacion(_usuarioActual.Id);
            int horas = tiempoTotal / 60;
            int minutos = tiempoTotal % 60;
            
            Label lblTiempoTotal = new Label();
            lblTiempoTotal.Text = $"Tiempo total de visualización: {horas} horas y {minutos} minutos";
            lblTiempoTotal.Font = new Font("Segoe UI", 10);
            lblTiempoTotal.AutoSize = true;
            lblTiempoTotal.Location = new Point(10, 20);
            panelEstadisticas.Controls.Add(lblTiempoTotal);

            // Contenido visto
            int contenidoVisto = _usuarioActual.ContenidoVisto.Count;
            
            Label lblContenidoVisto = new Label();
            lblContenidoVisto.Text = $"Contenido visto: {contenidoVisto} títulos";
            lblContenidoVisto.Font = new Font("Segoe UI", 10);
            lblContenidoVisto.AutoSize = true;
            lblContenidoVisto.Location = new Point(400, 20);
            panelEstadisticas.Controls.Add(lblContenidoVisto);

            // Panel de historial
            Panel panelHistorial = new Panel();
            panelHistorial.BorderStyle = BorderStyle.FixedSingle;
            panelHistorial.Location = new Point(20, 130);
            panelHistorial.Size = new Size(760, 350);
            panelHistorial.AutoScroll = true;
            panelPrincipal.Controls.Add(panelHistorial);

            // ListView para mostrar el historial
            ListView lvHistorial = new ListView();
            lvHistorial.View = View.Details;
            lvHistorial.FullRowSelect = true;
            lvHistorial.GridLines = true;
            lvHistorial.Location = new Point(10, 10);
            lvHistorial.Size = new Size(740, 330);
            
            // Columnas
            lvHistorial.Columns.Add("Título", 250);
            lvHistorial.Columns.Add("Tipo", 100);
            lvHistorial.Columns.Add("Episodio", 150);
            lvHistorial.Columns.Add("Fecha", 150);
            lvHistorial.Columns.Add("Estado", 100);
            
            panelHistorial.Controls.Add(lvHistorial);

            // Cargar historial
            _historial = _historial.OrderByDescending(h => h.FechaVisualizacion).ToList();
            
            foreach (var entrada in _historial)
            {
                string titulo = "";
                string episodio = "N/A";
                
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
                        
                        if (entrada.EpisodioId.HasValue)
                        {
                            foreach (var temporada in serie.Temporadas)
                            {
                                var ep = temporada.Episodios.FirstOrDefault(e => e.Id == entrada.EpisodioId.Value);
                                if (ep != null)
                                {
                                    episodio = $"T{temporada.NumeroTemporada}:E{ep.NumeroEpisodio} - {ep.Titulo}";
                                    break;
                                }
                            }
                        }
                    }
                }
                
                ListViewItem item = new ListViewItem(titulo);
                item.SubItems.Add(entrada.TipoContenido);
                item.SubItems.Add(episodio);
                item.SubItems.Add(entrada.FechaVisualizacion.ToString("dd/MM/yyyy HH:mm"));
                item.SubItems.Add(entrada.Completado ? "Completado" : "En progreso");
                item.Tag = entrada;
                
                lvHistorial.Items.Add(item);
            }

            // Menú contextual para eliminar entrada
            ContextMenuStrip menuContextual = new ContextMenuStrip();
            ToolStripMenuItem menuEliminar = new ToolStripMenuItem("Eliminar de historial");
            menuEliminar.Click += (sender, e) =>
            {
                if (lvHistorial.SelectedItems.Count > 0)
                {
                    var entrada = (HistorialVisualizacion)lvHistorial.SelectedItems[0].Tag;
                    
                    if (MessageBox.Show("¿Está seguro de eliminar esta entrada del historial?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (_historialController.EliminarEntradaHistorial(entrada.Id))
                        {
                            lvHistorial.Items.Remove(lvHistorial.SelectedItems[0]);
                            MessageBox.Show("Entrada eliminada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Error al eliminar la entrada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            };
            menuContextual.Items.Add(menuEliminar);
            
            lvHistorial.ContextMenuStrip = menuContextual;
        }

        private void HistorialForm_Load(object sender, EventArgs e)
        {
            // Centrar el formulario en la pantalla
            this.CenterToScreen();
        }
    }
}
