using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BE;
using BLL;
using SERVICIOS;
using SERVICIOS.Observer;

namespace ClubManager
{
    public partial class FrmInicio : Form, IOberverIdioma
    {
        private bool cargando = true;

        public FrmInicio()
        {
            InitializeComponent();
        }

        private void FrmInicio_Load(object sender, EventArgs e)
        {
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
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
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
        }

        private void btnOlvidaste_Click(object sender, EventArgs e)
        {
            FrmOlvidaste olvidoPassword = new FrmOlvidaste();
            olvidoPassword.Show();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public void ActualizarIdioma()
        {
            List<TraduccionBE> traducciones = new List<TraduccionBE>();

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
        }

        private void cmbIdiomas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cargando)
            {
                return;
            }

            IdiomaBE idioma = (IdiomaBE)cmbIdiomas.SelectedItem;
            TratamientoIdioma.Instancia.IdiomaActual = idioma;
            TratamientoIdioma.Instancia.Notificar();
        }
    }
}
