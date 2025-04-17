using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using GestionPeliculas.Controllers;

namespace GestionPeliculas.Views
{
    public partial class ReporteDistribucionPlataformasForm : Form
    {
        private readonly ReporteController _reporteController;

        public ReporteDistribucionPlataformasForm()
        {
            _reporteController = new ReporteController();
            
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ReporteDistribucionPlataformasForm
            // 
            this.ClientSize = new System.Drawing.Size(800, 500);
            this.Name = "ReporteDistribucionPlataformasForm";
            this.Text = "Reporte de Distribución de Plataformas - Sistema de Gestión de Series y Películas";
            this.Load += new System.EventHandler(this.ReporteDistribucionPlataformasForm_Load);
            this.ResumeLayout(false);

            // Panel principal
            Panel panelPrincipal = new Panel();
            panelPrincipal.Dock = DockStyle.Fill;
            this.Controls.Add(panelPrincipal);

            // Título
            Label lblTitulo = new Label();
            lblTitulo.Text = "Reporte de Distribución de Plataformas";
            lblTitulo.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point(20, 20);
            panelPrincipal.Controls.Add(lblTitulo);

            // Descripción
            Label lblDescripcion = new Label();
            lblDescripcion.Text = "Este reporte muestra la distribución de plataformas en el catálogo de contenido.";
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
            lvReporte.Columns.Add("Plataforma", 300);
            lvReporte.Columns.Add("Cantidad", 150);
            lvReporte.Columns.Add("Porcentaje", 150);
            
            panelReporte.Controls.Add(lvReporte);

            // Cargar datos del reporte
            var distribucionPlataformas = _reporteController.ObtenerDistribucionPlataformas();
            int total = distribucionPlataformas.Values.Sum();
            
            foreach (var item in distribucionPlataformas)
            {
                ListViewItem lvItem = new ListViewItem(item.Key);
                lvItem.SubItems.Add(item.Value.ToString());
                double porcentaje = (double)item.Value / total * 100;
                lvItem.SubItems.Add(porcentaje.ToString("F2") + "%");
                lvReporte.Items.Add(lvItem);
            }
        }

        private void ReporteDistribucionPlataformasForm_Load(object sender, EventArgs e)
        {
            // Centrar el formulario en la pantalla
            this.CenterToScreen();
        }
    }
}
