using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using GestionPeliculas.Controllers;
using GestionPeliculas.Models;

namespace GestionPeliculas.Views
{
    public partial class GestionContenidoForm : Form
    {
        private readonly ContenidoController _contenidoController;
        private List<Pelicula> _peliculas;
        private List<Serie> _series;
        private string _tipoContenidoActual = "Pelicula"; // Por defecto

        public GestionContenidoForm()
        {
            _contenidoController = new ContenidoController();
            _peliculas = _contenidoController.ObtenerTodasPeliculas();
            _series = _contenidoController.ObtenerTodasSeries();

            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // GestionContenidoForm
            // 
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.Name = "GestionContenidoForm";
            this.Text = "Gestión de Contenido - Sistema de Gestión de Series y Películas";
            this.Load += new System.EventHandler(this.GestionContenidoForm_Load);
            this.ResumeLayout(false);

            // Panel principal
            Panel panelPrincipal = new Panel();
            panelPrincipal.Dock = DockStyle.Fill;
            this.Controls.Add(panelPrincipal);

            // Título
            Label lblTitulo = new Label();
            lblTitulo.Text = "Gestión de Contenido";
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

            // Tipo de contenido
            Label lblTipoContenido = new Label();
            lblTipoContenido.Text = "Tipo de contenido:";
            lblTipoContenido.AutoSize = true;
            lblTipoContenido.Location = new Point(10, 20);
            panelFiltros.Controls.Add(lblTipoContenido);

            ComboBox cmbTipoContenido = new ComboBox();
            cmbTipoContenido.Location = new Point(130, 17);
            cmbTipoContenido.Size = new Size(150, 23);
            cmbTipoContenido.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTipoContenido.Items.Add("Película");
            cmbTipoContenido.Items.Add("Serie");
            cmbTipoContenido.SelectedIndex = 0;
            cmbTipoContenido.SelectedIndexChanged += (sender, e) =>
            {
                _tipoContenidoActual = cmbTipoContenido.SelectedIndex == 0 ? "Pelicula" : "Serie";
                MostrarContenido();
            };
            panelFiltros.Controls.Add(cmbTipoContenido);

            // Texto de búsqueda
            Label lblBuscar = new Label();
            lblBuscar.Text = "Buscar:";
            lblBuscar.AutoSize = true;
            lblBuscar.Location = new Point(300, 20);
            panelFiltros.Controls.Add(lblBuscar);

            TextBox txtBuscar = new TextBox();
            txtBuscar.Location = new Point(350, 17);
            txtBuscar.Size = new Size(200, 23);
            panelFiltros.Controls.Add(txtBuscar);

            // Botón de buscar
            Button btnBuscar = new Button();
            btnBuscar.Text = "Buscar";
            btnBuscar.Location = new Point(560, 16);
            btnBuscar.Size = new Size(80, 25);
            btnBuscar.Click += (sender, e) =>
            {
                string textoBusqueda = txtBuscar.Text.Trim().ToLower();

                if (_tipoContenidoActual == "Pelicula")
                {
                    if (string.IsNullOrEmpty(textoBusqueda))
                    {
                        _peliculas = _contenidoController.ObtenerTodasPeliculas();
                    }
                    else
                    {
                        _peliculas = _contenidoController.ObtenerTodasPeliculas().FindAll(p =>
                            p.Titulo.ToLower().Contains(textoBusqueda));
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(textoBusqueda))
                    {
                        _series = _contenidoController.ObtenerTodasSeries();
                    }
                    else
                    {
                        _series = _contenidoController.ObtenerTodasSeries().FindAll(s =>
                            s.Titulo.ToLower().Contains(textoBusqueda));
                    }
                }

                MostrarContenido();
            };
            panelFiltros.Controls.Add(btnBuscar);

            // Botón de limpiar
            Button btnLimpiar = new Button();
            btnLimpiar.Text = "Limpiar";
            btnLimpiar.Location = new Point(650, 16);
            btnLimpiar.Size = new Size(80, 25);
            btnLimpiar.Click += (sender, e) =>
            {
                txtBuscar.Text = "";

                if (_tipoContenidoActual == "Pelicula")
                {
                    _peliculas = _contenidoController.ObtenerTodasPeliculas();
                }
                else
                {
                    _series = _contenidoController.ObtenerTodasSeries();
                }

                MostrarContenido();
            };
            panelFiltros.Controls.Add(btnLimpiar);

            // Botón de agregar contenido
            Button btnAgregar = new Button();
            btnAgregar.Text = "Agregar Contenido";
            btnAgregar.Location = new Point(820, 16);
            btnAgregar.Size = new Size(130, 25);
            btnAgregar.Click += (sender, e) =>
            {
                if (_tipoContenidoActual == "Pelicula")
                {
                    AgregarEditarPelicula(null);
                }
                else
                {
                    AgregarEditarSerie(null);
                }
            };
            panelFiltros.Controls.Add(btnAgregar);

            // Panel de contenido
            Panel panelContenido = new Panel();
            panelContenido.BorderStyle = BorderStyle.FixedSingle;
            panelContenido.Location = new Point(20, 130);
            panelContenido.Size = new Size(960, 450);
            panelContenido.AutoScroll = true;
            panelContenido.Tag = "panelContenido"; // Para identificarlo al actualizar
            panelPrincipal.Controls.Add(panelContenido);

            // ListView para mostrar el contenido
            ListView lvContenido = new ListView();
            lvContenido.View = View.Details;
            lvContenido.FullRowSelect = true;
            lvContenido.GridLines = true;
            lvContenido.Location = new Point(10, 10);
            lvContenido.Size = new Size(940, 430);
            lvContenido.Tag = "lvContenido"; // Para identificarlo al actualizar

            // Columnas para películas
            lvContenido.Columns.Add("ID", 50);
            lvContenido.Columns.Add("Título", 200);
            lvContenido.Columns.Add("Año", 70);
            lvContenido.Columns.Add("Géneros", 200);
            lvContenido.Columns.Add("Plataforma", 150);
            lvContenido.Columns.Add("Calificación", 100);
            lvContenido.Columns.Add("Duración/Temporadas", 150);

            panelContenido.Controls.Add(lvContenido);

            // Menú contextual para editar/eliminar contenido
            ContextMenuStrip menuContextual = new ContextMenuStrip();

            ToolStripMenuItem menuEditar = new ToolStripMenuItem("Editar contenido");
            menuEditar.Click += (sender, e) =>
            {
                if (lvContenido.SelectedItems.Count > 0)
                {
                    int id = int.Parse(lvContenido.SelectedItems[0].SubItems[0].Text);

                    if (_tipoContenidoActual == "Pelicula")
                    {
                        var pelicula = _contenidoController.ObtenerPeliculaPorId(id);
                        if (pelicula != null)
                        {
                            AgregarEditarPelicula(pelicula);
                        }
                    }
                    else
                    {
                        var serie = _contenidoController.ObtenerSeriePorId(id);
                        if (serie != null)
                        {
                            AgregarEditarSerie(serie);
                        }
                    }
                }
            };
            menuContextual.Items.Add(menuEditar);

            ToolStripMenuItem menuEliminar = new ToolStripMenuItem("Eliminar contenido");
            menuEliminar.Click += (sender, e) =>
            {
                if (lvContenido.SelectedItems.Count > 0)
                {
                    int id = int.Parse(lvContenido.SelectedItems[0].SubItems[0].Text);
                    EliminarContenido(id);
                }
            };
            menuContextual.Items.Add(menuEliminar);

            lvContenido.ContextMenuStrip = menuContextual;

            // Mostrar contenido
            MostrarContenido();
        }

        private void GestionContenidoForm_Load(object sender, EventArgs e)
        {
            // Centrar el formulario en la pantalla
            this.CenterToScreen();
        }

        private void MostrarContenido()
        {
            // Obtener el ListView
            ListView lvContenido = null;
            foreach (Control control in this.Controls[0].Controls)
            {
                if (control is Panel && control.Tag?.ToString() == "panelContenido")
                {
                    foreach (Control subControl in control.Controls)
                    {
                        if (subControl is ListView && subControl.Tag?.ToString() == "lvContenido")
                        {
                            lvContenido = (ListView)subControl;
                            break;
                        }
                    }
                    break;
                }
            }

            if (lvContenido == null)
            {
                return;
            }

            // Limpiar el ListView
            lvContenido.Items.Clear();

            // Mostrar contenido según el tipo seleccionado
            if (_tipoContenidoActual == "Pelicula")
            {
                foreach (var pelicula in _peliculas)
                {
                    ListViewItem item = new ListViewItem(pelicula.Id.ToString());
                    item.SubItems.Add(pelicula.Titulo);
                    item.SubItems.Add(pelicula.Año.ToString());
                    item.SubItems.Add(string.Join(", ", pelicula.Generos));
                    item.SubItems.Add(pelicula.Plataforma);
                    item.SubItems.Add(pelicula.CalificacionPromedio.ToString("F1"));
                    item.SubItems.Add($"{pelicula.Duracion} min");
                    item.Tag = pelicula;

                    lvContenido.Items.Add(item);
                }
            }
            else
            {
                foreach (var serie in _series)
                {
                    ListViewItem item = new ListViewItem(serie.Id.ToString());
                    item.SubItems.Add(serie.Titulo);
                    item.SubItems.Add(serie.Año.ToString());
                    item.SubItems.Add(string.Join(", ", serie.Generos));
                    item.SubItems.Add(serie.Plataforma);
                    item.SubItems.Add(serie.CalificacionPromedio.ToString("F1"));
                    item.SubItems.Add($"{serie.NumeroTemporadas} temporadas");
                    item.Tag = serie;

                    lvContenido.Items.Add(item);
                }
            }
        }

        private void AgregarEditarPelicula(Pelicula pelicula)
        {
            bool esEdicion = pelicula != null;

            // Crear formulario
            Form formPelicula = new Form();
            formPelicula.Text = esEdicion ? "Editar Película" : "Agregar Película";
            formPelicula.Size = new Size(500, 500);
            formPelicula.StartPosition = FormStartPosition.CenterParent;
            formPelicula.FormBorderStyle = FormBorderStyle.FixedDialog;
            formPelicula.MaximizeBox = false;
            formPelicula.MinimizeBox = false;

            // Controles del formulario
            Label lblTitulo = new Label();
            lblTitulo.Text = "Título:";
            lblTitulo.Location = new Point(30, 30);
            lblTitulo.AutoSize = true;
            formPelicula.Controls.Add(lblTitulo);

            TextBox txtTitulo = new TextBox();
            txtTitulo.Location = new Point(150, 27);
            txtTitulo.Size = new Size(300, 23);
            if (esEdicion) txtTitulo.Text = pelicula.Titulo;
            formPelicula.Controls.Add(txtTitulo);

            Label lblDescripcion = new Label();
            lblDescripcion.Text = "Descripción:";
            lblDescripcion.Location = new Point(30, 70);
            lblDescripcion.AutoSize = true;
            formPelicula.Controls.Add(lblDescripcion);

            TextBox txtDescripcion = new TextBox();
            txtDescripcion.Multiline = true;
            txtDescripcion.ScrollBars = ScrollBars.Vertical;
            txtDescripcion.Location = new Point(150, 67);
            txtDescripcion.Size = new Size(300, 60);
            if (esEdicion) txtDescripcion.Text = pelicula.Descripcion;
            formPelicula.Controls.Add(txtDescripcion);

            Label lblAño = new Label();
            lblAño.Text = "Año:";
            lblAño.Location = new Point(30, 150);
            lblAño.AutoSize = true;
            formPelicula.Controls.Add(lblAño);

            NumericUpDown numAño = new NumericUpDown();
            numAño.Minimum = 1900;
            numAño.Maximum = DateTime.Now.Year;
            numAño.Value = esEdicion ? pelicula.Año : DateTime.Now.Year;
            numAño.Location = new Point(150, 147);
            numAño.Size = new Size(100, 23);
            formPelicula.Controls.Add(numAño);

            Label lblGeneros = new Label();
            lblGeneros.Text = "Géneros:";
            lblGeneros.Location = new Point(30, 190);
            lblGeneros.AutoSize = true;
            formPelicula.Controls.Add(lblGeneros);

            TextBox txtGeneros = new TextBox();
            txtGeneros.Location = new Point(150, 187);
            txtGeneros.Size = new Size(300, 23);
            if (esEdicion) txtGeneros.Text = string.Join(", ", pelicula.Generos);
            formPelicula.Controls.Add(txtGeneros);

            Label lblPlataforma = new Label();
            lblPlataforma.Text = "Plataforma:";
            lblPlataforma.Location = new Point(30, 230);
            lblPlataforma.AutoSize = true;
            formPelicula.Controls.Add(lblPlataforma);

            ComboBox cmbPlataforma = new ComboBox();
            cmbPlataforma.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPlataforma.Items.Add("Netflix");
            cmbPlataforma.Items.Add("Amazon Prime");
            cmbPlataforma.Items.Add("Disney+");
            cmbPlataforma.Items.Add("HBO Max");
            cmbPlataforma.Items.Add("Apple TV+");
            cmbPlataforma.Items.Add("Hulu");
            cmbPlataforma.Items.Add("Paramount+");
            if (esEdicion) cmbPlataforma.Text = pelicula.Plataforma;
            else cmbPlataforma.SelectedIndex = 0;
            cmbPlataforma.Location = new Point(150, 227);
            cmbPlataforma.Size = new Size(200, 23);
            formPelicula.Controls.Add(cmbPlataforma);

            Label lblDuracion = new Label();
            lblDuracion.Text = "Duración (min):";
            lblDuracion.Location = new Point(30, 270);
            lblDuracion.AutoSize = true;
            formPelicula.Controls.Add(lblDuracion);

            NumericUpDown numDuracion = new NumericUpDown();
            numDuracion.Minimum = 1;
            numDuracion.Maximum = 300;
            numDuracion.Value = esEdicion ? pelicula.Duracion : 90;
            numDuracion.Location = new Point(150, 267);
            numDuracion.Size = new Size(100, 23);
            formPelicula.Controls.Add(numDuracion);

            Label lblDirector = new Label();
            lblDirector.Text = "Director:";
            lblDirector.Location = new Point(30, 310);
            lblDirector.AutoSize = true;
            formPelicula.Controls.Add(lblDirector);

            TextBox txtDirector = new TextBox();
            txtDirector.Location = new Point(150, 307);
            txtDirector.Size = new Size(300, 23);
            if (esEdicion) txtDirector.Text = pelicula.Director;
            formPelicula.Controls.Add(txtDirector);

            Button btnGuardar = new Button();
            btnGuardar.Text = "Guardar";
            btnGuardar.Location = new Point(150, 370);
            btnGuardar.Size = new Size(100, 30);
            btnGuardar.Click += (sender, args) =>
            {
                // Validar datos
                if (string.IsNullOrEmpty(txtTitulo.Text) || string.IsNullOrEmpty(txtDescripcion.Text) ||
                    string.IsNullOrEmpty(txtGeneros.Text) || string.IsNullOrEmpty(txtDirector.Text))
                {
                    MessageBox.Show("Por favor, complete todos los campos obligatorios.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Crear o actualizar película
                Pelicula nuevaPelicula = esEdicion ? pelicula : new Pelicula();
                nuevaPelicula.Titulo = txtTitulo.Text;
                nuevaPelicula.Descripcion = txtDescripcion.Text;
                nuevaPelicula.Año = (int)numAño.Value;
                nuevaPelicula.Generos = new List<string>(txtGeneros.Text.Split(',').Select(g => g.Trim()));
                nuevaPelicula.Plataforma = cmbPlataforma.Text;
                nuevaPelicula.Duracion = (int)numDuracion.Value;
                nuevaPelicula.Director = txtDirector.Text;

                bool resultado;
                if (esEdicion)
                {
                    resultado = _contenidoController.ActualizarPelicula(nuevaPelicula);
                }
                else
                {
                    nuevaPelicula.CalificacionPromedio = 0;
                    nuevaPelicula.NumeroCalificaciones = 0;
                    nuevaPelicula.FechaAgregado = DateTime.Now;
                    resultado = _contenidoController.AgregarPelicula(nuevaPelicula);
                }

                if (resultado)
                {
                    MessageBox.Show("Película guardada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    formPelicula.Close();

                    // Actualizar lista de películas
                    _peliculas = _contenidoController.ObtenerTodasPeliculas();
                    MostrarContenido();
                }
                else
                {
                    MessageBox.Show("Error al guardar la película.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            formPelicula.Controls.Add(btnGuardar);

            Button btnCancelar = new Button();
            btnCancelar.Text = "Cancelar";
            btnCancelar.Location = new Point(260, 370);
            btnCancelar.Size = new Size(90, 30);
            btnCancelar.Click += (sender, args) => formPelicula.Close();
            formPelicula.Controls.Add(btnCancelar);

            formPelicula.ShowDialog();
        }

        private void AgregarEditarSerie(Serie serie)
        {
            bool esEdicion = serie != null;

            // Crear formulario
            Form formSerie = new Form();
            formSerie.Text = esEdicion ? "Editar Serie" : "Agregar Serie";
            formSerie.Size = new Size(600, 600);
            formSerie.StartPosition = FormStartPosition.CenterParent;
            formSerie.FormBorderStyle = FormBorderStyle.FixedDialog;
            formSerie.MaximizeBox = false;
            formSerie.MinimizeBox = false;

            // Controles del formulario
            Label lblTitulo = new Label();
            lblTitulo.Text = "Título:";
            lblTitulo.Location = new Point(30, 30);
            lblTitulo.AutoSize = true;
            formSerie.Controls.Add(lblTitulo);

            TextBox txtTitulo = new TextBox();
            txtTitulo.Location = new Point(150, 27);
            txtTitulo.Size = new Size(400, 23);
            if (esEdicion) txtTitulo.Text = serie.Titulo;
            formSerie.Controls.Add(txtTitulo);

            Label lblDescripcion = new Label();
            lblDescripcion.Text = "Descripción:";
            lblDescripcion.Location = new Point(30, 70);
            lblDescripcion.AutoSize = true;
            formSerie.Controls.Add(lblDescripcion);

            TextBox txtDescripcion = new TextBox();
            txtDescripcion.Multiline = true;
            txtDescripcion.ScrollBars = ScrollBars.Vertical;
            txtDescripcion.Location = new Point(150, 67);
            txtDescripcion.Size = new Size(400, 60);
            if (esEdicion) txtDescripcion.Text = serie.Descripcion;
            formSerie.Controls.Add(txtDescripcion);

            Label lblAño = new Label();
            lblAño.Text = "Año:";
            lblAño.Location = new Point(30, 150);
            lblAño.AutoSize = true;
            formSerie.Controls.Add(lblAño);

            NumericUpDown numAño = new NumericUpDown();
            numAño.Minimum = 1900;
            numAño.Maximum = DateTime.Now.Year;
            numAño.Value = esEdicion ? serie.Año : DateTime.Now.Year;
            numAño.Location = new Point(150, 147);
            numAño.Size = new Size(100, 23);
            formSerie.Controls.Add(numAño);

            Label lblGeneros = new Label();
            lblGeneros.Text = "Géneros:";
            lblGeneros.Location = new Point(30, 190);
            lblGeneros.AutoSize = true;
            formSerie.Controls.Add(lblGeneros);

            TextBox txtGeneros = new TextBox();
            txtGeneros.Location = new Point(150, 187);
            txtGeneros.Size = new Size(400, 23);
            if (esEdicion) txtGeneros.Text = string.Join(", ", serie.Generos);
            formSerie.Controls.Add(txtGeneros);

            Label lblPlataforma = new Label();
            lblPlataforma.Text = "Plataforma:";
            lblPlataforma.Location = new Point(30, 230);
            lblPlataforma.AutoSize = true;
            formSerie.Controls.Add(lblPlataforma);

            ComboBox cmbPlataforma = new ComboBox();
            cmbPlataforma.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPlataforma.Items.Add("Netflix");
            cmbPlataforma.Items.Add("Amazon Prime");
            cmbPlataforma.Items.Add("Disney+");
            cmbPlataforma.Items.Add("HBO Max");
            cmbPlataforma.Items.Add("Apple TV+");
            cmbPlataforma.Items.Add("Hulu");
            cmbPlataforma.Items.Add("Paramount+");
            if (esEdicion) cmbPlataforma.Text = serie.Plataforma;
            else cmbPlataforma.SelectedIndex = 0;
            cmbPlataforma.Location = new Point(150, 227);
            cmbPlataforma.Size = new Size(200, 23);
            formSerie.Controls.Add(cmbPlataforma);

            Label lblTemporadas = new Label();
            lblTemporadas.Text = "Temporadas:";
            lblTemporadas.Location = new Point(30, 270);
            lblTemporadas.AutoSize = true;
            formSerie.Controls.Add(lblTemporadas);

            NumericUpDown numTemporadas = new NumericUpDown();
            numTemporadas.Minimum = 1;
            numTemporadas.Maximum = 20;
            numTemporadas.Value = esEdicion ? serie.NumeroTemporadas : 1;
            numTemporadas.Location = new Point(150, 267);
            numTemporadas.Size = new Size(100, 23);
            formSerie.Controls.Add(numTemporadas);

            Label lblTemporadasInfo = new Label();
            lblTemporadasInfo.Text = "Nota: Para editar episodios, use el formulario de detalle de serie.";
            lblTemporadasInfo.Font = new Font("Segoe UI", 8, FontStyle.Italic);
            lblTemporadasInfo.Location = new Point(150, 300);
            lblTemporadasInfo.AutoSize = true;
            formSerie.Controls.Add(lblTemporadasInfo);

            Button btnGuardar = new Button();
            btnGuardar.Text = "Guardar";
            btnGuardar.Location = new Point(150, 350);
            btnGuardar.Size = new Size(100, 30);
            btnGuardar.Click += (sender, args) =>
            {
                // Validar datos
                if (string.IsNullOrEmpty(txtTitulo.Text) || string.IsNullOrEmpty(txtDescripcion.Text) ||
                    string.IsNullOrEmpty(txtGeneros.Text))
                {
                    MessageBox.Show("Por favor, complete todos los campos obligatorios.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Crear o actualizar serie
                Serie nuevaSerie = esEdicion ? serie : new Serie();
                nuevaSerie.Titulo = txtTitulo.Text;
                nuevaSerie.Descripcion = txtDescripcion.Text;
                nuevaSerie.Año = (int)numAño.Value;
                nuevaSerie.Generos = new List<string>(txtGeneros.Text.Split(',').Select(g => g.Trim()));
                nuevaSerie.Plataforma = cmbPlataforma.Text;

                int numTemporadasAnterior = esEdicion ? serie.NumeroTemporadas : 0;
                int numTemporadasNuevo = (int)numTemporadas.Value;

                // Si es una nueva serie o se aumentaron las temporadas, crear temporadas y episodios
                if (!esEdicion || numTemporadasNuevo > numTemporadasAnterior)
                {
                    if (!esEdicion)
                    {
                        nuevaSerie.Temporadas = new List<Temporada>();
                    }

                    for (int t = numTemporadasAnterior + 1; t <= numTemporadasNuevo; t++)
                    {
                        var temporada = new Temporada
                        {
                            NumeroTemporada = t,
                            Episodios = new List<Episodio>()
                        };

                        // Crear episodios por defecto (10 por temporada)
                        for (int e = 1; e <= 10; e++)
                        {
                            temporada.Episodios.Add(new Episodio
                            {
                                NumeroEpisodio = e,
                                Titulo = $"Episodio {e}",
                                Duracion = 45,
                                Descripcion = $"Descripción del episodio {e} de la temporada {t}."
                            });
                        }

                        nuevaSerie.Temporadas.Add(temporada);
                    }
                }
                else if (numTemporadasNuevo < numTemporadasAnterior)
                {
                    // Si se redujeron las temporadas, eliminar las sobrantes
                    nuevaSerie.Temporadas.RemoveAll(t => t.NumeroTemporada > numTemporadasNuevo);
                }

                nuevaSerie.NumeroTemporadas = numTemporadasNuevo;
                nuevaSerie.NumeroEpisodiosTotales = nuevaSerie.Temporadas.Sum(t => t.Episodios.Count);

                bool resultado;
                if (esEdicion)
                {
                    resultado = _contenidoController.ActualizarSerie(nuevaSerie);
                }
                else
                {
                    nuevaSerie.CalificacionPromedio = 0;
                    nuevaSerie.NumeroCalificaciones = 0;
                    nuevaSerie.FechaAgregado = DateTime.Now;
                    resultado = _contenidoController.AgregarSerie(nuevaSerie);
                }

                if (resultado)
                {
                    MessageBox.Show("Serie guardada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    formSerie.Close();

                    // Actualizar lista de series
                    _series = _contenidoController.ObtenerTodasSeries();
                    MostrarContenido();
                }
                else
                {
                    MessageBox.Show("Error al guardar la serie.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            formSerie.Controls.Add(btnGuardar);

            Button btnCancelar = new Button();
            btnCancelar.Text = "Cancelar";
            btnCancelar.Location = new Point(260, 350);
            btnCancelar.Size = new Size(90, 30);
            btnCancelar.Click += (sender, args) => formSerie.Close();
            formSerie.Controls.Add(btnCancelar);

            formSerie.ShowDialog();
        }

        private void EliminarContenido(int id)
        {
            bool resultado = false;
            string tipo = "";

            if (_tipoContenidoActual == "Pelicula")
            {
                var pelicula = _contenidoController.ObtenerPeliculaPorId(id);
                if (pelicula != null)
                {
                    tipo = "película";
                    if (MessageBox.Show($"¿Está seguro de eliminar la película '{pelicula.Titulo}'?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        resultado = _contenidoController.EliminarPelicula(id);
                    }
                    else
                    {
                        return;
                    }
                }
            }
            else
            {
                var serie = _contenidoController.ObtenerSeriePorId(id);
                if (serie != null)
                {
                    tipo = "serie";
                    if (MessageBox.Show($"¿Está seguro de eliminar la serie '{serie.Titulo}'?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        resultado = _contenidoController.EliminarSerie(id);
                    }
                    else
                    {
                        return;
                    }
                }
            }

            if (resultado)
            {
                MessageBox.Show($"La {tipo} ha sido eliminada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Actualizar lista de contenido
                if (_tipoContenidoActual == "Pelicula")
                {
                    _peliculas = _contenidoController.ObtenerTodasPeliculas();
                }
                else
                {
                    _series = _contenidoController.ObtenerTodasSeries();
                }

                MostrarContenido();
            }
            else
            {
                MessageBox.Show($"Error al eliminar la {tipo}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
