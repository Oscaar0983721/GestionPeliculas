using System;
using System.Drawing;
using System.Windows.Forms;
using GestionPeliculas.Controllers;
using GestionPeliculas.Models;

namespace GestionPeliculas.Views
{
    public partial class DetalleSerieForm : Form
    {
        private readonly Serie _serie;
        private readonly Usuario _usuarioActual;
        private readonly HistorialController _historialController;
        private readonly UsuarioController _usuarioController;
        private readonly ComentarioController _comentarioController;

        public DetalleSerieForm(Serie serie, Usuario usuario)
        {
            _serie = serie;
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
            // DetalleSerieForm
            // 
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Name = "DetalleSerieForm";
            this.Text = $"Detalle de Serie - {_serie.Titulo}";
            this.Load += new System.EventHandler(this.DetalleSerieForm_Load);
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
            lblTitulo.Text = _serie.Titulo;
            lblTitulo.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point(0, 0);
            panelInfo.Controls.Add(lblTitulo);

            // Año
            Label lblAño = new Label();
            lblAño.Text = $"Año: {_serie.Año}";
            lblAño.Font = new Font("Segoe UI", 10);
            lblAño.AutoSize = true;
            lblAño.Location = new Point(0, 40);
            panelInfo.Controls.Add(lblAño);

            // Creador
            Label lblCreador = new Label();
            lblCreador.Text = $"Creador: {_serie.Creador}";
            lblCreador.Font = new Font("Segoe UI", 10);
            lblCreador.AutoSize = true;
            lblCreador.Location = new Point(0, 70);
            panelInfo.Controls.Add(lblCreador);

            // Temporadas
            Label lblTemporadas = new Label();
            lblTemporadas.Text = $"Temporadas: {_serie.NumeroTemporadas}";
            lblTemporadas.Font = new Font("Segoe UI", 10);
            lblTemporadas.AutoSize = true;
            lblTemporadas.Location = new Point(0, 100);
            panelInfo.Controls.Add(lblTemporadas);

            // Episodios
            Label lblEpisodios = new Label();
            lblEpisodios.Text = $"Episodios: {_serie.NumeroEpisodio}";
            lblEpisodios.Font = new Font("Segoe UI", 10);
            lblEpisodios.AutoSize = true;
            lblEpisodios.Location = new Point(0, 130);
            panelInfo.Controls.Add(lblEpisodios);

            // Géneros
            Label lblGeneros = new Label();
            lblGeneros.Text = $"Géneros: {string.Join(", ", _serie.Generos)}";
            lblGeneros.Font = new Font("Segoe UI", 10);
            lblGeneros.AutoSize = true;
            lblGeneros.Location = new Point(0, 160);
            panelInfo.Controls.Add(lblGeneros);

            // Plataforma
            Label lblPlataforma = new Label();
            lblPlataforma.Text = $"Plataforma: {_serie.Plataforma}";
            lblPlataforma.Font = new Font("Segoe UI", 10);
            lblPlataforma.AutoSize = true;
            lblPlataforma.Location = new Point(0, 190);
            panelInfo.Controls.Add(lblPlataforma);

            // Calificación
            Label lblCalificacion = new Label();
            lblCalificacion.Text = $"Calificación: ★ {_serie.CalificacionPromedio:F1} ({_serie.NumeroCalificaciones} calificaciones)";
            lblCalificacion.Font = new Font("Segoe UI", 10);
            lblCalificacion.AutoSize = true;
            lblCalificacion.Location = new Point(0, 220);
            panelInfo.Controls.Add(lblCalificacion);

            // Descripción
            Label lblDescripcionTitulo = new Label();
            lblDescripcionTitulo.Text = "Descripción:";
            lblDescripcionTitulo.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblDescripcionTitulo.AutoSize = true;
            lblDescripcionTitulo.Location = new Point(0, 250);
            panelInfo.Controls.Add(lblDescripcionTitulo);

            TextBox txtDescripcion = new TextBox();
            txtDescripcion.Text = _serie.Descripcion;
            txtDescripcion.Multiline = true;
            txtDescripcion.ReadOnly = true;
            txtDescripcion.ScrollBars = ScrollBars.Vertical;
            txtDescripcion.Location = new Point(0, 280);
            txtDescripcion.Size = new Size(540, 50);
            panelInfo.Controls.Add(txtDescripcion);

            // Panel de temporadas y episodios
            Panel panelTemporadas = new Panel();
            panelTemporadas.BorderStyle = BorderStyle.FixedSingle;
            panelTemporadas.Location = new Point(20, 340);
            panelTemporadas.Size = new Size(760, 150);
            panelTemporadas.AutoScroll = true;
            panelPrincipal.Controls.Add(panelTemporadas);

            // Título del panel de temporadas
            Label lblTemporadasTitulo = new Label();
            lblTemporadasTitulo.Text = "Temporadas y Episodios";
            lblTemporadasTitulo.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblTemporadasTitulo.AutoSize = true;
            lblTemporadasTitulo.Location = new Point(10, 10);
            panelTemporadas.Controls.Add(lblTemporadasTitulo);

            // Simulación de temporadas y episodios
            int yPos = 40;
            for (int i = 1; i <= _serie.NumeroTemporadas; i++)
            {
                Label lblTemporada = new Label();
                lblTemporada.Text = $"Temporada {i}";
                lblTemporada.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                lblTemporada.AutoSize = true;
                lblTemporada.Location = new Point(10, yPos);
                panelTemporadas.Controls.Add(lblTemporada);

                // Simulación de episodios por temporada (5 episodios por temporada)
                int episodiosPorTemporada = _serie.NumeroEpisodio / _serie.NumeroTemporadas;
                for (int j = 1; j <= episodiosPorTemporada; j++)
                {
                    Button btnEpisodio = new Button();
                    btnEpisodio.Text = $"Episodio {j}";
                    btnEpisodio.Size = new Size(100, 25);
                    btnEpisodio.Location = new Point(120 + (j - 1) * 110, yPos);
                    btnEpisodio.Tag = new { Temporada = i, Episodio = j };
                    btnEpisodio.Click += (sender, e) =>
                    {
                        dynamic tag = ((Button)sender).Tag;
                        int temporada = tag.Temporada;
                        int episodio = tag.Episodio;

                        // Registrar visualización de episodio
                        var historial = new HistorialVisualizacion
                        {
                            UsuarioId = _usuarioActual.Id,
                            ContenidoId = _serie.Id,
                            TipoContenido = "Serie",
                            EpisodioId = episodio + (temporada - 1) * episodiosPorTemporada,
                            Completado = true,
                            ProgresoMinutos = 45 // Duración promedio de un episodio
                        };

                        _historialController.RegistrarVisualizacion(historial);
                        MessageBox.Show($"Has visto el Episodio {episodio} de la Temporada {temporada}.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    };
                    panelTemporadas.Controls.Add(btnEpisodio);
                }

                yPos += 35;
            }

            // Panel de acciones
            Panel panelAcciones = new Panel();
            panelAcciones.BorderStyle = BorderStyle.FixedSingle;
            panelAcciones.Location = new Point(20, 500);
            panelAcciones.Size = new Size(760, 80);
            panelPrincipal.Controls.Add(panelAcciones);

            // Título del panel de acciones
            Label lblAcciones = new Label();
            lblAcciones.Text = "Acciones";
            lblAcciones.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblAcciones.AutoSize = true;
            lblAcciones.Location = new Point(10, 10);
            panelAcciones.Controls.Add(lblAcciones);

            // Calificar serie
            Label lblCalificar = new Label();
            lblCalificar.Text = "Calificar:";
            lblCalificar.AutoSize = true;
            lblCalificar.Location = new Point(10, 45);
            panelAcciones.Controls.Add(lblCalificar);

            ComboBox cmbCalificacion = new ComboBox();
            cmbCalificacion.Location = new Point(70, 42);
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
            btnCalificar.Location = new Point(140, 40);
            btnCalificar.Size = new Size(150, 30);
            btnCalificar.Click += (sender, e) =>
            {
                int calificacion = (int)cmbCalificacion.SelectedItem;

                // Registrar calificación
                _usuarioController.CalificarContenido(_usuarioActual.Id, _serie.Id, calificacion);

                // Actualizar calificación promedio
                var contenidoController = new ContenidoController();
                contenidoController.ActualizarCalificacionPromedio(_serie.Id, calificacion, "Serie");

                MessageBox.Show("Calificación enviada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };
            panelAcciones.Controls.Add(btnCalificar);

            // Botón de comentarios
            Button btnComentarios = new Button();
            btnComentarios.Text = "Ver comentarios";
            btnComentarios.Location = new Point(300, 40);
            btnComentarios.Size = new Size(150, 30);
            btnComentarios.Click += (sender, e) =>
            {
                Form comentariosForm = new ComentariosForm(_serie.Id, "Serie", _usuarioActual, _serie.Titulo);
                comentariosForm.ShowDialog();
            };
            panelAcciones.Controls.Add(btnComentarios);

            // Botón de cerrar
            Button btnCerrar = new Button();
            btnCerrar.Text = "Cerrar";
            btnCerrar.Location = new Point(650, 40);
            btnCerrar.Size = new Size(100, 30);
            btnCerrar.Click += (sender, e) => this.Close();
            panelAcciones.Controls.Add(btnCerrar);

            // Botón para generar reportes (solo para administradores)
            Button btnReportes = new Button();
            btnReportes.Text = "Generar Reportes";
            btnReportes.Location = new Point(460, 40);
            btnReportes.Size = new Size(150, 30);
            btnReportes.Visible = _usuarioActual.Rol == "Administrador";
            btnReportes.Click += (sender, e) =>
            {
                Form reportesForm = new ReportesForm();
                reportesForm.ShowDialog();
            };
            panelAcciones.Controls.Add(btnReportes);
        }

        private void DetalleSerieForm_Load(object sender, EventArgs e)
        {
            // Centrar el formulario en la pantalla
            this.CenterToScreen();
        }
    }
}
