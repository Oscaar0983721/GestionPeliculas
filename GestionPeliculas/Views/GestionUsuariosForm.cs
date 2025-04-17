using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using GestionPeliculas.Controllers;
using GestionPeliculas.Models;

namespace GestionPeliculas.Views
{
    public partial class GestionUsuariosForm : Form
    {
        private readonly UsuarioController _usuarioController;
        private List<Usuario> _usuarios;

        public GestionUsuariosForm()
        {
            _usuarioController = new UsuarioController();
            _usuarios = _usuarioController.ObtenerTodosUsuarios();
            
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // GestionUsuariosForm
            // 
            this.ClientSize = new System.Drawing.Size(900, 600);
            this.Name = "GestionUsuariosForm";
            this.Text = "Gestión de Usuarios - Sistema de Gestión de Series y Películas";
            this.Load += new System.EventHandler(this.GestionUsuariosForm_Load);
            this.ResumeLayout(false);

            // Panel principal
            Panel panelPrincipal = new Panel();
            panelPrincipal.Dock = DockStyle.Fill;
            this.Controls.Add(panelPrincipal);

            // Título
            Label lblTitulo = new Label();
            lblTitulo.Text = "Gestión de Usuarios";
            lblTitulo.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point(20, 20);
            panelPrincipal.Controls.Add(lblTitulo);

            // Panel de búsqueda
            Panel panelBusqueda = new Panel();
            panelBusqueda.BorderStyle = BorderStyle.FixedSingle;
            panelBusqueda.Location = new Point(20, 60);
            panelBusqueda.Size = new Size(860, 60);
            panelPrincipal.Controls.Add(panelBusqueda);

            // Texto de búsqueda
            Label lblBuscar = new Label();
            lblBuscar.Text = "Buscar usuario:";
            lblBuscar.AutoSize = true;
            lblBuscar.Location = new Point(10, 20);
            panelBusqueda.Controls.Add(lblBuscar);

            TextBox txtBuscar = new TextBox();
            txtBuscar.Location = new Point(110, 17);
            txtBuscar.Size = new Size(300, 23);
            panelBusqueda.Controls.Add(txtBuscar);

            // Botón de buscar
            Button btnBuscar = new Button();
            btnBuscar.Text = "Buscar";
            btnBuscar.Location = new Point(420, 16);
            btnBuscar.Size = new Size(80, 25);
            btnBuscar.Click += (sender, e) =>
            {
                string textoBusqueda = txtBuscar.Text.Trim().ToLower();
                
                if (string.IsNullOrEmpty(textoBusqueda))
                {
                    _usuarios = _usuarioController.ObtenerTodosUsuarios();
                }
                else
                {
                    _usuarios = _usuarioController.ObtenerTodosUsuarios().FindAll(u => 
                        u.NombreUsuario.ToLower().Contains(textoBusqueda) || 
                        u.Email.ToLower().Contains(textoBusqueda));
                }
                
                MostrarUsuarios();
            };
            panelBusqueda.Controls.Add(btnBuscar);

            // Botón de limpiar
            Button btnLimpiar = new Button();
            btnLimpiar.Text = "Limpiar";
            btnLimpiar.Location = new Point(510, 16);
            btnLimpiar.Size = new Size(80, 25);
            btnLimpiar.Click += (sender, e) =>
            {
                txtBuscar.Text = "";
                _usuarios = _usuarioController.ObtenerTodosUsuarios();
                MostrarUsuarios();
            };
            panelBusqueda.Controls.Add(btnLimpiar);

            // Botón de agregar usuario
            Button btnAgregar = new Button();
            btnAgregar.Text = "Agregar Usuario";
            btnAgregar.Location = new Point(720, 16);
            btnAgregar.Size = new Size(120, 25);
            btnAgregar.Click += (sender, e) =>
            {
                Form registroForm = new RegistroForm();
                registroForm.ShowDialog();
                
                // Actualizar lista de usuarios
                _usuarios = _usuarioController.ObtenerTodosUsuarios();
                MostrarUsuarios();
            };
            panelBusqueda.Controls.Add(btnAgregar);

            // Panel de usuarios
            Panel panelUsuarios = new Panel();
            panelUsuarios.BorderStyle = BorderStyle.FixedSingle;
            panelUsuarios.Location = new Point(20, 130);
            panelUsuarios.Size = new Size(860, 450);
            panelUsuarios.AutoScroll = true;
            panelUsuarios.Tag = "panelUsuarios"; // Para identificarlo al actualizar
            panelPrincipal.Controls.Add(panelUsuarios);

            // ListView para mostrar los usuarios
            ListView lvUsuarios = new ListView();
            lvUsuarios.View = View.Details;
            lvUsuarios.FullRowSelect = true;
            lvUsuarios.GridLines = true;
            lvUsuarios.Location = new Point(10, 10);
            lvUsuarios.Size = new Size(840, 430);
            lvUsuarios.Tag = "lvUsuarios"; // Para identificarlo al actualizar
            
            // Columnas
            lvUsuarios.Columns.Add("ID", 50);
            lvUsuarios.Columns.Add("Usuario", 150);
            lvUsuarios.Columns.Add("Email", 200);
            lvUsuarios.Columns.Add("Rol", 100);
            lvUsuarios.Columns.Add("Fecha Registro", 150);
            lvUsuarios.Columns.Add("Contenido Visto", 100);
            
            panelUsuarios.Controls.Add(lvUsuarios);

            // Menú contextual para editar/eliminar usuario
            ContextMenuStrip menuContextual = new ContextMenuStrip();
            
            ToolStripMenuItem menuEditar = new ToolStripMenuItem("Editar usuario");
            menuEditar.Click += (sender, e) =>
            {
                if (lvUsuarios.SelectedItems.Count > 0)
                {
                    int id = int.Parse(lvUsuarios.SelectedItems[0].SubItems[0].Text);
                    EditarUsuario(id);
                }
            };
            menuContextual.Items.Add(menuEditar);
            
            ToolStripMenuItem menuEliminar = new ToolStripMenuItem("Eliminar usuario");
            menuEliminar.Click += (sender, e) =>
            {
                if (lvUsuarios.SelectedItems.Count > 0)
                {
                    int id = int.Parse(lvUsuarios.SelectedItems[0].SubItems[0].Text);
                    EliminarUsuario(id);
                }
            };
            menuContextual.Items.Add(menuEliminar);
            
            lvUsuarios.ContextMenuStrip = menuContextual;

            // Mostrar usuarios
            MostrarUsuarios();
        }

        private void GestionUsuariosForm_Load(object sender, EventArgs e)
        {
            // Centrar el formulario en la pantalla
            this.CenterToScreen();
        }

        private void MostrarUsuarios()
        {
            // Obtener el ListView
            ListView lvUsuarios = null;
            foreach (Control control in this.Controls[0].Controls)
            {
                if (control is Panel && control.Tag?.ToString() == "panelUsuarios")
                {
                    foreach (Control subControl in control.Controls)
                    {
                        if (subControl is ListView && subControl.Tag?.ToString() == "lvUsuarios")
                        {
                            lvUsuarios = (ListView)subControl;
                            break;
                        }
                    }
                    break;
                }
            }

            if (lvUsuarios == null)
            {
                return;
            }

            // Limpiar el ListView
            lvUsuarios.Items.Clear();

            // Mostrar usuarios
            foreach (var usuario in _usuarios)
            {
                ListViewItem item = new ListViewItem(usuario.Id.ToString());
                item.SubItems.Add(usuario.NombreUsuario);
                item.SubItems.Add(usuario.Email);
                item.SubItems.Add(usuario.Rol);
                item.SubItems.Add(usuario.FechaRegistro.ToString("dd/MM/yyyy HH:mm"));
                item.SubItems.Add(usuario.ContenidoVisto.Count.ToString());
                item.Tag = usuario;
                
                lvUsuarios.Items.Add(item);
            }
        }

        private void EditarUsuario(int id)
        {
            var usuario = _usuarioController.ObtenerUsuarioPorId(id);
            
            if (usuario == null)
            {
                MessageBox.Show("Usuario no encontrado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Crear formulario de edición
            Form formEditar = new Form();
            formEditar.Text = "Editar Usuario";
            formEditar.Size = new Size(400, 350);
            formEditar.StartPosition = FormStartPosition.CenterParent;
            formEditar.FormBorderStyle = FormBorderStyle.FixedDialog;
            formEditar.MaximizeBox = false;
            formEditar.MinimizeBox = false;

            // Controles del formulario
            Label lblUsuario = new Label();
            lblUsuario.Text = "Usuario:";
            lblUsuario.Location = new Point(30, 30);
            lblUsuario.AutoSize = true;
            formEditar.Controls.Add(lblUsuario);

            TextBox txtUsuario = new TextBox();
            txtUsuario.Text = usuario.NombreUsuario;
            txtUsuario.Location = new Point(150, 27);
            txtUsuario.Size = new Size(200, 23);
            formEditar.Controls.Add(txtUsuario);

            Label lblEmail = new Label();
            lblEmail.Text = "Email:";
            lblEmail.Location = new Point(30, 70);
            lblEmail.AutoSize = true;
            formEditar.Controls.Add(lblEmail);

            TextBox txtEmail = new TextBox();
            txtEmail.Text = usuario.Email;
            txtEmail.Location = new Point(150, 67);
            txtEmail.Size = new Size(200, 23);
            formEditar.Controls.Add(txtEmail);

            Label lblRol = new Label();
            lblRol.Text = "Rol:";
            lblRol.Location = new Point(30, 110);
            lblRol.AutoSize = true;
            formEditar.Controls.Add(lblRol);

            ComboBox cmbRol = new ComboBox();
            cmbRol.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRol.Items.Add("Usuario");
            cmbRol.Items.Add("Administrador");
            cmbRol.Text = usuario.Rol;
            cmbRol.Location = new Point(150, 107);
            cmbRol.Size = new Size(200, 23);
            formEditar.Controls.Add(cmbRol);

            Label lblResetPassword = new Label();
            lblResetPassword.Text = "Nueva contraseña:";
            lblResetPassword.Location = new Point(30, 150);
            lblResetPassword.AutoSize = true;
            formEditar.Controls.Add(lblResetPassword);

            TextBox txtPassword = new TextBox();
            txtPassword.PasswordChar = '*';
            txtPassword.Location = new Point(150, 147);
            txtPassword.Size = new Size(200, 23);
            formEditar.Controls.Add(txtPassword);

            Label lblConfirmPassword = new Label();
            lblConfirmPassword.Text = "Confirmar contraseña:";
            lblConfirmPassword.Location = new Point(30, 190);
            lblConfirmPassword.AutoSize = true;
            formEditar.Controls.Add(lblConfirmPassword);

            TextBox txtConfirmPassword = new TextBox();
            txtConfirmPassword.PasswordChar = '*';
            txtConfirmPassword.Location = new Point(150, 187);
            txtConfirmPassword.Size = new Size(200, 23);
            formEditar.Controls.Add(txtConfirmPassword);

            Button btnGuardar = new Button();
            btnGuardar.Text = "Guardar";
            btnGuardar.Location = new Point(150, 240);
            btnGuardar.Size = new Size(100, 30);
            btnGuardar.Click += (sender, e) =>
            {
                // Validar datos
                if (string.IsNullOrEmpty(txtUsuario.Text) || string.IsNullOrEmpty(txtEmail.Text))
                {
                    MessageBox.Show("Por favor, complete los campos obligatorios.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Validar contraseñas si se van a cambiar
                if (!string.IsNullOrEmpty(txtPassword.Text))
                {
                    if (txtPassword.Text != txtConfirmPassword.Text)
                    {
                        MessageBox.Show("Las contraseñas no coinciden.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                // Actualizar usuario
                usuario.NombreUsuario = txtUsuario.Text;
                usuario.Email = txtEmail.Text;
                usuario.Rol = cmbRol.Text;

                // Actualizar contraseña si se proporcionó una nueva
                if (!string.IsNullOrEmpty(txtPassword.Text))
                {
                    usuario.Contraseña = txtPassword.Text;
                }

                if (_usuarioController.ActualizarUsuario(usuario))
                {
                    MessageBox.Show("Usuario actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    formEditar.Close();
                    
                    // Actualizar lista de usuarios
                    _usuarios = _usuarioController.ObtenerTodosUsuarios();
                    MostrarUsuarios();
                }
                else
                {
                    MessageBox.Show("Error al actualizar el usuario.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            formEditar.Controls.Add(btnGuardar);

            Button btnCancelar = new Button();
            btnCancelar.Text = "Cancelar";
            btnCancelar.Location = new Point(260, 240);
            btnCancelar.Size = new Size(90, 30);
            btnCancelar.Click += (sender, e) => formEditar.Close();
            formEditar.Controls.Add(btnCancelar);

            formEditar.ShowDialog();
        }

        private void EliminarUsuario(int id)
        {
            var usuario = _usuarioController.ObtenerUsuarioPorId(id);
            
            if (usuario == null)
            {
                MessageBox.Show("Usuario no encontrado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Confirmar eliminación
            if (MessageBox.Show($"¿Está seguro de eliminar al usuario '{usuario.NombreUsuario}'?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (_usuarioController.EliminarUsuario(id))
                {
                    MessageBox.Show("Usuario eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    // Actualizar lista de usuarios
                    _usuarios = _usuarioController.ObtenerTodosUsuarios();
                    MostrarUsuarios();
                }
                else
                {
                    MessageBox.Show("Error al eliminar el usuario.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
