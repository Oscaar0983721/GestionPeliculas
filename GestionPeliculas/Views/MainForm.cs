using System;
using System.Drawing;
using System.Windows.Forms;
using GestionPeliculas.Controllers;
using GestionPeliculas.Models;
using GestionPeliculas.Views;

namespace GestionPeliculas.Views
{
    public partial class MainForm : Form
    {
        private readonly Usuario _usuarioActual;
        private readonly ContenidoController _contenidoController;
        private readonly HistorialController _historialController;
        private readonly RecomendacionController _recomendacionController;
        private readonly ReporteController _reporteController;

        public MainForm(Usuario usuario)
        {
            _usuarioActual = usuario;
            _contenidoController = new ContenidoController();
            _historialController = new HistorialController();
            _recomendacionController = new RecomendacionController();
            _reporteController = new ReporteController();
            
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.Name = "MainForm";
            this.Text = "Sistema de Gestión de Series y Películas";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

            // Crear menú principal
            MenuStrip menuPrincipal = new MenuStrip();
            menuPrincipal.Dock = DockStyle.Top;
            this.Controls.Add(menuPrincipal);

            // Menú Contenido
            ToolStripMenuItem menuContenido = new ToolStripMenuItem("Contenido");
            
            ToolStripMenuItem menuPeliculas = new ToolStripMenuItem("Películas");
            menuPeliculas.Click += (sender, e) => MostrarPeliculas();
            menuContenido.DropDownItems.Add(menuPeliculas);
            
            ToolStripMenuItem menuSeries = new ToolStripMenuItem("Series");
            menuSeries.Click += (sender, e) => MostrarSeries();
            menuContenido.DropDownItems.Add(menuSeries);
            
            ToolStripMenuItem menuBuscar = new ToolStripMenuItem("Buscar");
            menuBuscar.Click += (sender, e) => MostrarBusqueda();
            menuContenido.DropDownItems.Add(menuBuscar);
            
            menuPrincipal.Items.Add(menuContenido);

            // Menú Mi Perfil
            ToolStripMenuItem menuPerfil = new ToolStripMenuItem("Mi Perfil");
            
            ToolStripMenuItem menuHistorial = new ToolStripMenuItem("Historial");
            menuHistorial.Click += (sender, e) => MostrarHistorial();
            menuPerfil.DropDownItems.Add(menuHistorial);
            
            ToolStripMenuItem menuRecomendaciones = new ToolStripMenuItem("Recomendaciones");
            menuRecomendaciones.Click += (sender, e) => MostrarRecomendaciones();
            menuPerfil.DropDownItems.Add(menuRecomendaciones);
            
            ToolStripMenuItem menuCambiarContraseña = new ToolStripMenuItem("Cambiar Contraseña");
            menuCambiarContraseña.Click += (sender, e) => MostrarCambiarContraseña();
            menuPerfil.DropDownItems.Add(menuCambiarContraseña);
            
            menuPrincipal.Items.Add(menuPerfil);

            // Menú Reportes (solo para administradores)
            if (_usuarioActual.Rol == "Administrador")
            {
                ToolStripMenuItem menuReportes = new ToolStripMenuItem("Reportes");
                
                ToolStripMenuItem menuContenidoPopular = new ToolStripMenuItem("Contenido Popular");
                menuContenidoPopular.Click += (sender, e) => MostrarReporteContenidoPopular();
                menuReportes.DropDownItems.Add(menuContenidoPopular);
                
                ToolStripMenuItem menuUsuariosActivos = new ToolStripMenuItem("Usuarios Activos");
                menuUsuariosActivos.Click += (sender, e) => MostrarReporteUsuariosActivos();
                menuReportes.DropDownItems.Add(menuUsuariosActivos);
                
                ToolStripMenuItem menuTiempoVisualizacion = new ToolStripMenuItem("Tiempo de Visualización");
                menuTiempoVisualizacion.Click += (sender, e) => MostrarReporteTiempoVisualizacion();
                menuReportes.DropDownItems.Add(menuTiempoVisualizacion);
                
                ToolStripMenuItem menuDistribucionGeneros = new ToolStripMenuItem("Distribución de Géneros");
                menuDistribucionGeneros.Click += (sender, e) => MostrarReporteDistribucionGeneros();
                menuReportes.DropDownItems.Add(menuDistribucionGeneros);
                
                ToolStripMenuItem menuDistribucionPlataformas = new ToolStripMenuItem("Distribución de Plataformas");
                menuDistribucionPlataformas.Click += (sender, e) => MostrarReporteDistribucionPlataformas();
                menuReportes.DropDownItems.Add(menuDistribucionPlataformas);
                
                menuPrincipal.Items.Add(menuReportes);
                
                // Menú Administración
                ToolStripMenuItem menuAdmin = new ToolStripMenuItem("Administración");
                
                ToolStripMenuItem menuGestionUsuarios = new ToolStripMenuItem("Gestión de Usuarios");
                menuGestionUsuarios.Click += (sender, e) => MostrarGestionUsuarios();
                menuAdmin.DropDownItems.Add(menuGestionUsuarios);
                
                ToolStripMenuItem menuGestionContenido = new ToolStripMenuItem("Gestión de Contenido");
                menuGestionContenido.Click += (sender, e) => MostrarGestionContenido();
                menuAdmin.DropDownItems.Add(menuGestionContenido);
                
                menuPrincipal.Items.Add(menuAdmin);
            }

            // Menú Salir
            ToolStripMenuItem menuSalir = new ToolStripMenuItem("Salir");
            menuSalir.Click += (sender, e) => this.Close();
            menuPrincipal.Items.Add(menuSalir);

            // Panel principal
            Panel panelPrincipal = new Panel();
            panelPrincipal.Dock = DockStyle.Fill;
            this.Controls.Add(panelPrincipal);

            // Etiqueta de bienvenida
            Label lblBienvenida = new Label();
            lblBienvenida.Text = $"Bienvenido/a, {_usuarioActual.NombreUsuario}";
            lblBienvenida.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblBienvenida.AutoSize = true;
            lblBienvenida.Location = new Point(20, 50);
            panelPrincipal.Controls.Add(lblBienvenida);

            // Panel de recomendaciones
            Panel panelRecomendaciones = new Panel();
            panelRecomendaciones.BorderStyle = BorderStyle.FixedSingle;
            panelRecomendaciones.Location = new Point(20, 100);
            panelRecomendaciones.Size = new Size(960, 450);
            panelPrincipal.Controls.Add(panelRecomendaciones);

            Label lblRecomendaciones = new Label();
            lblRecomendaciones.Text = "Recomendaciones para ti";
            lblRecomendaciones.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblRecomendaciones.AutoSize = true;
            lblRecomendaciones.Location = new Point(10, 10);
            panelRecomendaciones.Controls.Add(lblRecomendaciones);

            // Cargar recomendaciones
            CargarRecomendaciones(panelRecomendaciones);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Centrar el formulario en la pantalla
            this.CenterToScreen();
        }

        private void CargarRecomendaciones(Panel panel)
        {
            // Obtener recomendaciones para el usuario
            var recomendaciones = _recomendacionController.GenerarRecomendaciones(_usuarioActual.Id);
            
            if (recomendaciones.Count == 0)
            {
                Label lblNoRecomendaciones = new Label();
                lblNoRecomendaciones.Text = "No hay recomendaciones disponibles en este momento.";
                lblNoRecomendaciones.AutoSize = true;
                lblNoRecomendaciones.Location = new Point(10, 50);
                panel.Controls.Add(lblNoRecomendaciones);
                return;
            }

            // Crear panel de desplazamiento
            FlowLayoutPanel flowPanel = new FlowLayoutPanel();
            flowPanel.FlowDirection = FlowDirection.LeftToRight;
            flowPanel.WrapContents = true;
            flowPanel.AutoScroll = true;
            flowPanel.Location = new Point(10, 50);
            flowPanel.Size = new Size(940, 380);
            panel.Controls.Add(flowPanel);

            // Mostrar recomendaciones
            foreach (var contenido in recomendaciones)
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
                        MostrarDetallePelicula((Pelicula)contenido);
                    }
                    else if (contenido is Serie)
                    {
                        MostrarDetalleSerie((Serie)contenido);
                    }
                };

                flowPanel.Controls.Add(panelContenido);
            }
        }

        #region Métodos para mostrar diferentes vistas
        private void MostrarPeliculas()
        {
            Form peliculasForm = new PeliculasForm(_usuarioActual);
            peliculasForm.ShowDialog();
        }

        private void MostrarSeries()
        {
            Form seriesForm = new SeriesForm(_usuarioActual);
            seriesForm.ShowDialog();
        }

        private void MostrarBusqueda()
        {
            Form busquedaForm = new BusquedaForm(_usuarioActual);
            busquedaForm.ShowDialog();
        }

        private void MostrarHistorial()
        {
            Form historialForm = new HistorialForm(_usuarioActual);
            historialForm.ShowDialog();
        }

        private void MostrarRecomendaciones()
        {
            Form recomendacionesForm = new RecomendacionesForm(_usuarioActual);
            recomendacionesForm.ShowDialog();
        }

        private void MostrarCambiarContraseña()
        {
            Form cambiarContraseñaForm = new CambiarContraseñaForm(_usuarioActual);
            cambiarContraseñaForm.ShowDialog();
        }

        private void MostrarReporteContenidoPopular()
        {
            Form reporteForm = new ReporteContenidoPopularForm();
            reporteForm.ShowDialog();
        }

        private void MostrarReporteUsuariosActivos()
        {
            Form reporteForm = new ReporteUsuariosActivosForm();
            reporteForm.ShowDialog();
        }

        private void MostrarReporteTiempoVisualizacion()
        {
            Form reporteForm = new ReporteTiempoVisualizacionForm();
            reporteForm.ShowDialog();
        }

        private void MostrarReporteDistribucionGeneros()
        {
            Form reporteForm = new ReporteDistribucionGenerosForm();
            reporteForm.ShowDialog();
        }

        private void MostrarReporteDistribucionPlataformas()
        {
            Form reporteForm = new ReporteDistribucionPlataformasForm();
            reporteForm.ShowDialog();
        }

        private void MostrarGestionUsuarios()
        {
            Form gestionUsuariosForm = new GestionUsuariosForm();
            gestionUsuariosForm.ShowDialog();
        }

        private void MostrarGestionContenido()
        {
            Form gestionContenidoForm = new GestionContenidoForm();
            gestionContenidoForm.ShowDialog();
        }

        private void MostrarDetallePelicula(Pelicula pelicula)
        {
            Form detallePeliculaForm = new DetallePeliculaForm(pelicula, _usuarioActual);
            detallePeliculaForm.ShowDialog();
        }

        private void MostrarDetalleSerie(Serie serie)
        {
            Form detalleSerieForm = new DetalleSerieForm(serie, _usuarioActual);
            detalleSerieForm.ShowDialog();
        }
        #endregion
    }
}
