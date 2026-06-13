using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            //Mostrar el FrmInicio
            FrmInicio ini = new FrmInicio();
            ini.Show();
        }

        private void btnCambiarPass_Click(object sender, EventArgs e)
        {
            UsuarioBLL usrBLL = new UsuarioBLL();

            bool usrOk = usrBLL.ValidarUsuario(txtUsuario.Text, txtNuevaPass.Text);

            if(usrOk)
            {
                UsuarioBE usrNuevo = new UsuarioBE();
                usrNuevo.Username = txtUsuario.Text;
                usrNuevo.Password = txtNuevaPass.Text;
                int filas = usrBLL.CambiarContraseña(usrNuevo);
                if (filas > 0)
                {
                    lblResultado.Text = "Contraseña Actualizada";
                }else
                {
                    lblResultado.Text = "Error al actualizar la contraseña";
                }
            }

        }
    }
}
