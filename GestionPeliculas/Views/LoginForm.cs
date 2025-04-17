using System;
using System.Windows.Forms;
using GestionPeliculas.Controllers;
using GestionPeliculas.Models;

namespace GestionPeliculas.Views
{
    public partial class LoginForm : Form
    {
        private readonly UsuarioController _usuarioController;

        public LoginForm()
        {
            InitializeComponent();
            _usuarioController = new UsuarioController();
            
            // Inicializar datos de prueba si no existen
            _usuarioController.InicializarDatosUsuario();
            
            var contenidoController = new ContenidoController();
            contenidoController.InicializarDatosContenido();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // LoginForm
            // 
            this.ClientSize = new System.Drawing.Size(400, 300);
            this.Name = "LoginForm";
            this.Text = "Iniciar Sesión - Sistema de Gestión de Series y Películas";
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.ResumeLayout(false);

            // Panel principal
            Panel panel = new Panel();
            panel.Dock = DockStyle.Fill;
            this.Controls.Add(panel);

            // Título
            Label lblTitulo = new Label();
            lblTitulo.Text = "Sistema de Gestión de Series y Películas";
            lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14, System.Drawing.FontStyle.Bold);
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new System.Drawing.Point(50, 30);
            panel.Controls.Add(lblTitulo);

            // Usuario
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

            // Contraseña
            Label lblContraseña = new Label();
            lblContraseña.Text = "Contraseña:";
            lblContraseña.Location = new System.Drawing.Point(50, 140);
            lblContraseña.AutoSize = true;
            panel.Controls.Add(lblContraseña);

            TextBox txtContraseña = new TextBox();
            txtContraseña.Name = "txtContraseña";
            txtContraseña.PasswordChar = '*';
            txtContraseña.Location = new System.Drawing.Point(150, 140);
            txtContraseña.Size = new System.Drawing.Size(200, 23);
            panel.Controls.Add(txtContraseña);

            // Botón de inicio de sesión
            Button btnIniciarSesion = new Button();
            btnIniciarSesion.Text = "Iniciar Sesión";
            btnIniciarSesion.Location = new System.Drawing.Point(150, 190);
            btnIniciarSesion.Size = new System.Drawing.Size(100, 30);
            btnIniciarSesion.Click += (sender, e) =>
            {
                string usuario = txtUsuario.Text;
                string contraseña = txtContraseña.Text;

                if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(contraseña))
                {
                    MessageBox.Show("Por favor, ingrese usuario y contraseña.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (_usuarioController.AutenticarUsuario(usuario, contraseña))
                {
                    var usuarioActual = _usuarioController.ObtenerUsuarioPorNombre(usuario);
                    this.Hide();
                    var mainForm = new MainForm(usuarioActual);
                    mainForm.FormClosed += (s, args) => this.Close();
                    mainForm.Show();
                }
                else
                {
                    MessageBox.Show("Usuario o contraseña incorrectos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            panel.Controls.Add(btnIniciarSesion);

            // Botón de registro
            Button btnRegistrarse = new Button();
            btnRegistrarse.Text = "Registrarse";
            btnRegistrarse.Location = new System.Drawing.Point(260, 190);
            btnRegistrarse.Size = new System.Drawing.Size(90, 30);
            btnRegistrarse.Click += (sender, e) =>
            {
                this.Hide();
                var registroForm = new RegistroForm();
                registroForm.FormClosed += (s, args) => this.Show();
                registroForm.Show();
            };
            panel.Controls.Add(btnRegistrarse);
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            // Centrar el formulario en la pantalla
            this.CenterToScreen();
        }
    }
}
