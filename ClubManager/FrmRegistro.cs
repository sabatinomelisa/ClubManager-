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
    public partial class FrmRegistro : Form
    {
        public FrmRegistro()
        {
            InitializeComponent();
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                UsuarioBE usr = new UsuarioBE();

                usr.TipoDocumento = (cmbTipDoc.Text).ToString();
                usr.NumeroDocumento = int.Parse(txtNroDoc.Text);
                usr.Nombre = txtNombre.Text;
                usr.Apellido = txtApellido.Text;
                usr.FechaNacimiento = DateTime.Parse(txtFecNac.Text);
                usr.Nacionalidad = txtNacionalidad.Text;
                usr.Username = txtUsuario.Text;
                usr.Password = txtPassword.Text;
                usr.Bloqueado = "N";
                usr.FechaCreacion = DateTime.Now;

                UsuarioBLL usrBLL = new UsuarioBLL();
                int resultado = usrBLL.AltaUsuario(usr);
                if(resultado > 0)
                {
                    lblResultado.Text = "Alta Exitosa";
                }
                
            }catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error en el Resgistro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


        }
    }
}
