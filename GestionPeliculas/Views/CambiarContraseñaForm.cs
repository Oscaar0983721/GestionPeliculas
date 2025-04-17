using System;
using System.Windows.Forms;
using GestionPeliculas.Controllers;
using GestionPeliculas.Models;

namespace GestionPeliculas.Views
{
    public partial class CambiarContraseñaForm : Form
    {
        private readonly Usuario _usuarioActual;
        private readonly UsuarioController _usuarioController;

        public CambiarContraseñaForm(Usuario usuario)
        {
            _usuarioActual = usuario;
            _usuarioController = new UsuarioController();
            
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // CambiarContraseñaForm
            // 
            this.ClientSize = new System.Drawing.Size(400, 300);
            this.Name = "CambiarContraseñaForm";
            this.Text = "Cambiar Contraseña - Sistema de Gestión de Series y Películas";
            this.Load += new System.EventHandler(this.CambiarContraseñaForm_Load);
            this.ResumeLayout(false);

            // Panel principal
            Panel panel = new Panel();
            panel.Dock = DockStyle.Fill;
            this.Controls.Add(panel);

            // Título
            Label lblTitulo = new Label();
            lblTitulo.Text = "Cambiar Contraseña";
            lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14, System.Drawing.FontStyle.Bold);
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new System.Drawing.Point(120, 30);
            panel.Controls.Add(lblTitulo);

            // Contraseña actual
            Label lblContraseñaActual = new Label();
            lblContraseñaActual.Text = "Contraseña actual:";
            lblContraseñaActual.Location = new System.Drawing.Point(50, 100);
            lblContraseñaActual.AutoSize = true;
            panel.Controls.Add(lblContraseñaActual);

            TextBox txtContraseñaActual = new TextBox();
            txtContraseñaActual.Name = "txtContraseñaActual";
            txtContraseñaActual.PasswordChar = '*';
            txtContraseñaActual.Location = new System.Drawing.Point(180, 100);
            txtContraseñaActual.Size = new System.Drawing.Size(170, 23);
            panel.Controls.Add(txtContraseñaActual);

            // Nueva contraseña
            Label lblNuevaContraseña = new Label();
            lblNuevaContraseña.Text = "Nueva contraseña:";
            lblNuevaContraseña.Location = new System.Drawing.Point(50, 140);
            lblNuevaContraseña.AutoSize = true;
            panel.Controls.Add(lblNuevaContraseña);

            TextBox txtNuevaContraseña = new TextBox();
            txtNuevaContraseña.Name = "txtNuevaContraseña";
            txtNuevaContraseña.PasswordChar = '*';
            txtNuevaContraseña.Location = new System.Drawing.Point(180, 140);
            txtNuevaContraseña.Size = new System.Drawing.Size(170, 23);
            panel.Controls.Add(txtNuevaContraseña);

            // Confirmar nueva contraseña
            Label lblConfirmarContraseña = new Label();
            lblConfirmarContraseña.Text = "Confirmar contraseña:";
            lblConfirmarContraseña.Location = new System.Drawing.Point(50, 180);
            lblConfirmarContraseña.AutoSize = true;
            panel.Controls.Add(lblConfirmarContraseña);

            TextBox txtConfirmarContraseña = new TextBox();
            txtConfirmarContraseña.Name = "txtConfirmarContraseña";
            txtConfirmarContraseña.PasswordChar = '*';
            txtConfirmarContraseña.Location = new System.Drawing.Point(180, 180);
            txtConfirmarContraseña.Size = new System.Drawing.Size(170, 23);
            panel.Controls.Add(txtConfirmarContraseña);

            // Botón de cambiar contraseña
            Button btnCambiarContraseña = new Button();
            btnCambiarContraseña.Text = "Cambiar Contraseña";
            btnCambiarContraseña.Location = new System.Drawing.Point(130, 230);
            btnCambiarContraseña.Size = new System.Drawing.Size(140, 30);
            btnCambiarContraseña.Click += (sender, e) =>
            {
                string contraseñaActual = txtContraseñaActual.Text;
                string nuevaContraseña = txtNuevaContraseña.Text;
                string confirmarContraseña = txtConfirmarContraseña.Text;

                if (string.IsNullOrEmpty(contraseñaActual) || string.IsNullOrEmpty(nuevaContraseña) || string.IsNullOrEmpty(confirmarContraseña))
                {
                    MessageBox.Show("Por favor, complete todos los campos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (nuevaContraseña != confirmarContraseña)
                {
                    MessageBox.Show("Las contraseñas no coinciden.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!_usuarioController.AutenticarUsuario(_usuarioActual.NombreUsuario, contraseñaActual))
                {
                    MessageBox.Show("La contraseña actual es incorrecta.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Actualizar contraseña
                _usuarioActual.Contraseña = nuevaContraseña;
                if (_usuarioController.ActualizarUsuario(_usuarioActual))
                {
                    MessageBox.Show("Contraseña cambiada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Error al cambiar la contraseña.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            panel.Controls.Add(btnCambiarContraseña);
        }

        private void CambiarContraseñaForm_Load(object sender, EventArgs e)
        {
            // Centrar el formulario en la pantalla
            this.CenterToScreen();
        }
    }
}
