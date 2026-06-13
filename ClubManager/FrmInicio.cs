using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClubManager
{
    public partial class FrmInicio : Form
    {
        public FrmInicio()
        {
            InitializeComponent();
        }

        private void FrmInicio_Load(object sender, EventArgs e)
        {
            lblMensaje.Text=string.Empty;
            txtPassword.Text=string.Empty;
            txtUsername.Text=string.Empty;
        }

            private void button1_Click(object sender, EventArgs e)
            {
                UsuarioBE userActual =new UsuarioBE();

                userActual.Username = txtUsername.Text;
                userActual.Password = txtPassword.Text;
                /*agregarbasededtos*/

                try
                {
                    SessionManager.Login(userActual);
                    MessageBox.Show("Login Exitoso");
                    //Mostrar el FrmRegistro
                    FrmMenu reg = new FrmMenu();
                    reg.Show();



            }
            catch (Exception ex)
                {
                    lblMensaje.Text = ex.Message;
                }
          
            
            }

        //Boton Registrar
        private void button2_Click(object sender, EventArgs e)
        {
            //Mostrar el FrmRegistro
            FrmRegistro reg = new FrmRegistro();
            reg.Show();

        }

        private void btnOlvidaste_Click(object sender, EventArgs e)
        {
            //Mostrar el FrmOlvidaste
            FrmOlvidaste olv = new FrmOlvidaste();
            olv.Show();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
