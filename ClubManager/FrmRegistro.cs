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
                usr.Mail=txtMail.Text;
                usr.Telefono = int.Parse(txtTelefono.Text);

                UsuarioBLL usrBLL = new UsuarioBLL();
                int resultado = usrBLL.AltaUsuario(usr);
                if(resultado > 0)
                {
                    lblResultado.Text = "Alta Exitosa";
                    BlanquearCampos();
                }
                
            }catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error en el Resgistro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


        }

        private void BlanquearCampos()
        {
            cmbTipDoc.Items.Clear();
            txtNombre.Text = string.Empty;
            txtApellido.Text = string.Empty;
            txtFecNac.Text= string.Empty;
            txtNacionalidad.Text=string.Empty;
            txtNroDoc.Text= string.Empty;
            txtUsuario.Text=string.Empty;
            txtPassword.Text= string.Empty;
            txtMail.Text= string.Empty;
            txtTelefono.Text= string.Empty;
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            //Mostrar el FrmInicio
            FrmInicio ini = new FrmInicio();
            ini.Show();
        }
    }
}
