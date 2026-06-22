using BE;
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

namespace ClubManager
{
    public partial class FrmInicio : Form, IOberverIdioma
    {
        bool cargando = true;
        public FrmInicio()
        {
            InitializeComponent();
        }

        private void FrmInicio_Load(object sender, EventArgs e)
        {
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
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
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

        }

        private void btnOlvidaste_Click(object sender, EventArgs e)
        {
            //Mostrar el FrmOlvidaste
            FrmOlvidaste olv = new FrmOlvidaste();
            olv.Show();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            //Salir de la Aplicación
            Application.Exit();
        }

        public void ActualizarIdioma()
        {
            List<TraduccionBE> traducciones = new List<TraduccionBE>();

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

        }

        private void cmbIdiomas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cargando) return;

            IdiomaBE idioma = (IdiomaBE)cmbIdiomas.SelectedItem;

            TratamientoIdioma.Instancia.IdiomaActual = idioma;
            TratamientoIdioma.Instancia.Notificar();
        }
    }
}
