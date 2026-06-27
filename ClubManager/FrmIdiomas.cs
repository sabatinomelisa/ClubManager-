using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BE;
using BLL;
using SERVICIOS;
using SERVICIOS.Observer;

namespace ClubManager
{
    public class FrmIdiomas : Form
    {
        private readonly IdiomaBLL idiomaBLL;
        private readonly TraduccionBLL traduccionBLL;
        private ComboBox cmbIdiomas;
        private DataGridView dgvTraducciones;
        private TextBox txtNuevoIdioma;
        private TextBox txtNombreControl;
        private TextBox txtTextoTraduccion;

        public FrmIdiomas()
        {
            idiomaBLL = new IdiomaBLL();
            traduccionBLL = new TraduccionBLL();
            InicializarControles();
            CargarIdiomas();
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

            txtNuevoIdioma = new TextBox();
            txtNuevoIdioma.Left = 320;
            txtNuevoIdioma.Top = 16;
            txtNuevoIdioma.Width = 150;

            Button btnAltaIdioma = new Button();
            btnAltaIdioma.Text = "Alta idioma";
            btnAltaIdioma.Left = 480;
            btnAltaIdioma.Top = 14;
            btnAltaIdioma.Width = 100;
            btnAltaIdioma.Click += btnAltaIdioma_Click;

            txtNombreControl = CrearTextBox("Control", 20, 80);
            txtTextoTraduccion = CrearTextBox("Traducción", 240, 80);

            Button btnGuardar = new Button();
            btnGuardar.Text = "Guardar traducción";
            btnGuardar.Left = 510;
            btnGuardar.Top = 78;
            btnGuardar.Width = 150;
            btnGuardar.Click += btnGuardar_Click;

            dgvTraducciones = new DataGridView();
            dgvTraducciones.Left = 20;
            dgvTraducciones.Top = 130;
            dgvTraducciones.Width = 700;
            dgvTraducciones.Height = 320;
            dgvTraducciones.ReadOnly = true;
            dgvTraducciones.AllowUserToAddRows = false;
            dgvTraducciones.AllowUserToDeleteRows = false;
            dgvTraducciones.AutoGenerateColumns = true;
            dgvTraducciones.SelectionChanged += dgvTraducciones_SelectionChanged;

            Controls.Add(lblIdioma);
            Controls.Add(cmbIdiomas);
            Controls.Add(txtNuevoIdioma);
            Controls.Add(btnAltaIdioma);
            Controls.Add(btnGuardar);
            Controls.Add(dgvTraducciones);
        }

        private TextBox CrearTextBox(string etiqueta, int left, int top)
        {
            Label label = new Label();
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

        private void CargarIdiomas()
        {
            List<IdiomaBE> idiomas = idiomaBLL.ListarIdiomas();
            cmbIdiomas.DataSource = idiomas;
            cmbIdiomas.DisplayMember = "Nombre";
            cmbIdiomas.ValueMember = "Id";
        }

        private void CargarTraducciones()
        {
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
            CargarTraducciones();

            if (cmbIdiomas.SelectedItem != null)
            {
                TratamientoIdioma.Instancia.IdiomaActual = (IdiomaBE)cmbIdiomas.SelectedItem;
                TratamientoIdioma.Instancia.Notificar();
            }
        }

        private void dgvTraducciones_SelectionChanged(object sender, EventArgs e)
        {
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

        private string ObtenerUsuarioActual()
        {
            return SessionManager.SesionIniciada ? SessionManager.ObtenerUsuarioActual().Username : "SIN_SESION";
        }
    }
}
