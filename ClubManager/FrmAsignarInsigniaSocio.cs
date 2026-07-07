using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using BLL;
using SERVICIOS;

namespace ClubManager
{
    public class FrmAsignarInsigniaSocio : Form
    {
        private readonly ModuloClubBLL moduloClubBLL = new ModuloClubBLL();
        private TextBox txtSocio;
        private ComboBox cmbInsignia;
        private ComboBox cmbNivel;
        private TextBox txtMotivo;
        private Label lblSocioEncontrado;
        private DataGridView dgvInsignias;
        private DataTable catalogo;

        public FrmAsignarInsigniaSocio()
        {
            Inicializar();
            CargarCatalogo();
            dgvInsignias.DataSource = CrearTablaVacia();
        }

        private void Inicializar()
        {
            Text = "Asignar insignia a socio";
            StartPosition = FormStartPosition.CenterScreen;
            Width = 1120;
            Height = 700;
            KeyPreview = true;
            KeyDown += delegate(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Escape) Close(); };
            VisualStyleHelper.AplicarEstiloBase(this);

            CrearLabel("Asignar insignia", 35, 85, 700, 30, 16F, FontStyle.Bold);
            CrearLabel("Busque primero el socio. Después seleccione la insignia y un nivel válido del catálogo.", 35, 125, 950, 35, 10F, FontStyle.Regular);

            CrearLabel("N° de socio o usuario", 35, 180, 220, 24, 9F, FontStyle.Bold);
            txtSocio = CrearTextBox(35, 207, 220);

            Button btnBuscar = CrearBoton("Buscar socio", 270, 204, 150, 32);
            btnBuscar.Click += delegate { EjecutarSeguro(BuscarSocio); };

            lblSocioEncontrado = CrearLabel("Socio seleccionado: ninguno", 440, 207, 500, 24, 9F, FontStyle.Bold);

            CrearLabel("Insignia", 35, 260, 220, 24, 9F, FontStyle.Bold);
            cmbInsignia = CrearComboBox(35, 287, 260);
            cmbInsignia.SelectedIndexChanged += delegate { CargarNivelesDeInsignia(); };

            CrearLabel("Nivel", 315, 260, 220, 24, 9F, FontStyle.Bold);
            cmbNivel = CrearComboBox(315, 287, 260);

            CrearLabel("Motivo / observación", 595, 260, 260, 24, 9F, FontStyle.Bold);
            txtMotivo = CrearTextBox(595, 287, 320);

            Button btnAsignar = CrearBoton("Asignar insignia", 35, 335, 190, 36);
            btnAsignar.Click += delegate { EjecutarSeguro(AsignarInsignia); };

            Button btnVolver = CrearBoton("Volver", 245, 335, 160, 36);
            btnVolver.Click += delegate { Close(); };

            CrearLabel("Insignias actuales del socio", 35, 395, 500, 24, 10F, FontStyle.Bold);
            dgvInsignias = new DataGridView();
            dgvInsignias.Left = 35;
            dgvInsignias.Top = 425;
            dgvInsignias.Width = 1030;
            dgvInsignias.Height = 205;
            dgvInsignias.ReadOnly = true;
            dgvInsignias.AllowUserToAddRows = false;
            dgvInsignias.AllowUserToDeleteRows = false;
            dgvInsignias.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            Controls.Add(dgvInsignias);
        }

        private void EjecutarSeguro(Action accion)
        {
            try { accion(); }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }

        private void CargarCatalogo()
        {
            catalogo = moduloClubBLL.ConsultarCatalogoInsignias();
            cmbInsignia.DataSource = catalogo.Copy();
            cmbInsignia.DisplayMember = "Nombre";
            cmbInsignia.ValueMember = "Nombre";
            CargarNivelesDeInsignia();
        }

        private void CargarNivelesDeInsignia()
        {
            cmbNivel.Items.Clear();
            DataRowView fila = cmbInsignia.SelectedItem as DataRowView;
            string requisitos = fila != null && fila.Row.Table.Columns.Contains("RequisitoNiveles") ? Convert.ToString(fila["RequisitoNiveles"]) : string.Empty;
            List<int> niveles = ExtraerNiveles(requisitos);
            foreach (int nivel in niveles) cmbNivel.Items.Add(nivel.ToString());
            if (cmbNivel.Items.Count == 0) cmbNivel.Items.Add("1");
            cmbNivel.SelectedIndex = 0;
        }

        private List<int> ExtraerNiveles(string requisitos)
        {
            List<int> niveles = new List<int>();
            if (!string.IsNullOrWhiteSpace(requisitos))
            {
                string[] partes = requisitos.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string parte in partes)
                {
                    string primera = parte.Split('|')[0].Trim();
                    int nivel;
                    Match match = Regex.Match(primera, @"\d+");
                    if (match.Success && int.TryParse(match.Value, out nivel) && !niveles.Contains(nivel)) niveles.Add(nivel);
                }
            }
            niveles.Sort();
            return niveles;
        }

        private void BuscarSocio()
        {
            int idSocio = moduloClubBLL.ResolverIdSocio(txtSocio.Text.Trim());
            string rol = moduloClubBLL.ObtenerRolSocio(idSocio);
            lblSocioEncontrado.Text = "Socio seleccionado: " + idSocio.ToString() + " - " + rol;
            if (!string.Equals(rol, "Socio Pleno", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Este socio no es Socio Pleno. Puede consultar sus datos, pero no puede recibir insignias deportivas.", "Restricción de insignias", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            DataTable tabla = moduloClubBLL.ConsultarInsignias();
            DataView vista = new DataView(tabla);
            vista.RowFilter = "IdSocio = " + idSocio.ToString();
            DataTable filtrada = vista.ToTable();
            dgvInsignias.DataSource = filtrada;
            OcultarColumnasTecnicas();
        }

        private void AsignarInsignia()
        {
            int idSocio = moduloClubBLL.ResolverIdSocio(txtSocio.Text.Trim());
            if (cmbInsignia.SelectedValue == null) throw new Exception("Seleccione una insignia.");
            int nivel;
            if (!int.TryParse(Convert.ToString(cmbNivel.SelectedItem), out nivel)) throw new Exception("Seleccione un nivel válido.");
            string usuario = SessionManager.SesionIniciada ? SessionManager.ObtenerUsuarioActual().Username : "SIN_SESION";
            moduloClubBLL.AsignarInsigniaSocio(idSocio, cmbInsignia.SelectedValue.ToString(), nivel, txtMotivo.Text.Trim(), usuario);
            BuscarSocio();
            MessageBox.Show("Insignia asignada correctamente.");
        }

        private void OcultarColumnasTecnicas()
        {
            if (dgvInsignias.Columns.Contains("IdInsignia")) dgvInsignias.Columns["IdInsignia"].Visible = false;
            if (dgvInsignias.Columns.Contains("Imagen")) dgvInsignias.Columns["Imagen"].Visible = false;
            if (dgvInsignias.Columns.Contains("RequisitoNiveles")) dgvInsignias.Columns["RequisitoNiveles"].HeaderText = "Requisitos por nivel";
            if (dgvInsignias.Columns.Contains("FechaOtorgamiento")) dgvInsignias.Columns["FechaOtorgamiento"].HeaderText = "Fecha de asignación";
            if (dgvInsignias.Columns.Contains("IdSocio")) dgvInsignias.Columns["IdSocio"].HeaderText = "N° socio";
        }

        private DataTable CrearTablaVacia()
        {
            DataTable tabla = new DataTable();
            tabla.Columns.Add("Mensaje");
            tabla.Rows.Add("Busque un socio para ver sus insignias.");
            return tabla;
        }

        private Label CrearLabel(string texto, int left, int top, int width, int height, float size, FontStyle style)
        {
            Label label = new Label();
            label.Text = texto;
            label.Left = left;
            label.Top = top;
            label.Width = width;
            label.Height = height;
            label.ForeColor = Color.White;
            label.BackColor = Color.FromArgb(120, 0, 0, 0);
            label.Font = new Font("Segoe UI", size, style);
            Controls.Add(label);
            return label;
        }

        private TextBox CrearTextBox(int left, int top, int width)
        {
            TextBox textBox = new TextBox();
            textBox.Left = left;
            textBox.Top = top;
            textBox.Width = width;
            Controls.Add(textBox);
            return textBox;
        }

        private ComboBox CrearComboBox(int left, int top, int width)
        {
            ComboBox combo = new ComboBox();
            combo.Left = left;
            combo.Top = top;
            combo.Width = width;
            combo.DropDownStyle = ComboBoxStyle.DropDownList;
            Controls.Add(combo);
            return combo;
        }

        private Button CrearBoton(string texto, int left, int top, int width, int height)
        {
            Button boton = new Button();
            boton.Text = texto;
            boton.Left = left;
            boton.Top = top;
            boton.Width = width;
            boton.Height = height;
            boton.BackColor = Color.FromArgb(75, 12, 24);
            boton.ForeColor = Color.White;
            boton.FlatStyle = FlatStyle.Flat;
            boton.FlatAppearance.BorderColor = Color.White;
            boton.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            Controls.Add(boton);
            return boton;
        }
    }
}
