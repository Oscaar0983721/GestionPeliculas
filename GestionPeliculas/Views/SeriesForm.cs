using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using GestionPeliculas.Controllers;
using GestionPeliculas.Models;

namespace GestionPeliculas.Views
{
    public partial class SeriesForm : Form
    {
        private readonly Usuario _usuarioActual;
        private readonly ContenidoController _contenidoController;
        private List<Serie> _series;

        public SeriesForm(Usuario usuario)
        {
            _usuarioActual = usuario;
            _contenidoController = new ContenidoController();
            _series = _contenidoController.ObtenerTodasSeries();
            
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // SeriesForm
            // 
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.Name = "SeriesForm";
            this.Text = "Series - Sistema de Gestión de Series y Películas";
            this.Load += new System.EventHandler(this.SeriesForm_Load);
            this.ResumeLayout(false);

            // Panel principal
            Panel panelPrincipal = new Panel();
            panelPrincipal.Dock = DockStyle.Fill;
            this.Controls.Add(panelPrincipal);

            // Título
            Label lblTitulo = new Label();
            lblTitulo.Text = "Series";
            lblTitulo.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point(20, 20);
            panelPrincipal.Controls.Add(lblTitulo);

            // Panel de filtros
            Panel panelFiltros = new Panel();
            panelFiltros.BorderStyle = BorderStyle.FixedSingle;
            panelFiltros.Location = new Point(20, 60);
            panelFiltros.Size = new Size(960, 60);
            panelPrincipal.Controls.Add(panelFiltros);

            // Filtro por género
            Label lblGenero = new Label();
            lblGenero.Text = "Género:";
            lblGenero.AutoSize = true;
            lblGenero.Location = new Point(10, 20);
            panelFiltros.Controls.Add(lblGenero);

            ComboBox cmbGenero = new ComboBox();
            cmbGenero.Location = new Point(70, 17);
            cmbGenero.Size = new Size(150, 23);
            cmbGenero.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbGenero.Items.Add("Todos");
            cmbGenero.Items.Add("Acción");
            cmbGenero.Items.Add("Aventura");
            cmbGenero.Items.Add("Comedia");
            cmbGenero.Items.Add("Drama");
            cmbGenero.Items.Add("Ciencia Ficción");
            cmbGenero.Items.Add("Terror");
            cmbGenero.Items.Add("Romance");
            cmbGenero.Items.Add("Animación");
            cmbGenero.Items.Add("Documental");
            cmbGenero.Items.Add("Thriller");
            cmbGenero.SelectedIndex = 0;
            panelFiltros.Controls.Add(cmbGenero);

            // Filtro por plataforma
            Label lblPlataforma = new Label();
            lblPlataforma.Text = "Plataforma:";
            lblPlataforma.AutoSize = true;
            lblPlataforma.Location = new Point(240, 20);
            panelFiltros.Controls.Add(lblPlataforma);

            ComboBox cmbPlataforma = new ComboBox();
            cmbPlataforma.Location = new Point(320, 17);
            cmbPlataforma.Size = new Size(150, 23);
            cmbPlataforma.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPlataforma.Items.Add("Todas");
            cmbPlataforma.Items.Add("Netflix");
            cmbPlataforma.Items.Add("Amazon Prime");
            cmbPlataforma.Items.Add("Disney+");
            cmbPlataforma.Items.Add("HBO Max");
            cmbPlataforma.Items.Add("Apple TV+");
            cmbPlataforma.Items.Add("Hulu");
            cmbPlataforma.Items.Add("Paramount+");
            cmbPlataforma.SelectedIndex = 0;
            panelFiltros.Controls.Add(cmbPlataforma);

            // Botón de filtrar
            Button btnFiltrar = new Button();
            btnFiltrar.Text = "Filtrar";
            btnFiltrar.Location = new Point(490, 16);
            btnFiltrar.Size = new Size(80, 25);
            btnFiltrar.Click += (sender, e) =>
            {
                string genero = cmbGenero.SelectedItem.ToString();
                string plataforma = cmbPlataforma.SelectedItem.ToString();
                
                FiltrarSeries(genero, plataforma);
            };
            panelFiltros.Controls.Add(btnFiltrar);

            // Botón de limpiar filtros
            Button btnLimpiar = new Button();
            btnLimpiar.Text = "Limpiar";
            btnLimpiar.Location = new Point(580, 16);
            btnLimpiar.Size = new Size(80, 25);
            btnLimpiar.Click += (sender, e) =>
            {
                cmbGenero.SelectedIndex = 0;
                cmbPlataforma.SelectedIndex = 0;
                _series = _contenidoController.ObtenerTodasSeries();
                MostrarSeries();
            };
            panelFiltros.Controls.Add(btnLimpiar);

            // Panel de series
            Panel panelSeries = new Panel();
            panelSeries.BorderStyle = BorderStyle.FixedSingle;
            panelSeries.Location = new Point(20, 130);
            panelSeries.Size = new Size(960, 450);
            panelSeries.AutoScroll = true;
            panelSeries.Tag = "panelSeries"; // Para identificarlo al actualizar
            panelPrincipal.Controls.Add(panelSeries);

            // Mostrar series
            MostrarSeries();
        }

        private void SeriesForm_Load(object sender, EventArgs e)
        {
            // Centrar el formulario en la pantalla
            this.CenterToScreen();
        }

        private void MostrarSeries()
        {
            // Obtener el panel de series
            Panel panelSeries = null;
            foreach (Control control in this.Controls[0].Controls)
            {
                if (control is Panel && control.Tag?.ToString() == "panelSeries")
                {
                    panelSeries = (Panel)control;
                    break;
                }
            }

            if (panelSeries == null)
            {
                return;
            }

            // Limpiar el panel
            panelSeries.Controls.Clear();

            // Crear panel de desplazamiento
            FlowLayoutPanel flowPanel = new FlowLayoutPanel();
            flowPanel.FlowDirection = FlowDirection.LeftToRight;
            flowPanel.WrapContents = true;
            flowPanel.AutoScroll = true;
            flowPanel.Dock = DockStyle.Fill;
            panelSeries.Controls.Add(flowPanel);

            // Mostrar series
            foreach (var serie in _series)
            {
                Panel panelSerie = new Panel();
                panelSerie.BorderStyle = BorderStyle.FixedSingle;
                panelSerie.Size = new Size(180, 250);
                panelSerie.Margin = new Padding(5);

                // Título
                Label lblTitulo = new Label();
                lblTitulo.Text = serie.Titulo;
                lblTitulo.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                lblTitulo.AutoSize = false;
                lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
                lblTitulo.Size = new Size(180, 40);
                lblTitulo.Location = new Point(0, 160);
                panelSerie.Controls.Add(lblTitulo);

                // Temporadas
                Label lblTemporadas = new Label();
                lblTemporadas.Text = $"Temporadas: {serie.NumeroTemporadas}";
                lblTemporadas.AutoSize = false;
                lblTemporadas.Size = new Size(180, 20);
                lblTemporadas.TextAlign = ContentAlignment.MiddleCenter;
                lblTemporadas.Location = new Point(0, 200);
                panelSerie.Controls.Add(lblTemporadas);

                // Calificación
                Label lblCalificacion = new Label();
                lblCalificacion.Text = $"★ {serie.CalificacionPromedio:F1}";
                lblCalificacion.AutoSize = false;
                lblCalificacion.Size = new Size(180, 20);
                lblCalificacion.TextAlign = ContentAlignment.MiddleCenter;
                lblCalificacion.Location = new Point(0, 220);
                panelSerie.Controls.Add(lblCalificacion);

                // Imagen (simulada con un panel de color)
                Panel panelImagen = new Panel();
                panelImagen.BackColor = Color.LightGray;
                panelImagen.Size = new Size(160, 150);
                panelImagen.Location = new Point(10, 10);
                panelSerie.Controls.Add(panelImagen);

                // Evento de clic para ver detalles
                panelSerie.Click += (sender, e) =>
                {
                    Form detalleSerieForm = new DetalleSerieForm(serie, _usuarioActual);
                    detalleSerieForm.ShowDialog();
                };

                flowPanel.Controls.Add(panelSerie);
            }

            // Mostrar mensaje si no hay series
            if (_series.Count == 0)
            {
                Label lblNoResultados = new Label();
                lblNoResultados.Text = "No se encontraron series con los filtros seleccionados.";
                lblNoResultados.AutoSize = true;
                lblNoResultados.Location = new Point(10, 10);
                flowPanel.Controls.Add(lblNoResultados);
            }
        }

        private void FiltrarSeries(string genero, string plataforma)
        {
            _series = _contenidoController.ObtenerTodasSeries();

            // Filtrar por género
            if (genero != "Todos")
            {
                _series = _series.FindAll(s => s.Generos.Contains(genero));
            }

            // Filtrar por plataforma
            if (plataforma != "Todas")
            {
                _series = _series.FindAll(s => s.Plataforma == plataforma);
            }

            MostrarSeries();
        }
    }
}
