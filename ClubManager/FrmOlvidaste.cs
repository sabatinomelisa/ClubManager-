using System;
using System.Windows.Forms;
using BLL;

namespace ClubManager
{
    public partial class FrmOlvidaste : Form
    {
        public FrmOlvidaste()
        {
            InitializeComponent();
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            FrmInicio inicio = new FrmInicio();
            inicio.Show();
            Close();
        }

        private void btnCambiarPass_Click(object sender, EventArgs e)
        {
            try
            {
                UsuarioBLL usuarioBLL = new UsuarioBLL();
                int filas = usuarioBLL.CambiarContraseña(txtUsuario.Text.Trim(), txtViejaPass.Text, txtNuevaPass.Text);

                if (filas > 0)
                {
                    lblResultado.Text = "Contraseña actualizada.";
                    txtViejaPass.Text = string.Empty;
                    txtNuevaPass.Text = string.Empty;
                }
                else
                {
                    lblResultado.Text = "No se actualizó la contraseña.";
                }
            }
            catch (Exception exception)
            {
                lblResultado.Text = exception.Message;
            }
        }
    }
}
