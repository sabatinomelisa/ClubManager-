<<<<<<< HEAD
using System;
using System.Windows.Forms;
using BLL;
=======
﻿using BE;
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
>>>>>>> origin/main

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
<<<<<<< HEAD
            FrmInicio inicio = new FrmInicio();
            inicio.Show();
            Close();
=======
            //Mostrar el FrmInicio
            FrmInicio ini = new FrmInicio();
            ini.Show();
>>>>>>> origin/main
        }

        private void btnCambiarPass_Click(object sender, EventArgs e)
        {
<<<<<<< HEAD
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
=======
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

>>>>>>> origin/main
        }
    }
}
