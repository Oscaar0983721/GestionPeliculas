using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using GestionPeliculas.Controllers;

namespace GestionPeliculas.Views
{
    public partial class ReporteContenidoPopularForm : Form
    {
        private readonly ReporteController _reporteController;

        public ReporteContenidoPopularForm()
        {
            _reporteController = new ReporteController();
            
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ReporteContenidoPopularForm
            // 
            this.ClientSize = new System.Drawing.Size(800, 500);
            this.Name = "ReporteContenidoPopularForm";
            this.Text = "Reporte de Contenido Popular - Sistema de Gestión de Series y Películas";
            this.Load += new System.EventHandler(this.ReporteContenidoPopularForm_Load);
            this.ResumeLayout(false);

            // Panel principal
            Panel panelPrincipal = new Panel();
            panelPrincipal.Dock = DockStyle.Fill;
            this.Controls.Add(panelPrincipal);

            // Título
            Label lblTitulo = new Label();
            lblTitulo.Text = "Reporte de Contenido Popular";
            lblTitulo.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point(20, 20);
            panelPrincipal.Controls.Add(lblTitulo);

            // Descripción
            Label lblDescripcion = new Label();
            lblDescripcion.Text = "Este reporte muestra el contenido más visto por los usuarios.";
            lblDescripcion.Font = new Font("Segoe UI", 10);
            lblDescripcion.AutoSize = true;
            lblDescripcion.Location = new Point(20, 50);
            panelPrincipal.Controls.Add(lblDescripcion);

            // Panel de reporte
            Panel panelReporte = new Panel();
            panelReporte.BorderStyle = BorderStyle.FixedSingle;
            panelReporte.Location = new Point(20, 80);
            panelReporte.Size = new Size(760, 400);
            panelReporte.AutoScroll = true;
            panelPrincipal.Controls.Add(panelReporte);

            // ListView para mostrar el reporte
            ListView lvReporte = new ListView();
            lvReporte.View = View.Details;
            lvReporte.FullRowSelect = true;
            lvReporte.GridLines = true;
            lvReporte.Location = new Point(10, 10);
            lvReporte.Size = new Size(740, 380);
            
            // Columnas
            lvReporte.Columns.Add("Título", 300);
            lvReporte.Columns.Add("Visualizaciones", 150);
            
            panelReporte.Controls.Add(lvReporte);

            // Cargar datos del reporte
            var contenidoPopular = _reporteController.ObtenerContenidoPopular();
            
            foreach (var item in contenidoPopular)
            {
                ListViewItem lvItem = new ListViewItem(item.Key);
                lvItem.SubItems.Add(item.Value.ToString());
                lvReporte.Items.Add(lvItem);
            }
        }

        private void ReporteContenidoPopularForm_Load(object sender, EventArgs e)
        {
            // Centrar el formulario en la pantalla
            this.CenterToScreen();
        }
    }
}
