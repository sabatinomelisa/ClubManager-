<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BE;
using BLL;
using SERVICIOS;
using SERVICIOS.Observer;
=======
﻿using BE;
using BLL;
using SERVICIOS;
using SERVICIOS.Observer;
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
>>>>>>> origin/main

namespace ClubManager
{
    public partial class FrmInicio : Form, IOberverIdioma
    {
<<<<<<< HEAD
        private bool cargando = true;

=======
        bool cargando = true;
>>>>>>> origin/main
        public FrmInicio()
        {
            InitializeComponent();
        }

        private void FrmInicio_Load(object sender, EventArgs e)
        {
<<<<<<< HEAD
            lblMensaje.Text = string.Empty;

            try
            {
                IdiomaBLL idiomaBLL = new IdiomaBLL();
                List<IdiomaBE> idiomas = idiomaBLL.ListarIdiomas();

                cmbIdiomas.DataSource = idiomas;
                cmbIdiomas.DisplayMember = "Nombre";
                cmbIdiomas.ValueMember = "Id";

                cargando = false;
                TratamientoIdioma.Instancia.Suscribir(this);

                if (TratamientoIdioma.Instancia.IdiomaActual != null)
                {
                    cmbIdiomas.SelectedValue = TratamientoIdioma.Instancia.IdiomaActual.Id;
                    ActualizarIdioma();
                }
            }
            catch (Exception exception)
            {
                lblMensaje.Text = "No se pudieron cargar los idiomas: " + exception.Message;
=======
            lblMensaje.Text=string.Empty;
            txtPassword.Text=string.Empty;
            txtUsername.Text=string.Empty;

            IdiomaBLL idiomaBLL = new IdiomaBLL();

            List<IdiomaBE> idiomas = idiomaBLL.ListarIdiomas();

            cmbIdiomas.DataSource = idiomas;
            cmbIdiomas.DisplayMember = "Nombre";
            cmbIdiomas.ValueMember = "Id";

            cargando = false;
            //Suscribo al Observer para el cambio de idioma
            TratamientoIdioma.Instancia.Suscribir(this);

            if (TratamientoIdioma.Instancia.IdiomaActual != null)
            {
                cmbIdiomas.SelectedValue =
                    TratamientoIdioma.Instancia.IdiomaActual.Id;

                ActualizarIdioma();
>>>>>>> origin/main
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
<<<<<<< HEAD
            try
            {
                UsuarioBLL usuarioBLL = new UsuarioBLL();
                usuarioBLL.Login(txtUsername.Text.Trim(), txtPassword.Text);

                MessageBox.Show("Login exitoso.");

                FrmMenu menu = new FrmMenu();
                menu.Show();
                Hide();
            }
            catch (Exception exception)
            {
                lblMensaje.Text = exception.Message;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FrmRegistro registro = new FrmRegistro();
            registro.Show();
=======
             try
             {
                if (txtUsername.Text != string.Empty && txtPassword.Text != string.Empty)
                {

                    UsuarioBE userActual = new UsuarioBE();
                    userActual.Username = txtUsername.Text;
                    userActual.Password = txtPassword.Text;
                    UsuarioBLL usrBLL = new UsuarioBLL();
                    //Valido si la contraseña es correcta
                    bool usrOk = usrBLL.ValidarUsuario(userActual.Username, userActual.Password);

                    if (usrOk)
                    {
                        SessionManager.Login(userActual);
                        MessageBox.Show("Login Exitoso");
                        //Mostrar el FrmMenu
                        FrmMenu reg = new FrmMenu();
                        reg.Show();
                    }
                    else
                    {
                        lblMensaje.Text = "Usuario y/o contraseña incorrectas";
                    }
                }else
                {
                    lblMensaje.Text = "Ingrese Usuario y Contraseña";
                }

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

>>>>>>> origin/main
        }

        private void btnOlvidaste_Click(object sender, EventArgs e)
        {
<<<<<<< HEAD
            FrmOlvidaste olvidoPassword = new FrmOlvidaste();
            olvidoPassword.Show();
=======
            //Mostrar el FrmOlvidaste
            FrmOlvidaste olv = new FrmOlvidaste();
            olv.Show();
>>>>>>> origin/main
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
<<<<<<< HEAD
=======
            //Salir de la Aplicación
>>>>>>> origin/main
            Application.Exit();
        }

        public void ActualizarIdioma()
        {
            List<TraduccionBE> traducciones = new List<TraduccionBE>();

<<<<<<< HEAD
            TraduccionBLL traduccionBLL = new TraduccionBLL();
            int idiomaSeleccionado = TratamientoIdioma.Instancia.IdiomaActual.Id;

            traducciones = traduccionBLL.Listar(idiomaSeleccionado);

            foreach (TraduccionBE traduccion in traducciones)
            {
                switch (traduccion.NombreControl)
                {
                    case "lblUsuario":
                        lblUsuario.Text = traduccion.Traduccion;
                        break;
                    case "lblContraseña":
                        lblContraseña.Text = traduccion.Traduccion;
                        break;
                    case "btnIngresar":
                        btnIngresar.Text = traduccion.Traduccion;
                        break;
                    case "btnRegistrar":
                        btnRegistrar.Text = traduccion.Traduccion;
                        break;
                    case "btnOlvidaste":
                        btnOlvidaste.Text = traduccion.Traduccion;
                        break;
                    case "btnSalir":
                        btnSalir.Text = traduccion.Traduccion;
                        break;
                    case "lblIdioma":
                        lblIdioma.Text = traduccion.Traduccion;
                        break;
                }
            }
=======
            TraduccionBLL tradBLL = new TraduccionBLL();
            int idiomaSel = TratamientoIdioma.Instancia.IdiomaActual.Id;

            traducciones = tradBLL.Listar(idiomaSel);

            foreach(var tr in traducciones)
            {
                switch (tr.NombreControl)
                {
                    case "lblUsuario":
                        lblUsuario.Text = tr.Traduccion;
                        break;

                    case "lblContraseña":
                        lblContraseña.Text = tr.Traduccion;
                        break;

                    case "btnIngresar":
                        btnIngresar.Text = tr.Traduccion;
                        break;

                    case "btnRegistrar":
                        btnRegistrar.Text = tr.Traduccion;
                        break;

                    case "btnOlvidaste":
                        btnOlvidaste.Text = tr.Traduccion;
                        break;

                    case "btnSalir":
                        btnSalir.Text = tr.Traduccion;
                        break;

                    case "lblIdioma":
                        lblIdioma.Text = tr.Traduccion;
                        break;


                }
            }

>>>>>>> origin/main
        }

        private void cmbIdiomas_SelectedIndexChanged(object sender, EventArgs e)
        {
<<<<<<< HEAD
            if (cargando)
            {
                return;
            }

            IdiomaBE idioma = (IdiomaBE)cmbIdiomas.SelectedItem;
=======
            if (cargando) return;

            IdiomaBE idioma = (IdiomaBE)cmbIdiomas.SelectedItem;

>>>>>>> origin/main
            TratamientoIdioma.Instancia.IdiomaActual = idioma;
            TratamientoIdioma.Instancia.Notificar();
        }
    }
}
