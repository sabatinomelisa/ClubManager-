using BE;
using BLL;
using SERVICIOS;
using SERVICIOS.Observer;
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
    public partial class FrmRegistro : Form, IOberverIdioma
    {
        bool cargando = true;
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

            IdiomaBLL idiomaBLL = new IdiomaBLL();

            List<IdiomaBE> idiomas = idiomaBLL.ListarIdiomas();

            cmbIdiomas.DataSource = idiomas;
            cmbIdiomas.DisplayMember = "Nombre";
            cmbIdiomas.ValueMember = "Id";

            cargando = false;
            //Suscribo al Observer para el cambio de idioma
            TratamientoIdioma.Instancia.Suscribir(this);
        }


        public void ActualizarIdioma()
        {
            List<TraduccionBE> traducciones = new List<TraduccionBE>();

            TraduccionBLL tradBLL = new TraduccionBLL();
            int idiomaSel = (int)cmbIdiomas.SelectedValue;


            traducciones = tradBLL.Listar(idiomaSel);

            foreach (var tr in traducciones)
            {
                switch (tr.NombreControl)
                {
                    case "lblTipDoc":
                        lblTipDoc.Text = tr.Traduccion;
                        break;

                    case "lblNroDoc":
                        lblNroDoc.Text = tr.Traduccion;
                        break;

                    case "lblNombre":
                        lblNombre.Text = tr.Traduccion;
                        break;

                    case "lblApellido":
                        lblApellido.Text = tr.Traduccion;
                        break;

                    case "lblMail":
                        lblMail.Text = tr.Traduccion;
                        break;

                    case "lblTelefono":
                        lblTelefono.Text = tr.Traduccion;
                        break;

                    case "lblFecNac":
                        lblFecNac.Text = tr.Traduccion;
                        break;

                    case "lblNacionalidad":
                        lblNacionalidad.Text = tr.Traduccion;
                        break;

                    case "lblUsuario":
                        lblUsuario.Text = tr.Traduccion;
                        break;
                    case "lblContraseña":
                        lblContraseña.Text = tr.Traduccion;
                        break;
                    case "btnRegistrar":
                        btnRegistrar.Text = tr.Traduccion;
                        break;
                    case "btnVolver":
                        btnVolver.Text = tr.Traduccion;
                        break;
                    case "lblIdioma":
                        lblIdioma.Text = tr.Traduccion;
                        break;

                }
            }

        }

        private void cmbIdiomas_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (cargando) return;

            IdiomaBE idioma = (IdiomaBE)cmbIdiomas.SelectedItem;

            TratamientoIdioma.Instancia.IdiomaActual = idioma;
            TratamientoIdioma.Instancia.Notificar();
        }
    }
}
