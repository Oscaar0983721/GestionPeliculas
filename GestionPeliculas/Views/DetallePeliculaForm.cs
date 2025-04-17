using System;
using System.Drawing;
using System.Windows.Forms;
using GestionPeliculas.Controllers;
using GestionPeliculas.Models;

namespace GestionPeliculas.Views
{
    public partial class DetallePeliculaForm : Form
    {
        private readonly Pelicula _pelicula;
        private readonly Usuario _usuarioActual;
        private readonly HistorialController _historialController;
        private readonly UsuarioController _usuarioController;
        private readonly ComentarioController _comentarioController;

        public DetallePeliculaForm(Pelicula pelicula, Usuario usuario)
        {
            _pelicula = pelicula;
            _usuarioActual = usuario;
            _historialController = new HistorialController();
            _usuarioController = new UsuarioController();
            _comentarioController = new ComentarioController();

            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // DetallePeliculaForm
            // 
            this.ClientSize = new System.Drawing.Size(800, 550);
            this.Name = "DetallePeliculaForm";
            this.Text = $"Detalle de Película - {_pelicula.Titulo}";
            this.Load += new System.EventHandler(this.DetallePeliculaForm_Load);
            this.ResumeLayout(false);

            // Panel principal
            Panel panelPrincipal = new Panel();
            panelPrincipal.Dock = DockStyle.Fill;
            this.Controls.Add(panelPrincipal);

            // Panel de imagen (simulada con un panel de color)
            Panel panelImagen = new Panel();
            panelImagen.BackColor = Color.LightGray;
            panelImagen.Size = new Size(200, 300);
            panelImagen.Location = new Point(20, 20);
            panelPrincipal.Controls.Add(panelImagen);

            // Panel de información
            Panel panelInfo = new Panel();
            panelInfo.Location = new Point(240, 20);
            panelInfo.Size = new Size(540, 300);
            panelPrincipal.Controls.Add(panelInfo);

            // Título
            Label lblTitulo = new Label();
            lblTitulo.Text = _pelicula.Titulo;
            lblTitulo.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point(0, 0);
            panelInfo.Controls.Add(lblTitulo);

            // Año
            Label lblAño = new Label();
            lblAño.Text = $"Año: {_pelicula.Año}";
            lblAño.Font = new Font("Segoe UI", 10);
            lblAño.AutoSize = true;
            lblAño.Location = new Point(0, 40);
            panelInfo.Controls.Add(lblAño);

            // Director
            Label lblDirector = new Label();
            lblDirector.Text = $"Director: {_pelicula.Director}";
            lblDirector.Font = new Font("Segoe UI", 10);
            lblDirector.AutoSize = true;
            lblDirector.Location = new Point(0, 70);
            panelInfo.Controls.Add(lblDirector);

            // Duración
            Label lblDuracion = new Label();
            lblDuracion.Text = $"Duración: {_pelicula.Duracion} minutos";
            lblDuracion.Font = new Font("Segoe UI", 10);
            lblDuracion.AutoSize = true;
            lblDuracion.Location = new Point(0, 100);
            panelInfo.Controls.Add(lblDuracion);

            // Géneros
            Label lblGeneros = new Label();
            lblGeneros.Text = $"Géneros: {string.Join(", ", _pelicula.Generos)}";
            lblGeneros.Font = new Font("Segoe UI", 10);
            lblGeneros.AutoSize = true;
            lblGeneros.Location = new Point(0, 130);
            panelInfo.Controls.Add(lblGeneros);

            // Plataforma
            Label lblPlataforma = new Label();
            lblPlataforma.Text = $"Plataforma: {_pelicula.Plataforma}";
            lblPlataforma.Font = new Font("Segoe UI", 10);
            lblPlataforma.AutoSize = true;
            lblPlataforma.Location = new Point(0, 160);
            panelInfo.Controls.Add(lblPlataforma);

            // Calificación
            Label lblCalificacion = new Label();
            lblCalificacion.Text = $"Calificación: ★ {_pelicula.CalificacionPromedio:F1} ({_pelicula.NumeroCalificaciones} calificaciones)";
            lblCalificacion.Font = new Font("Segoe UI", 10);
            lblCalificacion.AutoSize = true;
            lblCalificacion.Location = new Point(0, 190);
            panelInfo.Controls.Add(lblCalificacion);

            // Descripción
            Label lblDescripcionTitulo = new Label();
            lblDescripcionTitulo.Text = "Descripción:";
            lblDescripcionTitulo.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblDescripcionTitulo.AutoSize = true;
            lblDescripcionTitulo.Location = new Point(0, 220);
            panelInfo.Controls.Add(lblDescripcionTitulo);

            TextBox txtDescripcion = new TextBox();
            txtDescripcion.Text = _pelicula.Descripcion;
            txtDescripcion.Multiline = true;
            txtDescripcion.ReadOnly = true;
            txtDescripcion.ScrollBars = ScrollBars.Vertical;
            txtDescripcion.Location = new Point(0, 250);
            txtDescripcion.Size = new Size(540, 50);
            panelInfo.Controls.Add(txtDescripcion);

            // Panel de acciones
            Panel panelAcciones = new Panel();
            panelAcciones.BorderStyle = BorderStyle.FixedSingle;
            panelAcciones.Location = new Point(20, 340);
            panelAcciones.Size = new Size(760, 140);
            panelPrincipal.Controls.Add(panelAcciones);

            // Título del panel de acciones
            Label lblAcciones = new Label();
            lblAcciones.Text = "Acciones";
            lblAcciones.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblAcciones.AutoSize = true;
            lblAcciones.Location = new Point(10, 10);
            panelAcciones.Controls.Add(lblAcciones);

            // Botón de marcar como visto
            Button btnMarcarVisto = new Button();
            btnMarcarVisto.Text = "Marcar como visto";
            btnMarcarVisto.Location = new Point(10, 50);
            btnMarcarVisto.Size = new Size(150, 30);
            btnMarcarVisto.Click += (sender, e) =>
            {
                // Registrar visualización
                var historial = new HistorialVisualizacion
                {
                    UsuarioId = _usuarioActual.Id,
                    ContenidoId = _pelicula.Id,
                    TipoContenido = "Pelicula",
                    Completado = true,
                    ProgresoMinutos = _pelicula.Duracion
                };

                _historialController.RegistrarVisualizacion(historial);
                MessageBox.Show("Película marcada como vista.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };
            panelAcciones.Controls.Add(btnMarcarVisto);

            // Calificar película
            Label lblCalificar = new Label();
            lblCalificar.Text = "Calificar:";
            lblCalificar.AutoSize = true;
            lblCalificar.Location = new Point(180, 55);
            panelAcciones.Controls.Add(lblCalificar);

            ComboBox cmbCalificacion = new ComboBox();
            cmbCalificacion.Location = new Point(240, 52);
            cmbCalificacion.Size = new Size(60, 23);
            cmbCalificacion.DropDownStyle = ComboBoxStyle.DropDownList;
            for (int i = 1; i <= 5; i++)
            {
                cmbCalificacion.Items.Add(i);
            }
            cmbCalificacion.SelectedIndex = 4; // 5 estrellas por defecto
            panelAcciones.Controls.Add(cmbCalificacion);

            Button btnCalificar = new Button();
            btnCalificar.Text = "Enviar calificación";
            btnCalificar.Location = new Point(310, 50);
            btnCalificar.Size = new Size(150, 30);
            btnCalificar.Click += (sender, e) =>
            {
                int calificacion = (int)cmbCalificacion.SelectedItem;

                // Registrar calificación
                _usuarioController.CalificarContenido(_usuarioActual.Id, _pelicula.Id, calificacion);

                // Actualizar calificación promedio
                var contenidoController = new ContenidoController();
                contenidoController.ActualizarCalificacionPromedio(_pelicula.Id, calificacion, "Pelicula");

                MessageBox.Show("Calificación enviada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };
            panelAcciones.Controls.Add(btnCalificar);

            // Botón de comentarios
            Button btnComentarios = new Button();
            btnComentarios.Text = "Ver comentarios";
            btnComentarios.Location = new Point(10, 90);
            btnComentarios.Size = new Size(150, 30);
            btnComentarios.Click += (sender, e) =>
            {
                Form comentariosForm = new ComentariosForm(_pelicula.Id, "Pelicula", _usuarioActual, _pelicula.Titulo);
                comentariosForm.ShowDialog();
            };
            panelAcciones.Controls.Add(btnComentarios);

            // Botón de cerrar
            Button btnCerrar = new Button();
            btnCerrar.Text = "Cerrar";
            btnCerrar.Location = new Point(650, 90);
            btnCerrar.Size = new Size(100, 30);
            btnCerrar.Click += (sender, e) => this.Close();
            panelAcciones.Controls.Add(btnCerrar);

            // Botón para agregar a la lista de reportes
            Button btnReportes = new Button();
            btnReportes.Text = "Generar Reportes";
            btnReportes.Location = new Point(310, 90);
            btnReportes.Size = new Size(150, 30);
            btnReportes.Visible = _usuarioActual.Rol == "Administrador"; // Solo visible para administradores
            btnReportes.Click += (sender, e) =>
            {
                Form reportesForm = new ReportesForm();
                reportesForm.ShowDialog();
            };
            panelAcciones.Controls.Add(btnReportes);
        }

        private void DetallePeliculaForm_Load(object sender, EventArgs e)
        {
            // Centrar el formulario en la pantalla
            this.CenterToScreen();
        }
    }
}
