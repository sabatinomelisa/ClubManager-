using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BE;
using BLL;
using SERVICIOS;

namespace ClubManager
{
    public class FrmSocios : Form
    {
        private readonly SocioBLL socioBLL;
        private readonly UsuarioBLL usuarioBLL;
        private DataGridView dgvSocios;
        private TextBox txtBusqueda;
        private Label lblAyuda;
        private List<SocioBE> sociosCache;

        public FrmSocios()
        {
            socioBLL = new SocioBLL();
            usuarioBLL = new UsuarioBLL();
            sociosCache = new List<SocioBE>();
            InicializarControles();
            VisualStyleHelper.AplicarEstiloBase(this);
            CargarSocios();
        }

        private void InicializarControles()
        {
            Text = "Gestión de socios";
            StartPosition = FormStartPosition.CenterScreen;
            Width = 1180;
            Height = 730;

            Label titulo = new Label();
            titulo.Text = "Gestión de socios y usuarios";
            titulo.Left = 35;
            titulo.Top = 96;
            titulo.Width = 760;
            titulo.Height = 28;
            titulo.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            titulo.ForeColor = Color.White;
            titulo.BackColor = Color.FromArgb(140, 0, 0, 0);
            Controls.Add(titulo);

            lblAyuda = new Label();
            lblAyuda.Text = "Busque un socio por nombre, documento, mail o usuario. Seleccione una fila para modificar, dar de baja o cambiar rol.";
            lblAyuda.Left = 35;
            lblAyuda.Top = 132;
            lblAyuda.Width = 1030;
            lblAyuda.Height = 28;
            lblAyuda.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblAyuda.ForeColor = Color.White;
            lblAyuda.BackColor = Color.FromArgb(140, 0, 0, 0);
            Controls.Add(lblAyuda);

            Label lblBuscar = CrearLabel("Buscar socio", 35, 180, 140);
            txtBusqueda = new TextBox();
            txtBusqueda.Left = 35;
            txtBusqueda.Top = 204;
            txtBusqueda.Width = 300;
            txtBusqueda.KeyDown += txtBusqueda_KeyDown;
            Controls.Add(txtBusqueda);

            Button btnBuscar = CrearBoton("Buscar", 355, 201, 130);
            btnBuscar.Click += btnBuscar_Click;
            Controls.Add(btnBuscar);

            Button btnLimpiar = CrearBoton("Limpiar búsqueda", 500, 201, 155);
            btnLimpiar.Click += delegate { txtBusqueda.Text = string.Empty; MostrarSocios(sociosCache); };
            Controls.Add(btnLimpiar);

            Button btnNuevo = CrearBoton("Crear nuevo socio", 35, 252, 165);
            btnNuevo.Click += btnNuevo_Click;
            Controls.Add(btnNuevo);

            Button btnModificar = CrearBoton("Modificar seleccionado", 220, 252, 190);
            btnModificar.Click += btnModificar_Click;
            Controls.Add(btnModificar);

            Button btnBaja = CrearBoton("Dar de baja seleccionado", 430, 252, 210);
            btnBaja.Click += btnBaja_Click;
            Controls.Add(btnBaja);

            Button btnActualizar = CrearBoton("Actualizar listado", 660, 252, 160);
            btnActualizar.Click += delegate { CargarSocios(); };
            Controls.Add(btnActualizar);

            Button btnVolver = CrearBoton("Volver al menú", 840, 252, 160);
            btnVolver.Click += delegate { Close(); };
            Controls.Add(btnVolver);

            dgvSocios = new DataGridView();
            dgvSocios.Left = 35;
            dgvSocios.Top = 315;
            dgvSocios.Width = 1085;
            dgvSocios.Height = 330;
            dgvSocios.ReadOnly = true;
            dgvSocios.AllowUserToAddRows = false;
            dgvSocios.AllowUserToDeleteRows = false;
            dgvSocios.AutoGenerateColumns = true;
            dgvSocios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSocios.MultiSelect = false;
            dgvSocios.DataBindingComplete += dgvSocios_DataBindingComplete;
            dgvSocios.CellDoubleClick += delegate { AbrirEdicionSeleccionada(); };
            Controls.Add(dgvSocios);

            AcceptButton = btnBuscar;
            CancelButton = btnVolver;
        }

        private Label CrearLabel(string texto, int left, int top, int width)
        {
            Label label = new Label();
            label.Text = texto;
            label.Left = left;
            label.Top = top;
            label.Width = width;
            label.Height = 22;
            label.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label.ForeColor = Color.White;
            label.BackColor = Color.FromArgb(140, 0, 0, 0);
            Controls.Add(label);
            return label;
        }

        private Button CrearBoton(string texto, int left, int top, int width)
        {
            Button boton = new Button();
            boton.Text = texto;
            boton.Left = left;
            boton.Top = top;
            boton.Width = width;
            boton.Height = 36;
            return boton;
        }

        private void CargarSocios()
        {
            try
            {
                sociosCache = socioBLL.ListarSocios(true) ?? new List<SocioBE>();
                MostrarSocios(sociosCache);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarSocios(List<SocioBE> socios)
        {
            dgvSocios.DataSource = null;
            dgvSocios.DataSource = socios;
            ConfigurarGrillaSocios();
            dgvSocios.ClearSelection();
        }

        private void ConfigurarGrillaSocios()
        {
            if (dgvSocios.Columns.Count == 0)
            {
                return;
            }

            CambiarHeader("IdSocio", "N° socio", 75);
            CambiarHeader("TipoDocumento", "Tipo doc.", 85);
            CambiarHeader("NumeroDocumento", "Documento", 110);
            CambiarHeader("Nombre", "Nombre", 140);
            CambiarHeader("Apellido", "Apellido", 140);
            CambiarHeader("FechaNacimiento", "Fecha nacimiento", 120);
            CambiarHeader("Nacionalidad", "Nacionalidad", 115);
            CambiarHeader("Mail", "Mail", 190);
            CambiarHeader("Telefono", "Teléfono", 100);
            CambiarHeader("Activo", "Activo", 65);

            if (dgvSocios.Columns.Contains("DigitoVerificadorHorizontal"))
            {
                dgvSocios.Columns["DigitoVerificadorHorizontal"].Visible = false;
            }

            if (dgvSocios.Columns.Contains("NombreCompleto"))
            {
                dgvSocios.Columns["NombreCompleto"].Visible = false;
            }
        }

        private void CambiarHeader(string columna, string texto, int ancho)
        {
            if (!dgvSocios.Columns.Contains(columna))
            {
                return;
            }

            dgvSocios.Columns[columna].HeaderText = texto;
            dgvSocios.Columns[columna].Width = ancho;
        }

        private void dgvSocios_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvSocios.ClearSelection();
        }

        private void txtBusqueda_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                FiltrarSocios();
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            FiltrarSocios();
        }

        private void FiltrarSocios()
        {
            string filtro = txtBusqueda.Text.Trim().ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(filtro))
            {
                MostrarSocios(sociosCache);
                return;
            }

            List<SocioBE> filtrados = sociosCache.Where(delegate (SocioBE socio)
            {
                return Contiene(socio.IdSocio.ToString(), filtro)
                    || Contiene(socio.NumeroDocumento.ToString(), filtro)
                    || Contiene(socio.Nombre, filtro)
                    || Contiene(socio.Apellido, filtro)
                    || Contiene(socio.Mail, filtro)
                    || Contiene(socio.Telefono.ToString(), filtro);
            }).ToList();

            MostrarSocios(filtrados);
        }

        private bool Contiene(string valor, string filtro)
        {
            return !string.IsNullOrWhiteSpace(valor) && valor.ToLowerInvariant().Contains(filtro);
        }

        private SocioBE ObtenerSocioSeleccionado()
        {
            if (dgvSocios.CurrentRow == null || dgvSocios.CurrentRow.DataBoundItem == null)
            {
                MessageBox.Show("Seleccione un socio de la grilla.", "Gestión de socios", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }

            SocioBE socio = dgvSocios.CurrentRow.DataBoundItem as SocioBE;
            if (socio == null)
            {
                MessageBox.Show("Seleccione un socio válido.", "Gestión de socios", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            return socio;
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            using (FrmEdicionSocio formulario = new FrmEdicionSocio(null, true, usuarioBLL))
            {
                if (formulario.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        UsuarioBE usuario = formulario.ObtenerUsuario();
                        usuarioBLL.AltaUsuario(usuario);
                        CargarSocios();
                        MessageBox.Show("Socio y usuario creados correctamente.", "Gestión de socios", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            AbrirEdicionSeleccionada();
        }

        private void AbrirEdicionSeleccionada()
        {
            SocioBE socioSeleccionado = ObtenerSocioSeleccionado();
            if (socioSeleccionado == null)
            {
                return;
            }

            using (FrmEdicionSocio formulario = new FrmEdicionSocio(socioSeleccionado, false, usuarioBLL))
            {
                if (formulario.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        SocioBE socio = formulario.ObtenerSocio();
                        socioBLL.ModificarSocio(socio, ObtenerUsuarioActual());
                        usuarioBLL.CambiarRolSocio(socio.IdSocio, formulario.ObtenerIdRol(), ObtenerUsuarioActual());
                        CargarSocios();
                        MessageBox.Show("Datos del socio actualizados correctamente.", "Gestión de socios", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnBaja_Click(object sender, EventArgs e)
        {
            SocioBE socioSeleccionado = ObtenerSocioSeleccionado();
            if (socioSeleccionado == null)
            {
                return;
            }

            DialogResult respuesta = MessageBox.Show("¿Confirma la baja lógica del socio " + socioSeleccionado.NombreCompleto + "?", "Confirmar baja", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (respuesta != DialogResult.Yes)
            {
                return;
            }

            try
            {
                socioBLL.BajaSocio(socioSeleccionado.IdSocio, ObtenerUsuarioActual());
                CargarSocios();
                MessageBox.Show("Socio dado de baja lógicamente.", "Gestión de socios", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string ObtenerUsuarioActual()
        {
            return SessionManager.SesionIniciada ? SessionManager.ObtenerUsuarioActual().Username : "SIN_SESION";
        }

        private class TipoSocioOpcion
        {
            public int IdRol { get; private set; }
            public string Nombre { get; private set; }

            public TipoSocioOpcion(int idRol, string nombre)
            {
                IdRol = idRol;
                Nombre = nombre;
            }

            public override string ToString()
            {
                return Nombre;
            }
        }

        private class FrmEdicionSocio : Form
        {
            private readonly bool alta;
            private readonly UsuarioBLL usuarioBLL;
            private readonly SocioBE socioOriginal;
            private TextBox txtTipoDocumento;
            private TextBox txtNumeroDocumento;
            private TextBox txtNombre;
            private TextBox txtApellido;
            private TextBox txtFechaNacimiento;
            private TextBox txtNacionalidad;
            private TextBox txtMail;
            private TextBox txtTelefono;
            private TextBox txtUsuario;
            private TextBox txtPassword;
            private ComboBox cmbActivo;
            private ComboBox cmbRol;

            public FrmEdicionSocio(SocioBE socio, bool alta, UsuarioBLL usuarioBLL)
            {
                this.alta = alta;
                this.usuarioBLL = usuarioBLL;
                this.socioOriginal = socio;
                Inicializar();
                if (socio != null)
                {
                    CargarSocio(socio);
                }
            }

            private void Inicializar()
            {
                Text = alta ? "Crear socio y usuario" : "Modificar socio";
                StartPosition = FormStartPosition.CenterParent;
                Width = 660;
                Height = alta ? 590 : 510;
                FormBorderStyle = FormBorderStyle.FixedDialog;
                MaximizeBox = false;
                MinimizeBox = false;
                BackColor = Color.White;
                Font = new Font("Segoe UI", 9F, FontStyle.Regular);

                Label titulo = new Label();
                titulo.Text = alta ? "Complete los datos del nuevo socio y su usuario de acceso." : "Modifique los datos del socio seleccionado.";
                titulo.Left = 25;
                titulo.Top = 18;
                titulo.Width = 560;
                titulo.Height = 26;
                titulo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                Controls.Add(titulo);

                txtTipoDocumento = CrearTextBox("Tipo documento", 25, 80, 160);
                txtNumeroDocumento = CrearTextBox("Número documento", 220, 80, 160);
                txtNombre = CrearTextBox("Nombre", 25, 145, 250);
                txtApellido = CrearTextBox("Apellido", 315, 145, 250);
                txtFechaNacimiento = CrearTextBox("Fecha nacimiento (yyyy-mm-dd)", 25, 210, 250);
                txtNacionalidad = CrearTextBox("Nacionalidad", 315, 210, 250);
                txtMail = CrearTextBox("Mail", 25, 275, 250);
                txtTelefono = CrearTextBox("Teléfono", 315, 275, 250);

                Label lblActivo = CrearLabelModal("Estado", 25, 325, 120);
                cmbActivo = new ComboBox();
                cmbActivo.Left = 25;
                cmbActivo.Top = 348;
                cmbActivo.Width = 120;
                cmbActivo.DropDownStyle = ComboBoxStyle.DropDownList;
                cmbActivo.Items.Add("S");
                cmbActivo.Items.Add("N");
                cmbActivo.SelectedIndex = 0;
                Controls.Add(cmbActivo);

                Label lblRol = CrearLabelModal("Rol del usuario", 170, 325, 160);
                cmbRol = new ComboBox();
                cmbRol.Left = 170;
                cmbRol.Top = 348;
                cmbRol.Width = 180;
                cmbRol.DropDownStyle = ComboBoxStyle.DropDownList;
                cmbRol.Items.Add(new TipoSocioOpcion(1, "Administrador"));
                cmbRol.Items.Add(new TipoSocioOpcion(2, "Socio Simple"));
                cmbRol.Items.Add(new TipoSocioOpcion(3, "Socio Pleno"));
                cmbRol.DisplayMember = "Nombre";
                cmbRol.ValueMember = "IdRol";
                cmbRol.SelectedIndex = 1;
                Controls.Add(cmbRol);

                int botonTop = 425;
                if (alta)
                {
                    txtUsuario = CrearTextBox("Usuario de acceso", 25, 425, 250);
                    txtPassword = CrearTextBox("Contraseña", 335, 425, 250);
                    txtPassword.UseSystemPasswordChar = true;
                    botonTop = 500;
                }

                Button btnGuardar = new Button();
                btnGuardar.Text = alta ? "Crear socio" : "Guardar cambios";
                btnGuardar.Left = 335;
                btnGuardar.Top = botonTop;
                btnGuardar.Width = 120;
                btnGuardar.Height = 32;
                btnGuardar.DialogResult = DialogResult.OK;
                Controls.Add(btnGuardar);

                Button btnCancelar = new Button();
                btnCancelar.Text = "Cancelar";
                btnCancelar.Left = 475;
                btnCancelar.Top = botonTop;
                btnCancelar.Width = 120;
                btnCancelar.Height = 32;
                btnCancelar.DialogResult = DialogResult.Cancel;
                Controls.Add(btnCancelar);

                AcceptButton = btnGuardar;
                CancelButton = btnCancelar;
            }

            private Label CrearLabelModal(string texto, int left, int top, int width)
            {
                Label label = new Label();
                label.Text = texto;
                label.Left = left;
                label.Top = top;
                label.Width = width;
                label.Height = 20;
                Controls.Add(label);
                return label;
            }

            private TextBox CrearTextBox(string etiqueta, int left, int top, int width)
            {
                CrearLabelModal(etiqueta, left, top - 23, width);
                TextBox textBox = new TextBox();
                textBox.Left = left;
                textBox.Top = top;
                textBox.Width = width;
                Controls.Add(textBox);
                return textBox;
            }

            private void CargarSocio(SocioBE socio)
            {
                txtTipoDocumento.Text = socio.TipoDocumento;
                txtNumeroDocumento.Text = socio.NumeroDocumento.ToString();
                txtNombre.Text = socio.Nombre;
                txtApellido.Text = socio.Apellido;
                txtFechaNacimiento.Text = socio.FechaNacimiento.ToString("yyyy-MM-dd");
                txtNacionalidad.Text = socio.Nacionalidad;
                txtMail.Text = socio.Mail;
                txtTelefono.Text = socio.Telefono.ToString();
                cmbActivo.SelectedItem = string.IsNullOrWhiteSpace(socio.Activo) ? "S" : socio.Activo;

                try
                {
                    UsuarioBE usuario = usuarioBLL.ObtenerPorIdSocio(socio.IdSocio);
                    if (usuario != null)
                    {
                        SeleccionarRol(usuario.IdRol);
                    }
                }
                catch
                {
                    SeleccionarRol(2);
                }
            }

            private void SeleccionarRol(int idRol)
            {
                for (int i = 0; i < cmbRol.Items.Count; i++)
                {
                    TipoSocioOpcion opcion = cmbRol.Items[i] as TipoSocioOpcion;
                    if (opcion != null && opcion.IdRol == idRol)
                    {
                        cmbRol.SelectedIndex = i;
                        return;
                    }
                }
                cmbRol.SelectedIndex = 1;
            }

            public SocioBE ObtenerSocio()
            {
                SocioBE socio = new SocioBE();
                if (socioOriginal != null)
                {
                    socio.IdSocio = socioOriginal.IdSocio;
                }

                int numeroDocumento;
                int telefono;
                DateTime fechaNacimiento;

                if (!int.TryParse(txtNumeroDocumento.Text.Trim(), out numeroDocumento))
                {
                    throw new Exception("Ingrese un número de documento válido.");
                }

                if (!int.TryParse(txtTelefono.Text.Trim(), out telefono))
                {
                    throw new Exception("Ingrese un teléfono válido.");
                }

                if (!DateTime.TryParse(txtFechaNacimiento.Text.Trim(), out fechaNacimiento))
                {
                    throw new Exception("Ingrese una fecha de nacimiento válida con formato yyyy-mm-dd.");
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

            public UsuarioBE ObtenerUsuario()
            {
                SocioBE socio = ObtenerSocio();
                UsuarioBE usuario = new UsuarioBE();
                usuario.TipoDocumento = socio.TipoDocumento;
                usuario.NumeroDocumento = socio.NumeroDocumento;
                usuario.Nombre = socio.Nombre;
                usuario.Apellido = socio.Apellido;
                usuario.FechaNacimiento = socio.FechaNacimiento;
                usuario.Nacionalidad = socio.Nacionalidad;
                usuario.Mail = socio.Mail;
                usuario.Telefono = socio.Telefono;
                usuario.Activo = socio.Activo;
                usuario.Username = txtUsuario.Text.Trim();
                usuario.Password = txtPassword.Text.Trim();
                usuario.FechaCreacion = DateTime.Now;
                usuario.Bloqueado = "N";
                usuario.IdRol = ObtenerIdRol();
                return usuario;
            }

            public int ObtenerIdRol()
            {
                TipoSocioOpcion rol = cmbRol.SelectedItem as TipoSocioOpcion;
                if (rol == null)
                {
                    throw new Exception("Seleccione el rol del usuario.");
                }
                return rol.IdRol;
            }
        }
    }
}
