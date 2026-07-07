using System;
using System.Windows.Forms;
using BLL;

namespace ClubManager
{
    public partial class FrmBitacora : Form
    {
        private readonly BitacoraBLL bitacoraBLL;
        private TextBox txtFiltroUsuario;
        private TextBox txtFiltroAccion;
        private TextBox txtFiltroModulo;
        private Button btnBuscar;
        private Button btnLimpiarFiltros;

        public FrmBitacora()
        {
            InitializeComponent();
            bitacoraBLL = new BitacoraBLL();
            ConfigurarFiltros();
        }

        private void FrmBitacora_Load(object sender, EventArgs e)
        {
            CargarBitacora();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            CargarBitacora();
        }

        private void btnRegistrarPrueba_Click(object sender, EventArgs e)
        {
            try
            {
                bitacoraBLL.Registrar("prueba", "PRUEBA", "Bitacora", "Registro de prueba generado desde FrmBitacora.");
                CargarBitacora();
                MessageBox.Show("Registro creado correctamente.");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                dgvBitacora.AutoGenerateColumns = true;
                dgvBitacora.DataSource = bitacoraBLL.Buscar(
                    ObtenerFiltro(txtFiltroUsuario.Text),
                    ObtenerFiltro(txtFiltroAccion.Text),
                    ObtenerFiltro(txtFiltroModulo.Text),
                    null,
                    null);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLimpiarFiltros_Click(object sender, EventArgs e)
        {
            txtFiltroUsuario.Text = string.Empty;
            txtFiltroAccion.Text = string.Empty;
            txtFiltroModulo.Text = string.Empty;
            CargarBitacora();
        }

        private void CargarBitacora()
        {
            try
            {
                dgvBitacora.AutoGenerateColumns = true;
                dgvBitacora.DataSource = bitacoraBLL.Listar();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarFiltros()
        {
            Label lblUsuario = new Label();
            lblUsuario.Text = "Usuario";
            lblUsuario.Left = 12;
            lblUsuario.Top = 68;
            lblUsuario.Width = 70;

            txtFiltroUsuario = new TextBox();
            txtFiltroUsuario.Left = 84;
            txtFiltroUsuario.Top = 65;
            txtFiltroUsuario.Width = 120;

            Label lblAccion = new Label();
            lblAccion.Text = "Acción";
            lblAccion.Left = 214;
            lblAccion.Top = 68;
            lblAccion.Width = 70;

            txtFiltroAccion = new TextBox();
            txtFiltroAccion.Left = 286;
            txtFiltroAccion.Top = 65;
            txtFiltroAccion.Width = 120;

            Label lblModulo = new Label();
            lblModulo.Text = "Módulo";
            lblModulo.Left = 416;
            lblModulo.Top = 68;
            lblModulo.Width = 70;

            txtFiltroModulo = new TextBox();
            txtFiltroModulo.Left = 488;
            txtFiltroModulo.Top = 65;
            txtFiltroModulo.Width = 120;

            btnBuscar = new Button();
            btnBuscar.Text = "Buscar";
            btnBuscar.Left = 618;
            btnBuscar.Top = 63;
            btnBuscar.Width = 75;
            btnBuscar.Click += btnBuscar_Click;

            btnLimpiarFiltros = new Button();
            btnLimpiarFiltros.Text = "Limpiar";
            btnLimpiarFiltros.Left = 699;
            btnLimpiarFiltros.Top = 63;
            btnLimpiarFiltros.Width = 75;
            btnLimpiarFiltros.Click += btnLimpiarFiltros_Click;

            dgvBitacora.Top = 96;
            dgvBitacora.Height = 353;

            Controls.Add(lblUsuario);
            Controls.Add(txtFiltroUsuario);
            Controls.Add(lblAccion);
            Controls.Add(txtFiltroAccion);
            Controls.Add(lblModulo);
            Controls.Add(txtFiltroModulo);
            Controls.Add(btnBuscar);
            Controls.Add(btnLimpiarFiltros);
        }

        private string ObtenerFiltro(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
            {
                return null;
            }

            return valor.Trim();
        }
    }
}
