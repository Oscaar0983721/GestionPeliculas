using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Net;
using System.IO;
using GestionPeliculas.Controllers;
using GestionPeliculas.Models;

namespace GestionPeliculas.Views
{
    public partial class PeliculasForm : Form
    {
        private readonly Usuario _usuarioActual;
        private readonly ContenidoController _contenidoController;
        private List<Pelicula> _peliculas;

        public PeliculasForm(Usuario usuario)
        {
            _usuarioActual = usuario;
            _contenidoController = new ContenidoController();
            _peliculas = _contenidoController.ObtenerTodasPeliculas();

            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // PeliculasForm
            // 
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.Name = "PeliculasForm";
            this.Text = "Películas - Sistema de Gestión de Series y Películas";
            this.Load += new System.EventHandler(this.PeliculasForm_Load);
            this.ResumeLayout(false);

            // Panel principal
            Panel panelPrincipal = new Panel();
            panelPrincipal.Dock = DockStyle.Fill;
            this.Controls.Add(panelPrincipal);

            // Título
            Label lblTitulo = new Label();
            lblTitulo.Text = "Películas";
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

                FiltrarPeliculas(genero, plataforma);
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
                _peliculas = _contenidoController.ObtenerTodasPeliculas();
                MostrarPeliculas();
            };
            panelFiltros.Controls.Add(btnLimpiar);

            // Panel de películas
            Panel panelPeliculas = new Panel();
            panelPeliculas.BorderStyle = BorderStyle.FixedSingle;
            panelPeliculas.Location = new Point(20, 130);
            panelPeliculas.Size = new Size(960, 450);
            panelPeliculas.AutoScroll = true;
            panelPeliculas.Tag = "panelPeliculas"; // Para identificarlo al actualizar
            panelPrincipal.Controls.Add(panelPeliculas);

            // Mostrar películas
            MostrarPeliculas();
        }

        private void PeliculasForm_Load(object sender, EventArgs e)
        {
            // Centrar el formulario en la pantalla
            this.CenterToScreen();
        }

        private void MostrarPeliculas()
        {
            // Obtener el panel de películas
            Panel panelPeliculas = null;
            foreach (Control control in this.Controls[0].Controls)
            {
                if (control is Panel && control.Tag?.ToString() == "panelPeliculas")
                {
                    panelPeliculas = (Panel)control;
                    break;
                }
            }

            if (panelPeliculas == null)
            {
                return;
            }

            // Limpiar el panel
            panelPeliculas.Controls.Clear();

            // Crear panel de desplazamiento
            FlowLayoutPanel flowPanel = new FlowLayoutPanel();
            flowPanel.FlowDirection = FlowDirection.LeftToRight;
            flowPanel.WrapContents = true;
            flowPanel.AutoScroll = true;
            flowPanel.Dock = DockStyle.Fill;
            panelPeliculas.Controls.Add(flowPanel);

            // Mostrar películas
            foreach (var pelicula in _peliculas)
            {
                Panel panelPelicula = new Panel();
                panelPelicula.BorderStyle = BorderStyle.FixedSingle;
                panelPelicula.Size = new Size(180, 250);
                panelPelicula.Margin = new Padding(5);

                // Título
                Label lblTitulo = new Label();
                lblTitulo.Text = pelicula.Titulo;
                lblTitulo.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                lblTitulo.AutoSize = false;
                lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
                lblTitulo.Size = new Size(180, 40);
                lblTitulo.Location = new Point(0, 160);
                panelPelicula.Controls.Add(lblTitulo);

                // Año
                Label lblAño = new Label();
                lblAño.Text = $"Año: {pelicula.Año}";
                lblAño.AutoSize = false;
                lblAño.Size = new Size(180, 20);
                lblAño.TextAlign = ContentAlignment.MiddleCenter;
                lblAño.Location = new Point(0, 200);
                panelPelicula.Controls.Add(lblAño);

                // Calificación
                Label lblCalificacion = new Label();
                lblCalificacion.Text = $"★ {pelicula.CalificacionPromedio:F1}";
                lblCalificacion.AutoSize = false;
                lblCalificacion.Size = new Size(180, 20);
                lblCalificacion.TextAlign = ContentAlignment.MiddleCenter;
                lblCalificacion.Location = new Point(0, 220);
                panelPelicula.Controls.Add(lblCalificacion);

                // Imagen desde URL
                PictureBox pictureBox = new PictureBox();
                pictureBox.Size = new Size(160, 150);
                pictureBox.Location = new Point(10, 10);
                pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox.BackColor = Color.LightGray;

                try
                {
                    if (!string.IsNullOrEmpty(pelicula.ImagenUrl))
                    {
                        using (WebClient client = new WebClient())
                        {
                            byte[] imageData = client.DownloadData(pelicula.ImagenUrl);
                            using (MemoryStream ms = new MemoryStream(imageData))
                            {
                                pictureBox.Image = Image.FromStream(ms);
                            }
                        }
                    }
                }
                catch
                {
                    // Si hay error al cargar la imagen, mostrar un panel de color
                    pictureBox.Image = null;
                }

                panelPelicula.Controls.Add(pictureBox);

                // Evento de clic para ver detalles
                panelPelicula.Click += (sender, e) =>
                {
                    Form detallePeliculaForm = new DetallePeliculaForm(pelicula, _usuarioActual);
                    detallePeliculaForm.ShowDialog();
                };

                flowPanel.Controls.Add(panelPelicula);
            }

            // Mostrar mensaje si no hay películas
            if (_peliculas.Count == 0)
            {
                Label lblNoResultados = new Label();
                lblNoResultados.Text = "No se encontraron películas con los filtros seleccionados.";
                lblNoResultados.AutoSize = true;
                lblNoResultados.Location = new Point(10, 10);
                flowPanel.Controls.Add(lblNoResultados);
            }
        }

        private void FiltrarPeliculas(string genero, string plataforma)
        {
            _peliculas = _contenidoController.ObtenerTodasPeliculas();

            // Filtrar por género
            if (genero != "Todos")
            {
                _peliculas = _peliculas.FindAll(p => p.Generos.Contains(genero));
            }

            // Filtrar por plataforma
            if (plataforma != "Todas")
            {
                _peliculas = _peliculas.FindAll(p => p.Plataforma == plataforma);
            }

            MostrarPeliculas();
        }
    }
}
