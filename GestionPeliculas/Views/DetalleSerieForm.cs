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

        public DetalleSerieForm(Serie serie, Usuario usuario)
        {
            _serie = serie;
            _usuarioActual = usuario;
            _historialController = new HistorialController();
            _usuarioController = new UsuarioController();
            
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

            // Temporadas
            Label lblTemporadas = new Label();
            lblTemporadas.Text = $"Temporadas: {_serie.NumeroTemporadas}";
            lblTemporadas.Font = new Font("Segoe UI", 10);
            lblTemporadas.AutoSize = true;
            lblTemporadas.Location = new Point(0, 70);
            panelInfo.Controls.Add(lblTemporadas);

            // Episodios
            Label lblEpisodios = new Label();
            lblEpisodios.Text = $"Episodios totales: {_serie.NumeroEpisodiosTotales}";
            lblEpisodios.Font = new Font("Segoe UI", 10);
            lblEpisodios.AutoSize = true;
            lblEpisodios.Location = new Point(0, 100);
            panelInfo.Controls.Add(lblEpisodios);

            // Géneros
            Label lblGeneros = new Label();
            lblGeneros.Text = $"Géneros: {string.Join(", ", _serie.Generos)}";
            lblGeneros.Font = new Font("Segoe UI", 10);
            lblGeneros.AutoSize = true;
            lblGeneros.Location = new Point(0, 130);
            panelInfo.Controls.Add(lblGeneros);

            // Plataforma
            Label lblPlataforma = new Label();
            lblPlataforma.Text = $"Plataforma: {_serie.Plataforma}";
            lblPlataforma.Font = new Font("Segoe UI", 10);
            lblPlataforma.AutoSize = true;
            lblPlataforma.Location = new Point(0, 160);
            panelInfo.Controls.Add(lblPlataforma);

            // Calificación
            Label lblCalificacion = new Label();
            lblCalificacion.Text = $"Calificación: ★ {_serie.CalificacionPromedio:F1} ({_serie.NumeroCalificaciones} calificaciones)";
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
            txtDescripcion.Text = _serie.Descripcion;
            txtDescripcion.Multiline = true;
            txtDescripcion.ReadOnly = true;
            txtDescripcion.ScrollBars = ScrollBars.Vertical;
            txtDescripcion.Location = new Point(0, 250);
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

            // ComboBox para seleccionar temporada
            Label lblSeleccionarTemporada = new Label();
            lblSeleccionarTemporada.Text = "Seleccionar temporada:";
            lblSeleccionarTemporada.AutoSize = true;
            lblSeleccionarTemporada.Location = new Point(10, 50);
            panelTemporadas.Controls.Add(lblSeleccionarTemporada);

            ComboBox cmbTemporadas = new ComboBox();
            cmbTemporadas.Location = new Point(150, 47);
            cmbTemporadas.Size = new Size(150, 23);
            cmbTemporadas.DropDownStyle = ComboBoxStyle.DropDownList;
            foreach (var temporada in _serie.Temporadas)
            {
                cmbTemporadas.Items.Add($"Temporada {temporada.NumeroTemporada}");
            }
            if (cmbTemporadas.Items.Count > 0)
            {
                cmbTemporadas.SelectedIndex = 0;
            }
            panelTemporadas.Controls.Add(cmbTemporadas);

            // ListBox para mostrar episodios
            ListBox lstEpisodios = new ListBox();
            lstEpisodios.Location = new Point(320, 47);
            lstEpisodios.Size = new Size(430, 90);
            panelTemporadas.Controls.Add(lstEpisodios);

            // Evento de cambio de temporada
            cmbTemporadas.SelectedIndexChanged += (sender, e) =>
            {
                lstEpisodios.Items.Clear();
                
                if (cmbTemporadas.SelectedIndex >= 0)
                {
                    var temporada = _serie.Temporadas[cmbTemporadas.SelectedIndex];
                    foreach (var episodio in temporada.Episodios)
                    {
                        lstEpisodios.Items.Add($"Episodio {episodio.NumeroEpisodio}: {episodio.Titulo} ({episodio.Duracion} min)");
                    }
                }
            };

            // Disparar el evento para cargar los episodios de la primera temporada
            if (cmbTemporadas.Items.Count > 0)
            {
                cmbTemporadas.SelectedIndex = 0;
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

            // Botón de marcar episodio como visto
            Button btnMarcarEpisodio = new Button();
            btnMarcarEpisodio.Text = "Marcar episodio como visto";
            btnMarcarEpisodio.Location = new Point(10, 40);
            btnMarcarEpisodio.Size = new Size(180, 30);
            btnMarcarEpisodio.Click += (sender, e) =>
            {
                if (lstEpisodios.SelectedIndex >= 0)
                {
                    var temporada = _serie.Temporadas[cmbTemporadas.SelectedIndex];
                    var episodio = temporada.Episodios[lstEpisodios.SelectedIndex];
                    
                    // Registrar visualización
                    var historial = new HistorialVisualizacion
                    {
                        UsuarioId = _usuarioActual.Id,
                        ContenidoId = _serie.Id,
                        TipoContenido = "Serie",
                        EpisodioId = episodio.Id,
                        Completado = true,
                        ProgresoMinutos = episodio.Duracion
                    };
                    
                    _historialController.RegistrarVisualizacion(historial);
                    MessageBox.Show("Episodio marcado como visto.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Por favor, seleccione un episodio.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            panelAcciones.Controls.Add(btnMarcarEpisodio);

            // Calificar serie
            Label lblCalificar = new Label();
            lblCalificar.Text = "Calificar:";
            lblCalificar.AutoSize = true;
            lblCalificar.Location = new Point(210, 45);
            panelAcciones.Controls.Add(lblCalificar);

            ComboBox cmbCalificacion = new ComboBox();
            cmbCalificacion.Location = new Point(270, 42);
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
            btnCalificar.Location = new Point(340, 40);
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

            // Botón de cerrar
            Button btnCerrar = new Button();
            btnCerrar.Text = "Cerrar";
            btnCerrar.Location = new Point(650, 40);
            btnCerrar.Size = new Size(100, 30);
            btnCerrar.Click += (sender, e) => this.Close();
            panelAcciones.Controls.Add(btnCerrar);
        }

        private void DetalleSerieForm_Load(object sender, EventArgs e)
        {
            // Centrar el formulario en la pantalla
            this.CenterToScreen();
        }
    }
}
