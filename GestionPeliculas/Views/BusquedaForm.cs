using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using GestionPeliculas.Controllers;
using GestionPeliculas.Models;

namespace GestionPeliculas.Views
{
    public partial class BusquedaForm : Form
    {
        private readonly Usuario _usuarioActual;
        private readonly ContenidoController _contenidoController;
        private List<Contenido> _resultados;

        public BusquedaForm(Usuario usuario)
        {
            _usuarioActual = usuario;
            _contenidoController = new ContenidoController();
            _resultados = new List<Contenido>();
            
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // BusquedaForm
            // 
            this.ClientSize = new System.Drawing.Size(800, 500);
            this.Name = "BusquedaForm";
            this.Text = "Búsqueda - Sistema de Gestión de Series y Películas";
            this.Load += new System.EventHandler(this.BusquedaForm_Load);
            this.ResumeLayout(false);

            // Panel principal
            Panel panelPrincipal = new Panel();
            panelPrincipal.Dock = DockStyle.Fill;
            this.Controls.Add(panelPrincipal);

            // Título
            Label lblTitulo = new Label();
            lblTitulo.Text = "Búsqueda de Contenido";
            lblTitulo.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point(20, 20);
            panelPrincipal.Controls.Add(lblTitulo);

            // Panel de búsqueda
            Panel panelBusqueda = new Panel();
            panelBusqueda.BorderStyle = BorderStyle.FixedSingle;
            panelBusqueda.Location = new Point(20, 60);
            panelBusqueda.Size = new Size(760, 60);
            panelPrincipal.Controls.Add(panelBusqueda);

            // Texto de búsqueda
            Label lblBuscar = new Label();
            lblBuscar.Text = "Buscar:";
            lblBuscar.AutoSize = true;
            lblBuscar.Location = new Point(10, 20);
            panelBusqueda.Controls.Add(lblBuscar);

            TextBox txtBuscar = new TextBox();
            txtBuscar.Location = new Point(70, 17);
            txtBuscar.Size = new Size(300, 23);
            panelBusqueda.Controls.Add(txtBuscar);

            // Botón de buscar
            Button btnBuscar = new Button();
            btnBuscar.Text = "Buscar";
            btnBuscar.Location = new Point(380, 16);
            btnBuscar.Size = new Size(80, 25);
            btnBuscar.Click += (sender, e) =>
            {
                string textoBusqueda = txtBuscar.Text.Trim();
                
                if (string.IsNullOrEmpty(textoBusqueda))
                {
                    MessageBox.Show("Por favor, ingrese un texto para buscar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                _resultados = _contenidoController.BuscarContenidoPorNombre(textoBusqueda);
                MostrarResultados();
            };
            panelBusqueda.Controls.Add(btnBuscar);

            // Botón de limpiar
            Button btnLimpiar = new Button();
            btnLimpiar.Text = "Limpiar";
            btnLimpiar.Location = new Point(470, 16);
            btnLimpiar.Size = new Size(80, 25);
            btnLimpiar.Click += (sender, e) =>
            {
                txtBuscar.Text = "";
                _resultados.Clear();
                MostrarResultados();
            };
            panelBusqueda.Controls.Add(btnLimpiar);

            // Panel de resultados
            Panel panelResultados = new Panel();
            panelResultados.BorderStyle = BorderStyle.FixedSingle;
            panelResultados.Location = new Point(20, 130);
            panelResultados.Size = new Size(760, 350);
            panelResultados.AutoScroll = true;
            panelResultados.Tag = "panelResultados"; // Para identificarlo al actualizar
            panelPrincipal.Controls.Add(panelResultados);

            // Mostrar resultados (inicialmente vacío)
            MostrarResultados();
        }

        private void BusquedaForm_Load(object sender, EventArgs e)
        {
            // Centrar el formulario en la pantalla
            this.CenterToScreen();
        }

        private void MostrarResultados()
        {
            // Obtener el panel de resultados
            Panel panelResultados = null;
            foreach (Control control in this.Controls[0].Controls)
            {
                if (control is Panel && control.Tag?.ToString() == "panelResultados")
                {
                    panelResultados = (Panel)control;
                    break;
                }
            }

            if (panelResultados == null)
            {
                return;
            }

            // Limpiar el panel
            panelResultados.Controls.Clear();

            // Crear panel de desplazamiento
            FlowLayoutPanel flowPanel = new FlowLayoutPanel();
            flowPanel.FlowDirection = FlowDirection.LeftToRight;
            flowPanel.WrapContents = true;
            flowPanel.AutoScroll = true;
            flowPanel.Dock = DockStyle.Fill;
            panelResultados.Controls.Add(flowPanel);

            // Mostrar resultados
            if (_resultados.Count == 0)
            {
                Label lblNoResultados = new Label();
                lblNoResultados.Text = "No se encontraron resultados.";
                lblNoResultados.AutoSize = true;
                lblNoResultados.Location = new Point(10, 10);
                flowPanel.Controls.Add(lblNoResultados);
                return;
            }

            foreach (var contenido in _resultados)
            {
                Panel panelContenido = new Panel();
                panelContenido.BorderStyle = BorderStyle.FixedSingle;
                panelContenido.Size = new Size(180, 250);
                panelContenido.Margin = new Padding(5);

                // Título
                Label lblTitulo = new Label();
                lblTitulo.Text = contenido.Titulo;
                lblTitulo.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                lblTitulo.AutoSize = false;
                lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
                lblTitulo.Size = new Size(180, 40);
                lblTitulo.Location = new Point(0, 160);
                panelContenido.Controls.Add(lblTitulo);

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
    }
}
