using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using GestionPeliculas.Services;

namespace GestionPeliculas.Views
{
    public partial class ReportesForm : Form
    {
        private readonly ReportesPdfService _reportesPdfService;

        public ReportesForm()
        {
            _reportesPdfService = new ReportesPdfService();

            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ReportesForm
            // 
            this.ClientSize = new System.Drawing.Size(600, 400);
            this.Name = "ReportesForm";
            this.Text = "Generación de Reportes - Sistema de Gestión de Series y Películas";
            this.Load += new System.EventHandler(this.ReportesForm_Load);
            this.ResumeLayout(false);

            // Panel principal
            Panel panelPrincipal = new Panel();
            panelPrincipal.Dock = DockStyle.Fill;
            this.Controls.Add(panelPrincipal);

            // Título
            Label lblTitulo = new Label();
            lblTitulo.Text = "Generación de Reportes";
            lblTitulo.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point(20, 20);
            panelPrincipal.Controls.Add(lblTitulo);

            // Descripción
            Label lblDescripcion = new Label();
            lblDescripcion.Text = "Seleccione el tipo de reporte que desea generar:";
            lblDescripcion.Font = new Font("Segoe UI", 10);
            lblDescripcion.AutoSize = true;
            lblDescripcion.Location = new Point(20, 60);
            panelPrincipal.Controls.Add(lblDescripcion);

            // Panel de opciones
            Panel panelOpciones = new Panel();
            panelOpciones.BorderStyle = BorderStyle.FixedSingle;
            panelOpciones.Location = new Point(20, 90);
            panelOpciones.Size = new Size(560, 250);
            panelPrincipal.Controls.Add(panelOpciones);

            // Opción 1: Usuarios suscritos
            RadioButton rbUsuariosSuscritos = new RadioButton();
            rbUsuariosSuscritos.Text = "Usuarios registrados en el último mes";
            rbUsuariosSuscritos.Location = new Point(20, 20);
            rbUsuariosSuscritos.AutoSize = true;
            rbUsuariosSuscritos.Checked = true;
            panelOpciones.Controls.Add(rbUsuariosSuscritos);

            Label lblUsuariosSuscritos = new Label();
            lblUsuariosSuscritos.Text = "Genera un reporte con los usuarios registrados en el último mes, incluyendo estadísticas de crecimiento.";
            lblUsuariosSuscritos.Location = new Point(40, 45);
            lblUsuariosSuscritos.AutoSize = true;
            panelOpciones.Controls.Add(lblUsuariosSuscritos);

            // Opción 2: Interacción de usuarios
            RadioButton rbInteraccionUsuarios = new RadioButton();
            rbInteraccionUsuarios.Text = "Interacción de usuarios con la aplicación";
            rbInteraccionUsuarios.Location = new Point(20, 80);
            rbInteraccionUsuarios.AutoSize = true;
            panelOpciones.Controls.Add(rbInteraccionUsuarios);

            Label lblInteraccionUsuarios = new Label();
            lblInteraccionUsuarios.Text = "Genera un reporte con estadísticas de actividad de los usuarios en la plataforma.";
            lblInteraccionUsuarios.Location = new Point(40, 105);
            lblInteraccionUsuarios.AutoSize = true;
            panelOpciones.Controls.Add(lblInteraccionUsuarios);

            // Opción 3: Contenido popular
            RadioButton rbContenidoPopular = new RadioButton();
            rbContenidoPopular.Text = "Géneros y series más populares";
            rbContenidoPopular.Location = new Point(20, 140);
            rbContenidoPopular.AutoSize = true;
            panelOpciones.Controls.Add(rbContenidoPopular);

            Label lblContenidoPopular = new Label();
            lblContenidoPopular.Text = "Genera un reporte con el contenido más visto y los géneros más populares.";
            lblContenidoPopular.Location = new Point(40, 165);
            lblContenidoPopular.AutoSize = true;
            panelOpciones.Controls.Add(lblContenidoPopular);

            // Fechas para el reporte
            Label lblFechaInicio = new Label();
            lblFechaInicio.Text = "Fecha inicio:";
            lblFechaInicio.Location = new Point(20, 200);
            lblFechaInicio.AutoSize = true;
            panelOpciones.Controls.Add(lblFechaInicio);

            DateTimePicker dtpFechaInicio = new DateTimePicker();
            dtpFechaInicio.Format = DateTimePickerFormat.Short;
            dtpFechaInicio.Value = DateTime.Now.AddMonths(-1);
            dtpFechaInicio.Location = new Point(100, 198);
            dtpFechaInicio.Size = new Size(120, 23);
            panelOpciones.Controls.Add(dtpFechaInicio);

            Label lblFechaFin = new Label();
            lblFechaFin.Text = "Fecha fin:";
            lblFechaFin.Location = new Point(240, 200);
            lblFechaFin.AutoSize = true;
            panelOpciones.Controls.Add(lblFechaFin);

            DateTimePicker dtpFechaFin = new DateTimePicker();
            dtpFechaFin.Format = DateTimePickerFormat.Short;
            dtpFechaFin.Value = DateTime.Now;
            dtpFechaFin.Location = new Point(300, 198);
            dtpFechaFin.Size = new Size(120, 23);
            panelOpciones.Controls.Add(dtpFechaFin);

            // Botón de generar reporte
            Button btnGenerar = new Button();
            btnGenerar.Text = "Generar Reporte PDF";
            btnGenerar.Location = new Point(220, 350);
            btnGenerar.Size = new Size(150, 30);
            btnGenerar.Click += (sender, e) =>
            {
                try
                {
                    string rutaReporte = "";
                    DateTime fechaInicio = dtpFechaInicio.Value.Date;
                    DateTime fechaFin = dtpFechaFin.Value.Date.AddDays(1).AddSeconds(-1); // Incluir todo el día

                    Cursor = Cursors.WaitCursor;

                    if (rbUsuariosSuscritos.Checked)
                    {
                        rutaReporte = _reportesPdfService.GenerarReporteUsuariosSuscritos(fechaInicio, fechaFin);
                    }
                    else if (rbInteraccionUsuarios.Checked)
                    {
                        rutaReporte = _reportesPdfService.GenerarReporteInteraccionUsuarios(fechaInicio, fechaFin);
                    }
                    else if (rbContenidoPopular.Checked)
                    {
                        rutaReporte = _reportesPdfService.GenerarReporteContenidoPopular(fechaInicio, fechaFin);
                    }

                    Cursor = Cursors.Default;

                    if (!string.IsNullOrEmpty(rutaReporte))
                    {
                        MessageBox.Show($"Reporte generado correctamente en:\n{rutaReporte}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Preguntar si desea abrir el reporte
                        if (MessageBox.Show("¿Desea abrir el reporte ahora?", "Abrir reporte", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            Process.Start(rutaReporte);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Cursor = Cursors.Default;
                    MessageBox.Show($"Error al generar el reporte: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            panelPrincipal.Controls.Add(btnGenerar);
        }

        private void ReportesForm_Load(object sender, EventArgs e)
        {
            // Centrar el formulario en la pantalla
            this.CenterToScreen();
        }
    }
}
