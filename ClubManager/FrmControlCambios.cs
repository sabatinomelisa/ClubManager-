using System;
using System.Windows.Forms;
using BE;
using BLL;
using SERVICIOS;

namespace ClubManager
{
    public class FrmControlCambios : Form
    {
        private readonly ControlCambioBLL controlCambioBLL;
        private TextBox txtIdSocio;
        private TextBox txtIdHistorico;
        private Button btnBuscar;
        private Button btnVolverMail;
        private DataGridView dgvHistorialMail;

        public FrmControlCambios()
        {
            controlCambioBLL = new ControlCambioBLL();
            InicializarControles();
            VisualStyleHelper.AplicarEstiloBase(this);
        }

        private void InicializarControles()
        {
            Text = "Histórico de mail del socio";
            StartPosition = FormStartPosition.CenterScreen;
            Width = 820;
            Height = 500;

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

            Label lblHistorico = new Label();
            lblHistorico.Text = "Id histórico";
            lblHistorico.Left = 310;
            lblHistorico.Top = 20;
            lblHistorico.Width = 90;

            txtIdHistorico = new TextBox();
            txtIdHistorico.Left = 405;
            txtIdHistorico.Top = 16;
            txtIdHistorico.Width = 80;

            btnVolverMail = new Button();
            btnVolverMail.Text = "Volver al mail seleccionado";
            btnVolverMail.Left = 500;
            btnVolverMail.Top = 14;
            btnVolverMail.Width = 200;
            btnVolverMail.Click += btnVolverMail_Click;

            dgvHistorialMail = new DataGridView();
            dgvHistorialMail.Left = 20;
            dgvHistorialMail.Top = 55;
            dgvHistorialMail.Width = 760;
            dgvHistorialMail.Height = 360;
            dgvHistorialMail.ReadOnly = true;
            dgvHistorialMail.AllowUserToAddRows = false;
            dgvHistorialMail.AllowUserToDeleteRows = false;
            dgvHistorialMail.AutoGenerateColumns = true;
            dgvHistorialMail.SelectionChanged += dgvHistorialMail_SelectionChanged;

            Controls.Add(lblSocio);
            Controls.Add(txtIdSocio);
            Controls.Add(btnBuscar);
            Controls.Add(lblHistorico);
            Controls.Add(txtIdHistorico);
            Controls.Add(btnVolverMail);
            Controls.Add(dgvHistorialMail);
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

                dgvHistorialMail.DataSource = controlCambioBLL.ListarHistorialMailSocio(filtro);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnVolverMail_Click(object sender, EventArgs e)
        {
            try
            {
                int idSocio;
                int idHistorico;

                if (!int.TryParse(txtIdSocio.Text.Trim(), out idSocio))
                {
                    MessageBox.Show("Ingresar o seleccionar un id de socio válido.");
                    return;
                }

                if (!int.TryParse(txtIdHistorico.Text.Trim(), out idHistorico))
                {
                    MessageBox.Show("Ingresar o seleccionar un id histórico válido.");
                    return;
                }

                string usuario = SessionManager.SesionIniciada ? SessionManager.ObtenerUsuarioActual().Username : "SIN_SESION";
                controlCambioBLL.VolverAlMailHistorico(idSocio, idHistorico, usuario);
                btnBuscar_Click(sender, e);
                MessageBox.Show("Mail histórico restaurado correctamente.");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvHistorialMail_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvHistorialMail.CurrentRow == null || dgvHistorialMail.CurrentRow.DataBoundItem == null)
            {
                return;
            }

            HistorialBE historial = dgvHistorialMail.CurrentRow.DataBoundItem as HistorialBE;
            if (historial == null)
            {
                return;
            }

            txtIdSocio.Text = historial.IdSocio.ToString();
            txtIdHistorico.Text = historial.IdHistorico.ToString();
        }
    }
}
