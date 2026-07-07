using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using BLL;
using SERVICIOS;

namespace ClubManager
{
    public class FrmCatalogoInsignias : Form
    {
        private readonly ModuloClubBLL moduloClubBLL = new ModuloClubBLL();
        private ComboBox cmbExistente;
        private TextBox txtNombre;
        private TextBox txtDescripcion;
        private TextBox txtImagen;
        private DataGridView dgvNiveles;
        private DataGridView dgvCatalogo;
        private string rutaImagenSeleccionada;
        private int idInsigniaSeleccionada;

        public FrmCatalogoInsignias()
        {
            Inicializar();
            CargarCatalogo();
            LimpiarFormulario();
        }

        private void Inicializar()
        {
            Text = "Catálogo de insignias";
            StartPosition = FormStartPosition.CenterScreen;
            Width = 1180;
            Height = 760;
            KeyPreview = true;
            KeyDown += delegate(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Escape) Close(); };
            VisualStyleHelper.AplicarEstiloBase(this);

            CrearLabel("Crear o editar insignias", 35, 75, 800, 32, 16F, FontStyle.Bold);
            CrearLabel("Para editar, seleccione una insignia existente y presione Cargar. Para crear, presione Nueva insignia y complete los datos.", 35, 115, 1000, 35, 10F, FontStyle.Regular);

            CrearLabel("Insignia existente", 35, 165, 220, 24, 9F, FontStyle.Bold);
            cmbExistente = CrearComboBox(35, 192, 260);
            Button btnCargar = CrearBoton("Cargar para editar", 315, 190, 160, 32);
            btnCargar.Click += delegate { EjecutarSeguro(CargarSeleccionada); };
            Button btnNueva = CrearBoton("Nueva insignia", 495, 190, 150, 32);
            btnNueva.Click += delegate { LimpiarFormulario(); };
            Button btnVolver = CrearBoton("Volver", 665, 190, 150, 32);
            btnVolver.Click += delegate { Close(); };

            CrearLabel("Nombre", 35, 250, 200, 24, 9F, FontStyle.Bold);
            txtNombre = CrearTextBox(35, 277, 260);
            CrearLabel("Descripción general", 315, 250, 250, 24, 9F, FontStyle.Bold);
            txtDescripcion = CrearTextBox(315, 277, 360);
            CrearLabel("Imagen", 695, 250, 200, 24, 9F, FontStyle.Bold);
            txtImagen = CrearTextBox(695, 277, 240);
            txtImagen.ReadOnly = true;
            Button btnImagen = CrearBoton("Buscar imagen", 950, 275, 130, 32);
            btnImagen.Click += delegate { EjecutarSeguro(SeleccionarImagen); };

            CrearLabel("Niveles de la insignia", 35, 325, 500, 24, 10F, FontStyle.Bold);
            CrearLabel("Use + Agregar nivel para sumar filas. Complete una descripción clara para cada nivel posible.", 35, 350, 980, 28, 9F, FontStyle.Regular);

            dgvNiveles = new DataGridView();
            dgvNiveles.Left = 35;
            dgvNiveles.Top = 385;
            dgvNiveles.Width = 1080;
            dgvNiveles.Height = 135;
            dgvNiveles.AllowUserToAddRows = false;
            dgvNiveles.AllowUserToDeleteRows = true;
            dgvNiveles.AutoGenerateColumns = false;
            dgvNiveles.Columns.Add("Nivel", "Nivel");
            dgvNiveles.Columns.Add("Titulo", "Descripción del nivel");
            dgvNiveles.Columns.Add("Descripcion", "Detalle interno");
            dgvNiveles.Columns.Add("Requisito", "Requisito técnico");
            dgvNiveles.Columns[0].Width = 80;
            dgvNiveles.Columns[1].Width = 930;
            dgvNiveles.Columns[2].Visible = false;
            dgvNiveles.Columns[3].Visible = false;
            Controls.Add(dgvNiveles);

            Button btnAgregarNivel = CrearBoton("+ Agregar nivel", 35, 535, 160, 34);
            btnAgregarNivel.Click += delegate { AgregarFilaNivel(); };
            Button btnCrear = CrearBoton("Crear insignia", 215, 535, 160, 34);
            btnCrear.Click += delegate { EjecutarSeguro(delegate { GuardarCatalogo(true); }); };
            Button btnActualizar = CrearBoton("Actualizar insignia", 395, 535, 180, 34);
            btnActualizar.Click += delegate { EjecutarSeguro(delegate { GuardarCatalogo(false); }); };

            CrearLabel("Catálogo actual", 35, 590, 400, 24, 10F, FontStyle.Bold);
            dgvCatalogo = new DataGridView();
            dgvCatalogo.Left = 35;
            dgvCatalogo.Top = 620;
            dgvCatalogo.Width = 1080;
            dgvCatalogo.Height = 80;
            dgvCatalogo.ReadOnly = true;
            dgvCatalogo.AllowUserToAddRows = false;
            dgvCatalogo.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            Controls.Add(dgvCatalogo);
        }

        private void EjecutarSeguro(Action accion)
        {
            try { accion(); }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }

        private void CargarCatalogo()
        {
            DataTable tabla = moduloClubBLL.ConsultarCatalogoInsignias();
            cmbExistente.DataSource = tabla.Copy();
            cmbExistente.DisplayMember = "Nombre";
            cmbExistente.ValueMember = "IdInsigniaCatalogo";
            dgvCatalogo.DataSource = tabla;
            if (dgvCatalogo.Columns.Contains("IdInsigniaCatalogo")) dgvCatalogo.Columns["IdInsigniaCatalogo"].Visible = false;
            if (dgvCatalogo.Columns.Contains("RequisitoNiveles")) dgvCatalogo.Columns["RequisitoNiveles"].HeaderText = "Niveles configurados";
            if (dgvCatalogo.Columns.Contains("TieneNiveles")) dgvCatalogo.Columns["TieneNiveles"].Visible = false;
        }

        private void CargarSeleccionada()
        {
            DataRowView fila = cmbExistente.SelectedItem as DataRowView;
            if (fila == null) return;
            idInsigniaSeleccionada = Convert.ToInt32(fila["IdInsigniaCatalogo"]);
            txtNombre.Text = Convert.ToString(fila["Nombre"]);
            txtDescripcion.Text = Convert.ToString(fila["Descripcion"]);
            txtImagen.Text = Convert.ToString(fila["Imagen"]);
            rutaImagenSeleccionada = null;
            CargarNivelesDesdeTexto(Convert.ToString(fila["RequisitoNiveles"]));
        }

        private void LimpiarFormulario()
        {
            txtNombre.Clear();
            txtDescripcion.Clear();
            txtImagen.Clear();
            rutaImagenSeleccionada = null;
            idInsigniaSeleccionada = 0;
            dgvNiveles.Rows.Clear();
            AgregarFilaNivel();
            txtNombre.Focus();
        }

        private void SeleccionarImagen()
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "Imágenes|*.png;*.jpg;*.jpeg;*.bmp|Todos los archivos|*.*";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    rutaImagenSeleccionada = dialog.FileName;
                    txtImagen.Text = Path.GetFileName(dialog.FileName);
                }
            }
        }

        private void GuardarCatalogo(bool crear)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text)) throw new Exception("Ingrese el nombre de la insignia.");
            if (string.IsNullOrWhiteSpace(txtDescripcion.Text)) throw new Exception("Ingrese la descripción general de la insignia.");
            string nombreImagen = PrepararImagen();
            string requisitos = SerializarNiveles();
            string usuario = SessionManager.SesionIniciada ? SessionManager.ObtenerUsuarioActual().Username : "SIN_SESION";
            if (crear)
            {
                moduloClubBLL.GuardarInsigniaCatalogo(txtNombre.Text.Trim(), txtDescripcion.Text.Trim(), nombreImagen, "S", requisitos, usuario);
            }
            else
            {
                if (idInsigniaSeleccionada <= 0) throw new Exception("Seleccione una insignia existente para actualizar.");
                moduloClubBLL.ActualizarInsigniaCatalogo(idInsigniaSeleccionada, txtNombre.Text.Trim(), txtDescripcion.Text.Trim(), nombreImagen, "S", requisitos, usuario);
            }
            CargarCatalogo();
            MessageBox.Show(crear ? "Insignia creada correctamente." : "Insignia actualizada correctamente.");
        }

        private string PrepararImagen()
        {
            if (!string.IsNullOrWhiteSpace(rutaImagenSeleccionada) && File.Exists(rutaImagenSeleccionada))
            {
                string carpeta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Insignias");
                if (!Directory.Exists(carpeta)) Directory.CreateDirectory(carpeta);
                string destino = Path.Combine(carpeta, Path.GetFileName(rutaImagenSeleccionada));
                File.Copy(rutaImagenSeleccionada, destino, true);
                return Path.GetFileName(rutaImagenSeleccionada);
            }
            return txtImagen.Text.Trim();
        }

        private void CargarNivelesDesdeTexto(string texto)
        {
            dgvNiveles.Rows.Clear();
            if (string.IsNullOrWhiteSpace(texto))
            {
                AgregarFilaNivel();
                return;
            }
            string[] lineas = texto.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string linea in lineas)
            {
                string[] partes = linea.Split('|');
                string nivel = partes.Length > 0 ? partes[0] : string.Empty;
                string titulo = partes.Length > 1 ? partes[1] : string.Empty;
                string descripcion = partes.Length > 2 ? partes[2] : string.Empty;
                string requisito = partes.Length > 3 ? partes[3] : string.Empty;
                if (string.IsNullOrWhiteSpace(titulo) && !string.IsNullOrWhiteSpace(descripcion)) titulo = descripcion;
                dgvNiveles.Rows.Add(nivel, titulo, descripcion, requisito);
            }
            if (dgvNiveles.Rows.Count == 0) AgregarFilaNivel();
        }

        private void AgregarFilaNivel()
        {
            int siguiente = 1;
            foreach (DataGridViewRow row in dgvNiveles.Rows)
            {
                if (row.IsNewRow) continue;
                int valor;
                if (int.TryParse(Convert.ToString(row.Cells["Nivel"].Value), out valor) && valor >= siguiente) siguiente = valor + 1;
            }
            int indice = dgvNiveles.Rows.Add(siguiente.ToString(), string.Empty, string.Empty, string.Empty);
            dgvNiveles.CurrentCell = dgvNiveles.Rows[indice].Cells["Titulo"];
            dgvNiveles.BeginEdit(true);
        }

        private string SerializarNiveles()
        {
            System.Collections.Generic.List<string> niveles = new System.Collections.Generic.List<string>();
            foreach (DataGridViewRow row in dgvNiveles.Rows)
            {
                if (row.IsNewRow) continue;
                string nivel = Convert.ToString(row.Cells["Nivel"].Value).Trim();
                string titulo = Convert.ToString(row.Cells["Titulo"].Value).Trim();
                string descripcion = titulo;
                string requisito = titulo;
                if (string.IsNullOrWhiteSpace(nivel)) continue;
                niveles.Add(nivel + "|" + titulo + "|" + descripcion + "|" + requisito);
            }
            return string.Join(";", niveles.ToArray());
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
