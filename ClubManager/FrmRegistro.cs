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
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
>>>>>>> origin/main

namespace ClubManager
{
    public partial class FrmRegistro : Form, IOberverIdioma
    {
<<<<<<< HEAD
        private bool cargando = true;

=======
        bool cargando = true;
>>>>>>> origin/main
        public FrmRegistro()
        {
            InitializeComponent();
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
<<<<<<< HEAD
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
                usuario.Rol = new RolBE();
                usuario.Rol.Id = 2;

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
=======
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


>>>>>>> origin/main
        }

        private void BlanquearCampos()
        {
<<<<<<< HEAD
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
=======
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
>>>>>>> origin/main
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

        private void FrmRegistro_Load(object sender, EventArgs e)
        {
            lblResultado.Text = string.Empty;
            cmbTipDoc.Text = string.Empty;
            txtNroDoc.Text = string.Empty;
            txtNombre.Text = string.Empty;
            txtApellido.Text = string.Empty;
            txtMail.Text = string.Empty;
<<<<<<< HEAD
            txtTelefono.Text = string.Empty;
=======
            txtTelefono.Text = string.Empty; 
>>>>>>> origin/main
            txtFecNac.Text = string.Empty;
            txtNacionalidad.Text = string.Empty;
            txtUsuario.Text = string.Empty;
            txtPassword.Text = string.Empty;

<<<<<<< HEAD
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

=======
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


>>>>>>> origin/main
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
=======
            TraduccionBLL tradBLL = new TraduccionBLL();
            int idiomaSel = TratamientoIdioma.Instancia.IdiomaActual.Id;

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

>>>>>>> origin/main
        }

        private void cmbIdiomas_SelectedIndexChanged_1(object sender, EventArgs e)
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
