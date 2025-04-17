using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using GestionPeliculas.Controllers;
using GestionPeliculas.Models;

namespace GestionPeliculas.Views
{
    public partial class RecomendacionesForm : Form
    {
        private readonly Usuario _usuarioActual;
        private readonly RecomendacionController _recomendacionController;
        private List<Contenido> _recomendaciones;

        public RecomendacionesForm(Usuario usuario)
        {
            _usuarioActual = usuario;
            _recomendacionController = new RecomendacionController();
            _recomendaciones = _recomendacionController.GenerarRecomendaciones(usuario.Id);
            
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // RecomendacionesForm
            // 
            this.ClientSize = new System.Drawing.Size(800, 500);
            this.Name = "RecomendacionesForm";
            this.Text = "Recomendaciones - Sistema de Gestión de Series y Películas";
            this.Load += new System.EventHandler(this.RecomendacionesForm_Load);
            this.ResumeLayout(false);

            // Panel principal
            Panel panelPrincipal = new Panel();
            panelPrincipal.Dock = DockStyle.Fill;
            this.Controls.Add(panelPrincipal);

            // Título
            Label lblTitulo = new Label();
            lblTitulo.Text = "Recomendaciones para ti";
            lblTitulo.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point(20, 20);
            panelPrincipal.Controls.Add(lblTitulo);

            // Descripción
            Label lblDescripcion = new Label();
            lblDescripcion.Text = "Basado en tu historial de visualización y tus géneros preferidos";
            lblDescripcion.Font = new Font("Segoe UI", 10);
            lblDescripcion.AutoSize = true;
            lblDescripcion.Location = new Point(20, 50);
            panelPrincipal.Controls.Add(lblDescripcion);

            // Panel de recomendaciones
            Panel panelRecomendaciones = new Panel();
            panelRecomendaciones.BorderStyle = BorderStyle.FixedSingle;
            panelRecomendaciones.Location = new Point(20, 80);
            panelRecomendaciones.Size = new Size(760, 400);
            panelRecomendaciones.AutoScroll = true;
            panelPrincipal.Controls.Add(panelRecomendaciones);

            // Crear panel de desplazamiento
            FlowLayoutPanel flowPanel = new FlowLayoutPanel();
            flowPanel.FlowDirection = FlowDirection.LeftToRight;
            flowPanel.WrapContents = true;
            flowPanel.AutoScroll = true;
            flowPanel.Dock = DockStyle.Fill;
            panelRecomendaciones.Controls.Add(flowPanel);

            // Mostrar recomendaciones
            if (_recomendaciones.Count == 0)
            {
                Label lblNoRecomendaciones = new Label();
                lblNoRecomendaciones.Text = "No hay recomendaciones disponibles en este momento.";
                lblNoRecomendaciones.AutoSize = true;
                lblNoRecomendaciones.Location = new Point(10, 10);
                flowPanel.Controls.Add(lblNoRecomendaciones);
                return;
            }

            foreach (var contenido in _recomendaciones)
            {
                Panel panelContenido = new Panel();
                panelContenido.BorderStyle = BorderStyle.FixedSingle;
                panelContenido.Size = new Size(180, 250);
                panelContenido.Margin = new Padding(5);

                // Título
                Label lblTituloContenido = new Label();
                lblTituloContenido.Text = contenido.Titulo;
                lblTituloContenido.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                lblTituloContenido.AutoSize = false;
                lblTituloContenido.TextAlign = ContentAlignment.MiddleCenter;
                lblTituloContenido.Size = new Size(180, 40);
                lblTituloContenido.Location = new Point(0, 160);
                panelContenido.Controls.Add(lblTituloContenido);

                // Año
                Label lblAño = new Label();
                lblAño.Text = $"Año: {contenido.Año}";
                lblAño.AutoSize = false;
                lblAño.Size = new Size(180, 20);
                lblAño.TextAlign = ContentAlignment.MiddleCenter;
                lblAño.Location = new Point(0, 200);
                panelContenido.Controls.Add(lblAño);

                // Calificación
                Label lblCalificacion = new Label();
                lblCalificacion.Text = $"★ {contenido.CalificacionPromedio:F1}";
                lblCalificacion.AutoSize = false;
                lblCalificacion.Size = new Size(180, 20);
                lblCalificacion.TextAlign = ContentAlignment.MiddleCenter;
                lblCalificacion.Location = new Point(0, 220);
                panelContenido.Controls.Add(lblCalificacion);

                // Imagen (simulada con un panel de color)
                Panel panelImagen = new Panel();
                panelImagen.BackColor = Color.LightGray;
                panelImagen.Size = new Size(160, 150);
                panelImagen.Location = new Point(10, 10);
                panelContenido.Controls.Add(panelImagen);

                // Tipo de contenido
                Label lblTipo = new Label();
                lblTipo.AutoSize = false;
                lblTipo.Size = new Size(160, 20);
                lblTipo.TextAlign = ContentAlignment.MiddleCenter;
                lblTipo.Location = new Point(10, 130);
                
                if (contenido is Pelicula)
                {
                    lblTipo.Text = "Película";
                    lblTipo.BackColor = Color.LightBlue;
                }
                else
                {
                    lblTipo.Text = "Serie";
                    lblTipo.BackColor = Color.LightGreen;
                }
                
                panelContenido.Controls.Add(lblTipo);

                // Evento de clic para ver detalles
                panelContenido.Click += (sender, e) =>
                {
                    if (contenido is Pelicula)
                    {
                        Form detallePeliculaForm = new DetallePeliculaForm((Pelicula)contenido, _usuarioActual);
                        detallePeliculaForm.ShowDialog();
                    }
                    else if (contenido is Serie)
                    {
                        Form detalleSerieForm = new DetalleSerieForm((Serie)contenido, _usuarioActual);
                        detalleSerieForm.ShowDialog();
                    }
                };

                flowPanel.Controls.Add(panelContenido);
            }
        }

        private void RecomendacionesForm_Load(object sender, EventArgs e)
        {
            // Centrar el formulario en la pantalla
            this.CenterToScreen();
        }
    }
}
