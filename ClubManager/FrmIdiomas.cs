using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BE;
using BLL;
using SERVICIOS;
using SERVICIOS.Observer;

namespace ClubManager
{
    public class FrmIdiomas : Form, IOberverIdioma
    {
        private readonly IdiomaBLL idiomaBLL;
        private readonly TraduccionBLL traduccionBLL;
        private readonly bool usuarioActualEsAdministrador;

        private ComboBox cmbIdiomas;
        private DataGridView dgvTraducciones;
        private TextBox txtNuevoIdioma;
        private TextBox txtNombreControl;
        private TextBox txtTextoTraduccion;
        private Button btnAltaIdioma;
        private Button btnGuardarTraduccion;
        private Label lblNuevoIdioma;
        private Label lblNombreControl;
        private Label lblTextoTraduccion;
        private Label lblPermisoAdministrador;
        private Button btnVolver;
        private bool cargandoIdiomas;

        public FrmIdiomas()
        {
            idiomaBLL = new IdiomaBLL();
            traduccionBLL = new TraduccionBLL();
            usuarioActualEsAdministrador = EsUsuarioActualAdministrador();
            InicializarControles();
            ConfigurarPermisosDePantalla();
            CargarIdiomas();
            VisualStyleHelper.AplicarEstiloBase(this);
            TratamientoIdioma.Instancia.Suscribir(this);
            ActualizarIdioma();
        }

        private void InicializarControles()
        {
            Text = "Gestión de idiomas";
            StartPosition = FormStartPosition.CenterScreen;
            Width = 760;
            Height = 520;

            Label lblIdioma = new Label();
            lblIdioma.Text = "Idioma";
            lblIdioma.Left = 20;
            lblIdioma.Top = 20;
            lblIdioma.Width = 100;

            cmbIdiomas = new ComboBox();
            cmbIdiomas.Left = 120;
            cmbIdiomas.Top = 16;
            cmbIdiomas.Width = 180;
            cmbIdiomas.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbIdiomas.SelectedIndexChanged += cmbIdiomas_SelectedIndexChanged;

            lblNuevoIdioma = new Label();
            lblNuevoIdioma.Text = "Crear nuevo idioma";
            lblNuevoIdioma.Left = 320;
            lblNuevoIdioma.Top = 0;
            lblNuevoIdioma.Width = 150;

            txtNuevoIdioma = new TextBox();
            txtNuevoIdioma.Left = 320;
            txtNuevoIdioma.Top = 16;
            txtNuevoIdioma.Width = 150;

            btnAltaIdioma = new Button();
            btnAltaIdioma.Text = "Agregar idioma";
            btnAltaIdioma.Left = 480;
            btnAltaIdioma.Top = 14;
            btnAltaIdioma.Width = 130;
            btnAltaIdioma.Click += btnAltaIdioma_Click;

            lblPermisoAdministrador = new Label();
            lblPermisoAdministrador.Text = "Cambio de idioma disponible. La administración de traducciones queda reservada al administrador.";
            lblPermisoAdministrador.Left = 120;
            lblPermisoAdministrador.Top = 60;
            lblPermisoAdministrador.Width = 520;
            lblPermisoAdministrador.Height = 45;
            lblPermisoAdministrador.Visible = false;

            txtNombreControl = CrearTextBox("Control", 20, 100, out lblNombreControl);
            txtTextoTraduccion = CrearTextBox("Traducción", 240, 100, out lblTextoTraduccion);

            btnGuardarTraduccion = new Button();
            btnGuardarTraduccion.Text = "Guardar / crear traducción";
            btnGuardarTraduccion.Left = 500;
            btnGuardarTraduccion.Top = 98;
            btnGuardarTraduccion.Width = 170;
            btnGuardarTraduccion.Click += btnGuardar_Click;

            btnVolver = new Button();
            btnVolver.Text = "Volver al menú";
            btnVolver.Left = 610;
            btnVolver.Top = 14;
            btnVolver.Width = 120;
            btnVolver.Height = 26;
            btnVolver.Click += delegate { Close(); };
            CancelButton = btnVolver;

            dgvTraducciones = new DataGridView();
            dgvTraducciones.Left = 20;
            dgvTraducciones.Top = 150;
            dgvTraducciones.Width = 700;
            dgvTraducciones.Height = 300;
            dgvTraducciones.ReadOnly = true;
            dgvTraducciones.AllowUserToAddRows = false;
            dgvTraducciones.AllowUserToDeleteRows = false;
            dgvTraducciones.AutoGenerateColumns = true;
            dgvTraducciones.SelectionChanged += dgvTraducciones_SelectionChanged;

            Controls.Add(lblIdioma);
            Controls.Add(cmbIdiomas);
            Controls.Add(lblNuevoIdioma);
            Controls.Add(txtNuevoIdioma);
            Controls.Add(btnAltaIdioma);
            Controls.Add(lblPermisoAdministrador);
            Controls.Add(btnGuardarTraduccion);
            Controls.Add(btnVolver);
            Controls.Add(dgvTraducciones);
        }

        private TextBox CrearTextBox(string etiqueta, int left, int top, out Label label)
        {
            label = new Label();
            label.Text = etiqueta;
            label.Left = left;
            label.Top = top - 22;
            label.Width = 150;
            Controls.Add(label);

            TextBox textBox = new TextBox();
            textBox.Left = left;
            textBox.Top = top;
            textBox.Width = 200;
            Controls.Add(textBox);
            return textBox;
        }

        private void ConfigurarPermisosDePantalla()
        {
            bool permiteAdministrarIdiomas = usuarioActualEsAdministrador;

            lblNuevoIdioma.Visible = permiteAdministrarIdiomas;
            txtNuevoIdioma.Visible = permiteAdministrarIdiomas;
            btnAltaIdioma.Visible = permiteAdministrarIdiomas;

            lblNombreControl.Visible = permiteAdministrarIdiomas;
            txtNombreControl.Visible = permiteAdministrarIdiomas;
            lblTextoTraduccion.Visible = permiteAdministrarIdiomas;
            txtTextoTraduccion.Visible = permiteAdministrarIdiomas;
            btnGuardarTraduccion.Visible = permiteAdministrarIdiomas;
            dgvTraducciones.Visible = permiteAdministrarIdiomas;

            if (!permiteAdministrarIdiomas)
            {
                Width = 760;
                Height = 240;
                btnVolver.Top = 110;
            }

            lblPermisoAdministrador.Visible = !permiteAdministrarIdiomas;
        }

        private bool EsUsuarioActualAdministrador()
        {
            if (!SessionManager.SesionIniciada)
            {
                return false;
            }

            UsuarioBE usuarioActual = SessionManager.ObtenerUsuarioActual();
            if (usuarioActual == null)
            {
                return false;
            }

            if (usuarioActual.IdRol == 1)
            {
                return true;
            }

            return string.Equals(usuarioActual.NombreRol, "Administrador", StringComparison.OrdinalIgnoreCase);
        }

        private void CargarIdiomas()
        {
            cargandoIdiomas = true;

            List<IdiomaBE> idiomas = idiomaBLL.ListarIdiomas();
            cmbIdiomas.DataSource = idiomas;
            cmbIdiomas.DisplayMember = "Nombre";
            cmbIdiomas.ValueMember = "Id";

            if (TratamientoIdioma.Instancia.IdiomaActual != null)
            {
                cmbIdiomas.SelectedValue = TratamientoIdioma.Instancia.IdiomaActual.Id;
            }

            cargandoIdiomas = false;
            CargarTraducciones();
        }

        private void CargarTraducciones()
        {
            if (!usuarioActualEsAdministrador)
            {
                return;
            }

            if (cmbIdiomas.SelectedItem == null)
            {
                return;
            }

            IdiomaBE idioma = (IdiomaBE)cmbIdiomas.SelectedItem;
            dgvTraducciones.DataSource = traduccionBLL.Listar(idioma.Id);
        }

        private void btnAltaIdioma_Click(object sender, EventArgs e)
        {
            try
            {
                if (!usuarioActualEsAdministrador)
                {
                    MessageBox.Show("Solo un usuario administrador puede crear nuevos idiomas.", "Permiso denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                idiomaBLL.AltaIdioma(txtNuevoIdioma.Text.Trim(), ObtenerUsuarioActual());
                txtNuevoIdioma.Text = string.Empty;
                CargarIdiomas();
                MessageBox.Show("Idioma creado correctamente.");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!usuarioActualEsAdministrador)
                {
                    MessageBox.Show("Solo un usuario administrador puede editar traducciones.", "Permiso denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                IdiomaBE idioma = (IdiomaBE)cmbIdiomas.SelectedItem;
                traduccionBLL.GuardarTraduccion(idioma.Id, txtNombreControl.Text, txtTextoTraduccion.Text, ObtenerUsuarioActual());
                CargarTraducciones();
                MessageBox.Show("Traducción guardada correctamente.");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbIdiomas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cargandoIdiomas)
            {
                return;
            }

            CargarTraducciones();

            if (cmbIdiomas.SelectedItem != null)
            {
                TratamientoIdioma.Instancia.IdiomaActual = (IdiomaBE)cmbIdiomas.SelectedItem;
                TratamientoIdioma.Instancia.Notificar();
                ActualizarIdioma();
            }
        }

        private void dgvTraducciones_SelectionChanged(object sender, EventArgs e)
        {
            if (!usuarioActualEsAdministrador)
            {
                return;
            }

            if (dgvTraducciones.CurrentRow == null || dgvTraducciones.CurrentRow.DataBoundItem == null)
            {
                return;
            }

            TraduccionBE traduccion = dgvTraducciones.CurrentRow.DataBoundItem as TraduccionBE;
            if (traduccion == null)
            {
                return;
            }

            txtNombreControl.Text = traduccion.NombreControl;
            txtTextoTraduccion.Text = traduccion.Traduccion;
        }


        public void ActualizarIdioma()
        {
            bool ingles = TratamientoIdioma.Instancia.IdiomaActual != null &&
                          (TratamientoIdioma.Instancia.IdiomaActual.Id == 2 ||
                           string.Equals(TratamientoIdioma.Instancia.IdiomaActual.Nombre, "English", StringComparison.OrdinalIgnoreCase));

            if (ingles)
            {
                Text = "Language management";
                ActualizarLabel("Idioma", "Language");
                lblNuevoIdioma.Text = "Create new language";
                btnAltaIdioma.Text = "Add language";
                lblNombreControl.Text = "Control";
                lblTextoTraduccion.Text = "Translation";
                btnGuardarTraduccion.Text = "Save / create translation";
                btnVolver.Text = "Back to menu";
                lblPermisoAdministrador.Text = "Language change available. Translation administration is reserved to the administrator.";
            }
            else
            {
                Text = "Gestión de idiomas";
                ActualizarLabel("Idioma", "Idioma");
                lblNuevoIdioma.Text = "Crear nuevo idioma";
                btnAltaIdioma.Text = "Agregar idioma";
                lblNombreControl.Text = "Control";
                lblTextoTraduccion.Text = "Traducción";
                btnGuardarTraduccion.Text = "Guardar / crear traducción";
                btnVolver.Text = "Volver al menú";
                lblPermisoAdministrador.Text = "Cambio de idioma disponible. La administración de traducciones queda reservada al administrador.";
            }
        }

        private void ActualizarLabel(string textoActualOInicial, string nuevoTexto)
        {
            foreach (Control control in Controls)
            {
                Label label = control as Label;
                if (label != null && (label.Text == textoActualOInicial || label.Text == "Language" || label.Text == "Idioma"))
                {
                    if (label.Left == 20 && label.Top == 20)
                    {
                        label.Text = nuevoTexto;
                    }
                }
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            TratamientoIdioma.Instancia.Desuscribir(this);
            base.OnFormClosed(e);
        }

        private string ObtenerUsuarioActual()
        {
            return SessionManager.SesionIniciada ? SessionManager.ObtenerUsuarioActual().Username : "SIN_SESION";
        }
    }
}
