using System;
using System.Windows.Forms;
using GestionPeliculas.Controllers;
using GestionPeliculas.Models;

namespace GestionPeliculas.Views
{
    public partial class RegistroForm : Form
    {
        private readonly UsuarioController _usuarioController;

        public RegistroForm()
        {
            InitializeComponent();
            _usuarioController = new UsuarioController();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // RegistroForm
            // 
            this.ClientSize = new System.Drawing.Size(400, 350);
            this.Name = "RegistroForm";
            this.Text = "Registro de Usuario - Sistema de Gestión de Series y Películas";
            this.Load += new System.EventHandler(this.RegistroForm_Load);
            this.ResumeLayout(false);

            // Panel principal
            Panel panel = new Panel();
            panel.Dock = DockStyle.Fill;
            this.Controls.Add(panel);

            // Título
            Label lblTitulo = new Label();
            lblTitulo.Text = "Registro de Usuario";
            lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14, System.Drawing.FontStyle.Bold);
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new System.Drawing.Point(120, 30);
            panel.Controls.Add(lblTitulo);

            // Nombre de usuario
            Label lblUsuario = new Label();
            lblUsuario.Text = "Usuario:";
            lblUsuario.Location = new System.Drawing.Point(50, 100);
            lblUsuario.AutoSize = true;
            panel.Controls.Add(lblUsuario);

            TextBox txtUsuario = new TextBox();
            txtUsuario.Name = "txtUsuario";
            txtUsuario.Location = new System.Drawing.Point(150, 100);
            txtUsuario.Size = new System.Drawing.Size(200, 23);
            panel.Controls.Add(txtUsuario);

            // Email
            Label lblEmail = new Label();
            lblEmail.Text = "Email:";
            lblEmail.Location = new System.Drawing.Point(50, 140);
            lblEmail.AutoSize = true;
            panel.Controls.Add(lblEmail);

            TextBox txtEmail = new TextBox();
            txtEmail.Name = "txtEmail";
            txtEmail.Location = new System.Drawing.Point(150, 140);
            txtEmail.Size = new System.Drawing.Size(200, 23);
            panel.Controls.Add(txtEmail);

            // Contraseña
            Label lblContraseña = new Label();
            lblContraseña.Text = "Contraseña:";
            lblContraseña.Location = new System.Drawing.Point(50, 180);
            lblContraseña.AutoSize = true;
            panel.Controls.Add(lblContraseña);

            TextBox txtContraseña = new TextBox();
            txtContraseña.Name = "txtContraseña";
            txtContraseña.PasswordChar = '*';
            txtContraseña.Location = new System.Drawing.Point(150, 180);
            txtContraseña.Size = new System.Drawing.Size(200, 23);
            panel.Controls.Add(txtContraseña);

            // Confirmar contraseña
            Label lblConfirmarContraseña = new Label();
            lblConfirmarContraseña.Text = "Confirmar:";
            lblConfirmarContraseña.Location = new System.Drawing.Point(50, 220);
            lblConfirmarContraseña.AutoSize = true;
            panel.Controls.Add(lblConfirmarContraseña);

            TextBox txtConfirmarContraseña = new TextBox();
            txtConfirmarContraseña.Name = "txtConfirmarContraseña";
            txtConfirmarContraseña.PasswordChar = '*';
            txtConfirmarContraseña.Location = new System.Drawing.Point(150, 220);
            txtConfirmarContraseña.Size = new System.Drawing.Size(200, 23);
            panel.Controls.Add(txtConfirmarContraseña);

            // Botón de registro
            Button btnRegistrarse = new Button();
            btnRegistrarse.Text = "Registrarse";
            btnRegistrarse.Location = new System.Drawing.Point(150, 270);
            btnRegistrarse.Size = new System.Drawing.Size(100, 30);
            btnRegistrarse.Click += (sender, e) =>
            {
                string usuario = txtUsuario.Text;
                string email = txtEmail.Text;
                string contraseña = txtContraseña.Text;
                string confirmarContraseña = txtConfirmarContraseña.Text;

                if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(email) || 
                    string.IsNullOrEmpty(contraseña) || string.IsNullOrEmpty(confirmarContraseña))
                {
                    MessageBox.Show("Por favor, complete todos los campos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (contraseña != confirmarContraseña)
                {
                    MessageBox.Show("Las contraseñas no coinciden.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var nuevoUsuario = new Usuario
                {
                    NombreUsuario = usuario,
                    Email = email,
                    Contraseña = contraseña,
                    Rol = "Usuario"
                };

                if (_usuarioController.RegistrarUsuario(nuevoUsuario))
                {
                    MessageBox.Show("Usuario registrado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("El nombre de usuario ya existe.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            panel.Controls.Add(btnRegistrarse);

            // Botón de cancelar
            Button btnCancelar = new Button();
            btnCancelar.Text = "Cancelar";
            btnCancelar.Location = new System.Drawing.Point(260, 270);
            btnCancelar.Size = new System.Drawing.Size(90, 30);
            btnCancelar.Click += (sender, e) => this.Close();
            panel.Controls.Add(btnCancelar);
        }

        private void RegistroForm_Load(object sender, EventArgs e)
        {
            // Centrar el formulario en la pantalla
            this.CenterToScreen();
        }
    }
}
