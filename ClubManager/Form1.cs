using BLL;
using System;
using System.Windows.Forms;

namespace ClubManager
{
    public partial class Form1 : Form
    {
        private readonly BitacoraBLL bitacoraBLL;
        private const string UsuarioPrueba = "prueba";

        public Form1()
        {
            InitializeComponent();
            bitacoraBLL = new BitacoraBLL();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CargarBitacora();
        }

        private void btnRegistrarLogin_Click(object sender, EventArgs e)
        {
            try
            {
                bitacoraBLL.RegistrarLogin(UsuarioPrueba);
                CargarBitacora();
                MessageBox.Show("Login registrado en bitácora.", "Bitácora", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void btnRegistrarLogout_Click(object sender, EventArgs e)
        {
            try
            {
                bitacoraBLL.RegistrarLogout(UsuarioPrueba);
                CargarBitacora();
                MessageBox.Show("Logout registrado en bitácora.", "Bitácora", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            CargarBitacora();
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
                MostrarError(ex);
            }
        }

        private void MostrarError(Exception ex)
        {
            MessageBox.Show(
                "No se pudo operar con la bitácora. Verifique la conexión a SQL Server y que exista la base de datos ClubManagerDB.\n\nDetalle: " + ex.Message,
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}
