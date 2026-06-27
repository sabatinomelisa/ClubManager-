using System;
using System.Windows.Forms;
using BLL;

namespace ClubManager
{
    public class FrmIntegridad : Form
    {
        private readonly IntegridadBLL integridadBLL;
        private DataGridView dgvResultados;
        private Button btnVerificar;
        private Button btnRecalcular;
        private Button btnCerrar;
        private Label lblTitulo;

        public FrmIntegridad()
        {
            integridadBLL = new IntegridadBLL();
            InicializarControles();
        }

        private void InicializarControles()
        {
            Text = "Dígitos verificadores";
            StartPosition = FormStartPosition.CenterScreen;
            Width = 820;
            Height = 500;

            lblTitulo = new Label();
            lblTitulo.Text = "Verificación de integridad - Entidad Socio";
            lblTitulo.Left = 20;
            lblTitulo.Top = 15;
            lblTitulo.Width = 500;
            lblTitulo.Height = 25;

            btnVerificar = new Button();
            btnVerificar.Text = "Verificar";
            btnVerificar.Left = 20;
            btnVerificar.Top = 50;
            btnVerificar.Width = 110;
            btnVerificar.Click += btnVerificar_Click;

            btnRecalcular = new Button();
            btnRecalcular.Text = "Recalcular DV";
            btnRecalcular.Left = 140;
            btnRecalcular.Top = 50;
            btnRecalcular.Width = 130;
            btnRecalcular.Click += btnRecalcular_Click;

            btnCerrar = new Button();
            btnCerrar.Text = "Cerrar";
            btnCerrar.Left = 680;
            btnCerrar.Top = 50;
            btnCerrar.Width = 100;
            btnCerrar.Click += btnCerrar_Click;

            dgvResultados = new DataGridView();
            dgvResultados.Left = 20;
            dgvResultados.Top = 90;
            dgvResultados.Width = 760;
            dgvResultados.Height = 340;
            dgvResultados.ReadOnly = true;
            dgvResultados.AllowUserToAddRows = false;
            dgvResultados.AllowUserToDeleteRows = false;
            dgvResultados.AutoGenerateColumns = true;

            Controls.Add(lblTitulo);
            Controls.Add(btnVerificar);
            Controls.Add(btnRecalcular);
            Controls.Add(btnCerrar);
            Controls.Add(dgvResultados);
        }

        private void btnVerificar_Click(object sender, EventArgs e)
        {
            Verificar();
        }

        private void btnRecalcular_Click(object sender, EventArgs e)
        {
            try
            {
                integridadBLL.RecalcularIntegridad();
                Verificar();
                MessageBox.Show("Dígitos verificadores recalculados correctamente.");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Verificar()
        {
            try
            {
                dgvResultados.DataSource = integridadBLL.VerificarIntegridad();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
