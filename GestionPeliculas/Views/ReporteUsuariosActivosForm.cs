using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using GestionPeliculas.Controllers;

namespace GestionPeliculas.Views
{
    public partial class ReporteUsuariosActivosForm : Form
    {
        private readonly ReporteController _reporteController;

        public ReporteUsuariosActivosForm()
        {
            _reporteController = new ReporteController();
            
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ReporteUsuariosActivosForm
            // 
            this.ClientSize = new System.Drawing.Size(800, 500);
            this.Name = "ReporteUsuariosActivosForm";
            this.Text = "Reporte de Usuarios Activos - Sistema de Gestión de Series y Películas";
            this.Load += new System.EventHandler(this.ReporteUsuariosActivosForm_Load);
            this.ResumeLayout(false);

            // Panel principal
            Panel panelPrincipal = new Panel();
            panelPrincipal.Dock = DockStyle.Fill;
            this.Controls.Add(panelPrincipal);

            // Título
            Label lblTitulo = new Label();
            lblTitulo.Text = "Reporte de Usuarios Activos";
            lblTitulo.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point(20, 20);
            panelPrincipal.Controls.Add(lblTitulo);

            // Descripción
            Label lblDescripcion = new Label();
            lblDescripcion.Text = "Este reporte muestra los usuarios más activos en los últimos 30 días.";
            lblDescripcion.Font = new Font("Segoe UI", 10);
            lblDescripcion.AutoSize = true;
            lblDescripcion.Location = new Point(20, 50);
            panelPrincipal.Controls.Add(lblDescripcion);

            // Panel de filtros
            Panel panelFiltros = new Panel();
            panelFiltros.BorderStyle = BorderStyle.FixedSingle;
            panelFiltros.Location = new Point(20, 80);
            panelFiltros.Size = new Size(760, 60);
            panelPrincipal.Controls.Add(panelFiltros);

            // Filtro por días
            Label lblDias = new Label();
            lblDias.Text = "Período (días):";
            lblDias.AutoSize = true;
            lblDias.Location = new Point(10, 20);
            panelFiltros.Controls.Add(lblDias);

            ComboBox cmbDias = new ComboBox();
            cmbDias.Location = new Point(110, 17);
            cmbDias.Size = new Size(100, 23);
            cmbDias.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDias.Items.Add("7");
            cmbDias.Items.Add("15");
            cmbDias.Items.Add("30");
            cmbDias.Items.Add("60");
            cmbDias.Items.Add("90");
            cmbDias.SelectedIndex = 2; // 30 días por defecto
            panelFiltros.Controls.Add(cmbDias);

            // Botón de filtrar
            Button btnFiltrar = new Button();
            btnFiltrar.Text = "Filtrar";
            btnFiltrar.Location = new Point(220, 16);
            btnFiltrar.Size = new Size(80, 25);
            panelFiltros.Controls.Add(btnFiltrar);

            // Panel de reporte
            Panel panelReporte = new Panel();
            panelReporte.BorderStyle = BorderStyle.FixedSingle;
            panelReporte.Location = new Point(20, 150);
            panelReporte.Size = new Size(760, 330);
            panelReporte.AutoScroll = true;
            panelReporte.Tag = "panelReporte"; // Para identificarlo al actualizar
            panelPrincipal.Controls.Add(panelReporte);

            // ListView para mostrar el reporte
            ListView lvReporte = new ListView();
            lvReporte.View = View.Details;
            lvReporte.FullRowSelect = true;
            lvReporte.GridLines = true;
            lvReporte.Location = new Point(10, 10);
            lvReporte.Size = new Size(740, 310);
            
            // Columnas
            lvReporte.Columns.Add("Usuario", 300);
            lvReporte.Columns.Add("Actividad", 150);
            
            panelReporte.Controls.Add(lvReporte);

            // Cargar datos del reporte
            CargarDatosReporte(lvReporte, 30);

            // Evento de filtrar
            btnFiltrar.Click += (sender, e) =>
            {
                int dias = int.Parse(cmbDias.SelectedItem.ToString());
                CargarDatosReporte(lvReporte, dias);
            };
        }

        private void CargarDatosReporte(ListView lvReporte, int dias)
        {
            lvReporte.Items.Clear();
            
            var usuariosActivos = _reporteController.ObtenerUsuariosActivos(dias);
            
            foreach (var item in usuariosActivos)
            {
                ListViewItem lvItem = new ListViewItem(item.Key);
                lvItem.SubItems.Add(item.Value.ToString());
                lvReporte.Items.Add(lvItem);
            }
        }

        private void ReporteUsuariosActivosForm_Load(object sender, EventArgs e)
        {
            // Centrar el formulario en la pantalla
            this.CenterToScreen();
        }
    }
}
