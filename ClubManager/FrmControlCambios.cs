using System;
using System.Windows.Forms;
using BLL;
using SERVICIOS;

namespace ClubManager
{
    public class FrmControlCambios : Form
    {
        private readonly ControlCambioBLL controlCambioBLL;
        private TextBox txtIdSocio;
        private TextBox txtIdCambio;
        private Button btnBuscar;
        private Button btnRecomponer;
        private DataGridView dgvCambios;

        public FrmControlCambios()
        {
            controlCambioBLL = new ControlCambioBLL();
            InicializarControles();
        }

        private void InicializarControles()
        {
            Text = "Control de cambios - Socios";
            StartPosition = FormStartPosition.CenterScreen;
            Width = 900;
            Height = 520;

            Label lblSocio = new Label();
            lblSocio.Text = "Id socio";
            lblSocio.Left = 20;
            lblSocio.Top = 20;
            lblSocio.Width = 70;

            txtIdSocio = new TextBox();
            txtIdSocio.Left = 95;
            txtIdSocio.Top = 16;
            txtIdSocio.Width = 80;

            btnBuscar = new Button();
            btnBuscar.Text = "Buscar";
            btnBuscar.Left = 185;
            btnBuscar.Top = 14;
            btnBuscar.Width = 90;
            btnBuscar.Click += btnBuscar_Click;

            Label lblCambio = new Label();
            lblCambio.Text = "Id cambio";
            lblCambio.Left = 310;
            lblCambio.Top = 20;
            lblCambio.Width = 80;

            txtIdCambio = new TextBox();
            txtIdCambio.Left = 395;
            txtIdCambio.Top = 16;
            txtIdCambio.Width = 80;

            btnRecomponer = new Button();
            btnRecomponer.Text = "Recomponer estado anterior";
            btnRecomponer.Left = 485;
            btnRecomponer.Top = 14;
            btnRecomponer.Width = 190;
            btnRecomponer.Click += btnRecomponer_Click;

            dgvCambios = new DataGridView();
            dgvCambios.Left = 20;
            dgvCambios.Top = 55;
            dgvCambios.Width = 840;
            dgvCambios.Height = 390;
            dgvCambios.ReadOnly = true;
            dgvCambios.AllowUserToAddRows = false;
            dgvCambios.AllowUserToDeleteRows = false;
            dgvCambios.AutoGenerateColumns = true;

            Controls.Add(lblSocio);
            Controls.Add(txtIdSocio);
            Controls.Add(btnBuscar);
            Controls.Add(lblCambio);
            Controls.Add(txtIdCambio);
            Controls.Add(btnRecomponer);
            Controls.Add(dgvCambios);
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                int idSocio;
                int? filtro = null;

                if (int.TryParse(txtIdSocio.Text.Trim(), out idSocio))
                {
                    filtro = idSocio;
                }

                dgvCambios.DataSource = controlCambioBLL.ListarCambiosSocio(filtro);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRecomponer_Click(object sender, EventArgs e)
        {
            try
            {
                int idCambioSocio;
                if (!int.TryParse(txtIdCambio.Text.Trim(), out idCambioSocio))
                {
                    MessageBox.Show("Ingresar un id de cambio válido.");
                    return;
                }

                string usuario = SessionManager.SesionIniciada ? SessionManager.ObtenerUsuarioActual().Username : "SIN_SESION";
                controlCambioBLL.RecomponerEstadoAnterior(idCambioSocio, usuario);
                btnBuscar_Click(sender, e);
                MessageBox.Show("Estado anterior recompuesto correctamente.");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
