using System;
using System.Windows.Forms;
using BE;
using BLL;
using SERVICIOS;

namespace ClubManager
{
    public class FrmSocios : Form
    {
        private readonly SocioBLL socioBLL;
        private DataGridView dgvSocios;
        private TextBox txtIdSocio;
        private TextBox txtTipoDocumento;
        private TextBox txtNumeroDocumento;
        private TextBox txtNombre;
        private TextBox txtApellido;
        private TextBox txtFechaNacimiento;
        private TextBox txtNacionalidad;
        private TextBox txtMail;
        private TextBox txtTelefono;
        private ComboBox cmbActivo;

        public FrmSocios()
        {
            socioBLL = new SocioBLL();
            InicializarControles();
            CargarSocios();
        }

        private void InicializarControles()
        {
            Text = "Gestión de socios";
            StartPosition = FormStartPosition.CenterScreen;
            Width = 1050;
            Height = 620;

            dgvSocios = new DataGridView();
            dgvSocios.Left = 20;
            dgvSocios.Top = 210;
            dgvSocios.Width = 980;
            dgvSocios.Height = 330;
            dgvSocios.ReadOnly = true;
            dgvSocios.AllowUserToAddRows = false;
            dgvSocios.AllowUserToDeleteRows = false;
            dgvSocios.AutoGenerateColumns = true;
            dgvSocios.SelectionChanged += dgvSocios_SelectionChanged;

            txtIdSocio = CrearTextBox("Id", 20, 35, true);
            txtTipoDocumento = CrearTextBox("Tipo documento", 130, 35, false);
            txtNumeroDocumento = CrearTextBox("Número documento", 260, 35, false);
            txtNombre = CrearTextBox("Nombre", 420, 35, false);
            txtApellido = CrearTextBox("Apellido", 580, 35, false);
            txtFechaNacimiento = CrearTextBox("Fecha nac. yyyy-mm-dd", 740, 35, false);
            txtNacionalidad = CrearTextBox("Nacionalidad", 20, 105, false);
            txtMail = CrearTextBox("Mail", 180, 105, false);
            txtTelefono = CrearTextBox("Teléfono", 420, 105, false);

            Label lblActivo = new Label();
            lblActivo.Text = "Activo";
            lblActivo.Left = 580;
            lblActivo.Top = 82;
            lblActivo.Width = 120;
            cmbActivo = new ComboBox();
            cmbActivo.Left = 580;
            cmbActivo.Top = 105;
            cmbActivo.Width = 80;
            cmbActivo.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbActivo.Items.Add("S");
            cmbActivo.Items.Add("N");
            cmbActivo.SelectedIndex = 0;

            Button btnNuevo = CrearBoton("Nuevo", 20, 160);
            btnNuevo.Click += btnNuevo_Click;
            Button btnGuardar = CrearBoton("Guardar", 120, 160);
            btnGuardar.Click += btnGuardar_Click;
            Button btnBaja = CrearBoton("Baja lógica", 220, 160);
            btnBaja.Click += btnBaja_Click;
            Button btnActualizar = CrearBoton("Actualizar", 340, 160);
            btnActualizar.Click += btnActualizar_Click;

            Controls.Add(lblActivo);
            Controls.Add(cmbActivo);
            Controls.Add(btnNuevo);
            Controls.Add(btnGuardar);
            Controls.Add(btnBaja);
            Controls.Add(btnActualizar);
            Controls.Add(dgvSocios);
        }

        private TextBox CrearTextBox(string etiqueta, int left, int top, bool soloLectura)
        {
            Label label = new Label();
            label.Text = etiqueta;
            label.Left = left;
            label.Top = top - 23;
            label.Width = 150;
            Controls.Add(label);

            TextBox textBox = new TextBox();
            textBox.Left = left;
            textBox.Top = top;
            textBox.Width = 140;
            textBox.ReadOnly = soloLectura;
            Controls.Add(textBox);
            return textBox;
        }

        private Button CrearBoton(string texto, int left, int top)
        {
            Button boton = new Button();
            boton.Text = texto;
            boton.Left = left;
            boton.Top = top;
            boton.Width = 100;
            return boton;
        }

        private void CargarSocios()
        {
            try
            {
                dgvSocios.DataSource = socioBLL.ListarSocios(true);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                SocioBE socio = ObtenerSocioDesdePantalla();
                string usuario = ObtenerUsuarioActual();

                if (socio.IdSocio > 0)
                {
                    socioBLL.ModificarSocio(socio, usuario);
                }
                else
                {
                    socioBLL.AltaSocio(socio, usuario);
                }

                CargarSocios();
                LimpiarCampos();
                MessageBox.Show("Operación realizada correctamente.");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBaja_Click(object sender, EventArgs e)
        {
            try
            {
                int idSocio;
                if (!int.TryParse(txtIdSocio.Text.Trim(), out idSocio))
                {
                    MessageBox.Show("Seleccionar un socio.");
                    return;
                }

                socioBLL.BajaSocio(idSocio, ObtenerUsuarioActual());
                CargarSocios();
                MessageBox.Show("Socio dado de baja lógicamente.");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            CargarSocios();
        }

        private void dgvSocios_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvSocios.CurrentRow == null || dgvSocios.CurrentRow.DataBoundItem == null)
            {
                return;
            }

            SocioBE socio = dgvSocios.CurrentRow.DataBoundItem as SocioBE;
            if (socio == null)
            {
                return;
            }

            txtIdSocio.Text = socio.IdSocio.ToString();
            txtTipoDocumento.Text = socio.TipoDocumento;
            txtNumeroDocumento.Text = socio.NumeroDocumento.ToString();
            txtNombre.Text = socio.Nombre;
            txtApellido.Text = socio.Apellido;
            txtFechaNacimiento.Text = socio.FechaNacimiento.ToString("yyyy-MM-dd");
            txtNacionalidad.Text = socio.Nacionalidad;
            txtMail.Text = socio.Mail;
            txtTelefono.Text = socio.Telefono.ToString();
            cmbActivo.SelectedItem = string.IsNullOrWhiteSpace(socio.Activo) ? "S" : socio.Activo;
        }

        private SocioBE ObtenerSocioDesdePantalla()
        {
            SocioBE socio = new SocioBE();
            int idSocio;
            int numeroDocumento;
            int telefono;
            DateTime fechaNacimiento;

            if (int.TryParse(txtIdSocio.Text.Trim(), out idSocio))
            {
                socio.IdSocio = idSocio;
            }

            if (!int.TryParse(txtNumeroDocumento.Text.Trim(), out numeroDocumento))
            {
                throw new Exception("Número de documento inválido.");
            }

            if (!int.TryParse(txtTelefono.Text.Trim(), out telefono))
            {
                throw new Exception("Teléfono inválido.");
            }

            if (!DateTime.TryParse(txtFechaNacimiento.Text.Trim(), out fechaNacimiento))
            {
                throw new Exception("Fecha de nacimiento inválida.");
            }

            socio.TipoDocumento = txtTipoDocumento.Text.Trim();
            socio.NumeroDocumento = numeroDocumento;
            socio.Nombre = txtNombre.Text.Trim();
            socio.Apellido = txtApellido.Text.Trim();
            socio.FechaNacimiento = fechaNacimiento;
            socio.Nacionalidad = txtNacionalidad.Text.Trim();
            socio.Mail = txtMail.Text.Trim();
            socio.Telefono = telefono;
            socio.Activo = cmbActivo.SelectedItem == null ? "S" : cmbActivo.SelectedItem.ToString();
            return socio;
        }

        private string ObtenerUsuarioActual()
        {
            return SessionManager.SesionIniciada ? SessionManager.ObtenerUsuarioActual().Username : "SIN_SESION";
        }

        private void LimpiarCampos()
        {
            txtIdSocio.Text = string.Empty;
            txtTipoDocumento.Text = string.Empty;
            txtNumeroDocumento.Text = string.Empty;
            txtNombre.Text = string.Empty;
            txtApellido.Text = string.Empty;
            txtFechaNacimiento.Text = string.Empty;
            txtNacionalidad.Text = string.Empty;
            txtMail.Text = string.Empty;
            txtTelefono.Text = string.Empty;
            cmbActivo.SelectedIndex = 0;
        }
    }
}
