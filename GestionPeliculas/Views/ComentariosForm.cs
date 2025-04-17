using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GestionPeliculas.Controllers;
using GestionPeliculas.Models;

namespace GestionPeliculas.Views
{
    public partial class ComentariosForm : Form
    {
        private readonly int _contenidoId;
        private readonly string _tipoContenido;
        private readonly Usuario _usuarioActual;
        private readonly string _tituloContenido;
        private readonly ComentarioController _comentarioController;
        private readonly UsuarioController _usuarioController;

        public ComentariosForm(int contenidoId, string tipoContenido, Usuario usuarioActual, string tituloContenido)
        {
            _contenidoId = contenidoId;
            _tipoContenido = tipoContenido;
            _usuarioActual = usuarioActual;
            _tituloContenido = tituloContenido;
            _comentarioController = new ComentarioController();
            _usuarioController = new UsuarioController();

            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ComentariosForm
            // 
            this.ClientSize = new System.Drawing.Size(600, 500);
            this.Name = "ComentariosForm";
            this.Text = $"Comentarios - {_tituloContenido}";
            this.Load += new System.EventHandler(this.ComentariosForm_Load);
            this.ResumeLayout(false);

            // Panel principal
            Panel panelPrincipal = new Panel();
            panelPrincipal.Dock = DockStyle.Fill;
            this.Controls.Add(panelPrincipal);

            // Título
            Label lblTitulo = new Label();
            lblTitulo.Text = $"Comentarios para {_tituloContenido}";
            lblTitulo.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point(20, 20);
            panelPrincipal.Controls.Add(lblTitulo);

            // Panel para agregar comentario
            Panel panelAgregarComentario = new Panel();
            panelAgregarComentario.BorderStyle = BorderStyle.FixedSingle;
            panelAgregarComentario.Location = new Point(20, 60);
            panelAgregarComentario.Size = new Size(560, 120);
            panelPrincipal.Controls.Add(panelAgregarComentario);

            // Título del panel
            Label lblAgregarComentario = new Label();
            lblAgregarComentario.Text = "Agregar comentario";
            lblAgregarComentario.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblAgregarComentario.AutoSize = true;
            lblAgregarComentario.Location = new Point(10, 10);
            panelAgregarComentario.Controls.Add(lblAgregarComentario);

            // Campo de texto para el comentario
            TextBox txtComentario = new TextBox();
            txtComentario.Multiline = true;
            txtComentario.ScrollBars = ScrollBars.Vertical;
            txtComentario.Location = new Point(10, 40);
            txtComentario.Size = new Size(430, 60);
            panelAgregarComentario.Controls.Add(txtComentario);

            // Botón para enviar comentario
            Button btnEnviarComentario = new Button();
            btnEnviarComentario.Text = "Enviar";
            btnEnviarComentario.Location = new Point(450, 40);
            btnEnviarComentario.Size = new Size(100, 30);
            btnEnviarComentario.Click += (sender, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtComentario.Text))
                {
                    MessageBox.Show("El comentario no puede estar vacío.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var comentario = new Comentario
                {
                    UsuarioId = _usuarioActual.Id,
                    NombreUsuario = _usuarioActual.NombreUsuario,
                    ContenidoId = _contenidoId,
                    TipoContenido = _tipoContenido,
                    Texto = txtComentario.Text,
                    FechaCreacion = DateTime.Now
                };

                if (_comentarioController.AgregarComentario(comentario))
                {
                    MessageBox.Show("Comentario agregado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtComentario.Text = string.Empty;
                    CargarComentarios(panelPrincipal);
                }
                else
                {
                    MessageBox.Show("Error al agregar el comentario.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            panelAgregarComentario.Controls.Add(btnEnviarComentario);

            // Panel para mostrar comentarios
            Panel panelComentarios = new Panel();
            panelComentarios.AutoScroll = true;
            panelComentarios.Location = new Point(20, 190);
            panelComentarios.Size = new Size(560, 260);
            panelPrincipal.Controls.Add(panelComentarios);

            // Cargar comentarios
            CargarComentarios(panelPrincipal);

            // Botón para cerrar
            Button btnCerrar = new Button();
            btnCerrar.Text = "Cerrar";
            btnCerrar.Location = new Point(480, 460);
            btnCerrar.Size = new Size(100, 30);
            btnCerrar.Click += (sender, e) => this.Close();
            panelPrincipal.Controls.Add(btnCerrar);
        }

        private void CargarComentarios(Panel panelPrincipal)
        {
            // Limpiar panel de comentarios existente
            var panelComentarios = panelPrincipal.Controls.OfType<Panel>().FirstOrDefault(p => p.Location.Y == 190);
            if (panelComentarios != null)
            {
                panelComentarios.Controls.Clear();
            }
            else
            {
                panelComentarios = new Panel
                {
                    AutoScroll = true,
                    Location = new Point(20, 190),
                    Size = new Size(560, 260)
                };
                panelPrincipal.Controls.Add(panelComentarios);
            }

            // Obtener comentarios
            var comentarios = _comentarioController.ObtenerComentariosPorContenido(_contenidoId, _tipoContenido);

            if (comentarios.Count == 0)
            {
                Label lblNoComentarios = new Label();
                lblNoComentarios.Text = "No hay comentarios para este contenido.";
                lblNoComentarios.Font = new Font("Segoe UI", 10);
                lblNoComentarios.AutoSize = true;
                lblNoComentarios.Location = new Point(10, 10);
                panelComentarios.Controls.Add(lblNoComentarios);
                return;
            }

            int yPos = 10;
            foreach (var comentario in comentarios)
            {
                Panel panelComentario = new Panel();
                panelComentario.BorderStyle = BorderStyle.FixedSingle;
                panelComentario.Location = new Point(0, yPos);
                panelComentario.Size = new Size(540, 100);
                panelComentarios.Controls.Add(panelComentario);

                // Usuario y fecha
                Label lblUsuarioFecha = new Label();
                lblUsuarioFecha.Text = $"{comentario.NombreUsuario} - {comentario.FechaCreacion:dd/MM/yyyy HH:mm}";
                lblUsuarioFecha.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                lblUsuarioFecha.AutoSize = true;
                lblUsuarioFecha.Location = new Point(10, 10);
                panelComentario.Controls.Add(lblUsuarioFecha);

                // Texto del comentario
                TextBox txtComentarioTexto = new TextBox();
                txtComentarioTexto.Text = comentario.Texto;
                txtComentarioTexto.Multiline = true;
                txtComentarioTexto.ReadOnly = true;
                txtComentarioTexto.BorderStyle = BorderStyle.None;
                txtComentarioTexto.BackColor = SystemColors.Control;
                txtComentarioTexto.Location = new Point(10, 35);
                txtComentarioTexto.Size = new Size(520, 55);
                panelComentario.Controls.Add(txtComentarioTexto);

                // Botones de editar y eliminar (solo para el usuario que lo creó o administradores)
                if (_usuarioActual.Id == comentario.UsuarioId || _usuarioActual.Rol == "Administrador")
                {
                    Button btnEliminar = new Button();
                    btnEliminar.Text = "Eliminar";
                    btnEliminar.Size = new Size(80, 25);
                    btnEliminar.Location = new Point(450, 5);
                    btnEliminar.Click += (sender, e) =>
                    {
                        if (MessageBox.Show("¿Está seguro de que desea eliminar este comentario?", "Confirmar eliminación",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            if (_comentarioController.EliminarComentario(comentario.Id))
                            {
                                MessageBox.Show("Comentario eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                CargarComentarios(panelPrincipal);
                            }
                            else
                            {
                                MessageBox.Show("Error al eliminar el comentario.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    };
                    panelComentario.Controls.Add(btnEliminar);
                }

                yPos += 110;
            }
        }

        private void ComentariosForm_Load(object sender, EventArgs e)
        {
            // Centrar el formulario en la pantalla
            this.CenterToScreen();
        }
    }
}
