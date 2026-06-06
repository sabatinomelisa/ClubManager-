using System;
using System.Windows.Forms;
using BLL;

namespace ClubManager
{
    public partial class FrmBitacora : Form
    {
        private readonly BitacoraBLL bitacoraBLL;

        public FrmBitacora()
        {
            InitializeComponent();
            bitacoraBLL = new BitacoraBLL();
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarBitacora()
        {
            try
            {
                dgvBitacora.AutoGenerateColumns = true;
                dgvBitacora.DataSource = bitacoraBLL.Listar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
