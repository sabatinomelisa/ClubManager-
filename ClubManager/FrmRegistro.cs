using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BE;
using BLL;
using SERVICIOS;
using SERVICIOS.Observer;

namespace ClubManager
{
    public partial class FrmRegistro : Form, IOberverIdioma
    {
        private bool cargando = true;

        public FrmRegistro()
        {
            InitializeComponent();
            PasswordVisibilityHelper.AgregarBoton(this, txtPassword);
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                UsuarioBE usuario = new UsuarioBE();
                usuario.TipoDocumento = cmbTipDoc.Text.Trim();
                usuario.NumeroDocumento = Convert.ToInt32(txtNroDoc.Text.Trim());
                usuario.Nombre = txtNombre.Text.Trim();
                usuario.Apellido = txtApellido.Text.Trim();
                usuario.FechaNacimiento = Convert.ToDateTime(txtFecNac.Text.Trim());
                usuario.Nacionalidad = txtNacionalidad.Text.Trim();
                usuario.Username = txtUsuario.Text.Trim();
                usuario.Password = txtPassword.Text;
                usuario.Bloqueado = "N";
                usuario.Activo = "S";
                usuario.FechaCreacion = DateTime.Now;
                usuario.Mail = txtMail.Text.Trim();
                usuario.Telefono = Convert.ToInt32(txtTelefono.Text.Trim());
                usuario.IdRol = 2;

                UsuarioBLL usuarioBLL = new UsuarioBLL();
                int resultado = usuarioBLL.AltaUsuario(usuario);

                if (resultado > 0)
                {
                    lblResultado.Text = "Alta exitosa.";
                    BlanquearCampos();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error en el registro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BlanquearCampos()
        {
            cmbTipDoc.Text = string.Empty;
            txtNombre.Text = string.Empty;
            txtApellido.Text = string.Empty;
            txtFecNac.Text = string.Empty;
            txtNacionalidad.Text = string.Empty;
            txtNroDoc.Text = string.Empty;
            txtUsuario.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtMail.Text = string.Empty;
            txtTelefono.Text = string.Empty;
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FrmRegistro_Load(object sender, EventArgs e)
        {
            lblResultado.Text = string.Empty;
            cmbTipDoc.Text = string.Empty;
            txtNroDoc.Text = string.Empty;
            txtNombre.Text = string.Empty;
            txtApellido.Text = string.Empty;
            txtMail.Text = string.Empty;
            txtTelefono.Text = string.Empty;
            txtFecNac.Text = string.Empty;
            txtNacionalidad.Text = string.Empty;
            txtUsuario.Text = string.Empty;
            txtPassword.Text = string.Empty;

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
            catch
            {
                cargando = false;
            }
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
                    case "lblTipDoc":
                        lblTipDoc.Text = traduccion.Traduccion;
                        break;
                    case "lblNroDoc":
                        lblNroDoc.Text = traduccion.Traduccion;
                        break;
                    case "lblNombre":
                        lblNombre.Text = traduccion.Traduccion;
                        break;
                    case "lblApellido":
                        lblApellido.Text = traduccion.Traduccion;
                        break;
                    case "lblMail":
                        lblMail.Text = traduccion.Traduccion;
                        break;
                    case "lblTelefono":
                        lblTelefono.Text = traduccion.Traduccion;
                        break;
                    case "lblFecNac":
                        lblFecNac.Text = traduccion.Traduccion;
                        break;
                    case "lblNacionalidad":
                        lblNacionalidad.Text = traduccion.Traduccion;
                        break;
                    case "lblUsuario":
                        lblUsuario.Text = traduccion.Traduccion;
                        break;
                    case "lblContraseña":
                        lblContraseña.Text = traduccion.Traduccion;
                        break;
                    case "btnRegistrar":
                        btnRegistrar.Text = traduccion.Traduccion;
                        break;
                    case "btnVolver":
                        btnVolver.Text = traduccion.Traduccion;
                        break;
                    case "lblIdioma":
                        lblIdioma.Text = traduccion.Traduccion;
                        break;
                }
            }
        }

        private void cmbIdiomas_SelectedIndexChanged_1(object sender, EventArgs e)
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
