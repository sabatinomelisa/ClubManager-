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
        private string claveUltimoMensaje = string.Empty;
        private string mensajeOriginal = string.Empty;

        public FrmInicio()
        {
            InitializeComponent();
            PasswordVisibilityHelper.AgregarBoton(this, txtPassword);
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

                MessageBox.Show(ObtenerTraduccionActual("msgLoginExitoso", "Login exitoso."));

                Hide();
                FrmMenu menu = new FrmMenu();
                menu.FormClosed += delegate
                {
                    txtPassword.Text = string.Empty;
                    Show();
                    Activate();
                };
                menu.Show();
            }
            catch (Exception exception)
            {
                MostrarMensajeError(exception.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AbrirFormularioHijo(new FrmRegistro());
        }

        private void btnOlvidaste_Click(object sender, EventArgs e)
        {
            AbrirFormularioHijo(new FrmOlvidaste());
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void AbrirFormularioHijo(Form formulario)
        {
            formulario.StartPosition = FormStartPosition.CenterScreen;
            formulario.FormClosed += delegate
            {
                Show();
                Activate();
            };

            Hide();
            formulario.Show();
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

            ActualizarMensajeVisible(traducciones);
        }

        private void MostrarMensajeError(string mensaje)
        {
            mensajeOriginal = mensaje;
            claveUltimoMensaje = ObtenerClaveMensaje(mensaje);

            if (!string.IsNullOrWhiteSpace(claveUltimoMensaje))
            {
                lblMensaje.Text = ObtenerTraduccionActual(claveUltimoMensaje, mensajeOriginal);
            }
            else
            {
                lblMensaje.Text = mensajeOriginal;
            }
        }

        private string ObtenerClaveMensaje(string mensaje)
        {
            if (string.IsNullOrWhiteSpace(mensaje))
            {
                return string.Empty;
            }

            if (mensaje.StartsWith("Error de integridad en login", StringComparison.OrdinalIgnoreCase))
            {
                return "msgErrorIntegridadLogin";
            }

            if (mensaje.Equals("Usuario inexistente.", StringComparison.OrdinalIgnoreCase))
            {
                return "msgUsuarioInexistente";
            }

            if (mensaje.Equals("Contraseña incorrecta.", StringComparison.OrdinalIgnoreCase))
            {
                return "msgPasswordIncorrecta";
            }

            if (mensaje.StartsWith("Contraseña incorrecta. La cuenta fue bloqueada", StringComparison.OrdinalIgnoreCase))
            {
                return "msgPasswordBloqueada";
            }

            return string.Empty;
        }

        private void ActualizarMensajeVisible(List<TraduccionBE> traducciones)
        {
            if (string.IsNullOrWhiteSpace(claveUltimoMensaje) || string.IsNullOrWhiteSpace(lblMensaje.Text))
            {
                return;
            }

            foreach (TraduccionBE traduccion in traducciones)
            {
                if (traduccion.NombreControl == claveUltimoMensaje)
                {
                    lblMensaje.Text = traduccion.Traduccion;
                    return;
                }
            }

            lblMensaje.Text = ObtenerTraduccionFallback(claveUltimoMensaje, mensajeOriginal);
        }

        private string ObtenerTraduccionActual(string clave, string valorDefault)
        {
            try
            {
                if (TratamientoIdioma.Instancia.IdiomaActual == null)
                {
                    return ObtenerTraduccionFallback(clave, valorDefault);
                }

                TraduccionBLL traduccionBLL = new TraduccionBLL();
                List<TraduccionBE> traducciones = traduccionBLL.Listar(TratamientoIdioma.Instancia.IdiomaActual.Id);

                foreach (TraduccionBE traduccion in traducciones)
                {
                    if (traduccion.NombreControl == clave)
                    {
                        return traduccion.Traduccion;
                    }
                }
            }
            catch
            {
            }

            return ObtenerTraduccionFallback(clave, valorDefault);
        }

        private string ObtenerTraduccionFallback(string clave, string valorDefault)
        {
            bool idiomaIngles = false;

            if (TratamientoIdioma.Instancia.IdiomaActual != null)
            {
                idiomaIngles = TratamientoIdioma.Instancia.IdiomaActual.Id == 2 ||
                               string.Equals(TratamientoIdioma.Instancia.IdiomaActual.Nombre, "English", StringComparison.OrdinalIgnoreCase);
            }

            if (idiomaIngles)
            {
                switch (clave)
                {
                    case "msgLoginExitoso":
                        return "Login successful.";
                    case "msgErrorIntegridadLogin":
                        return "Integrity error on login: The record was modified or altered outside the system.";
                    case "msgUsuarioInexistente":
                        return "User does not exist.";
                    case "msgPasswordIncorrecta":
                        return "Incorrect password.";
                    case "msgPasswordBloqueada":
                        return "Incorrect password. The account was blocked after 3 failed attempts.";
                }
            }

            switch (clave)
            {
                case "msgLoginExitoso":
                    return "Login exitoso.";
                case "msgErrorIntegridadLogin":
                    return "Error de integridad en login: El registro fue modificado o alterado por fuera del sistema.";
                case "msgUsuarioInexistente":
                    return "Usuario inexistente.";
                case "msgPasswordIncorrecta":
                    return "Contraseña incorrecta.";
                case "msgPasswordBloqueada":
                    return "Contraseña incorrecta. La cuenta fue bloqueada por superar los 3 intentos fallidos.";
            }

            return valorDefault;
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
