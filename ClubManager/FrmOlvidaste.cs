using System;
using System.Drawing;
using System.Windows.Forms;
using BLL;

namespace ClubManager
{
    public partial class FrmOlvidaste : Form
    {
        public FrmOlvidaste()
        {
            InitializeComponent();
            ConfigurarMensajeResultado();
            PasswordVisibilityHelper.AgregarBoton(this, txtViejaPass);
            PasswordVisibilityHelper.AgregarBoton(this, txtNuevaPass);
        }

        private void ConfigurarMensajeResultado()
        {
            lblResultado.AutoSize = false;
            lblResultado.BackColor = Color.Transparent;
            lblResultado.ForeColor = Color.White;
            lblResultado.Width = 650;
            lblResultado.Height = 55;
            lblResultado.Left = 307;
            lblResultado.Top = 394;
            lblResultado.TextAlign = ContentAlignment.MiddleLeft;
            lblResultado.BringToFront();
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
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
