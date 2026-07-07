using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using BE;
using BLL;
using SERVICIOS;
using SERVICIOS.Observer;

namespace ClubManager
{
    public class FrmIdiomas : Form, IOberverIdioma
    {
        private const string ColControl = "NombreControl";
        private const string ColElemento = "Elemento";
        private const string ColTextoBase = "TextoBase";
        private const string ColTraduccion = "Traduccion";

        private readonly IdiomaBLL idiomaBLL;
        private readonly TraduccionBLL traduccionBLL;
        private readonly bool usuarioActualEsAdministrador;

        private Label lblTitulo;
        private Label lblIdiomaExistente;
        private Label lblNuevoIdioma;
        private Label lblAyuda;
        private Label lblPermisoAdministrador;
        private ComboBox cmbIdiomas;
        private TextBox txtNuevoIdioma;
        private Button btnPrepararNuevoIdioma;
        private Button btnCrearIdioma;
        private Button btnGuardarTraducciones;
        private Button btnEliminarIdioma;
        private Button btnVolver;
        private DataGridView dgvTraducciones;

        private bool cargandoIdiomas;
        private bool modoNuevoIdioma;
        private int idIdiomaSeleccionado;

        public FrmIdiomas()
        {
            idiomaBLL = new IdiomaBLL();
            traduccionBLL = new TraduccionBLL();
            usuarioActualEsAdministrador = EsUsuarioActualAdministrador();
            InicializarControles();
            ConfigurarPermisosDePantalla();
            CargarIdiomas(0);
            VisualStyleHelper.AplicarEstiloBase(this);
            TratamientoIdioma.Instancia.Suscribir(this);
            ActualizarIdioma();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams parametros = base.CreateParams;
                parametros.ExStyle |= 0x02000000;
                return parametros;
            }
        }

        private void InicializarControles()
        {
            Text = "Gestión de idiomas";
            StartPosition = FormStartPosition.CenterScreen;
            Width = 980;
            Height = 640;
            KeyPreview = true;

            lblTitulo = CrearLabel("Administrar idiomas y traducciones", 25, 20, 880, 28, 12F, true);

            lblIdiomaExistente = CrearLabel("Idioma existente", 25, 68, 220, 22, 9F, true);
            cmbIdiomas = new ComboBox();
            cmbIdiomas.Left = 25;
            cmbIdiomas.Top = 94;
            cmbIdiomas.Width = 260;
            cmbIdiomas.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbIdiomas.SelectedIndexChanged += cmbIdiomas_SelectedIndexChanged;
            Controls.Add(cmbIdiomas);

            lblNuevoIdioma = CrearLabel("Nuevo idioma", 315, 68, 220, 22, 9F, true);
            txtNuevoIdioma = new TextBox();
            txtNuevoIdioma.Left = 315;
            txtNuevoIdioma.Top = 94;
            txtNuevoIdioma.Width = 210;
            Controls.Add(txtNuevoIdioma);

            btnPrepararNuevoIdioma = CrearBoton("Preparar tabla", 535, 92, 145, 30);
            btnPrepararNuevoIdioma.Click += btnPrepararNuevoIdioma_Click;

            btnCrearIdioma = CrearBoton("Crear idioma", 690, 92, 120, 30);
            btnCrearIdioma.Click += btnCrearIdioma_Click;

            btnEliminarIdioma = CrearBoton("Eliminar idioma", 820, 92, 130, 30);
            btnEliminarIdioma.Click += btnEliminarIdioma_Click;

            btnVolver = CrearBoton("Volver al menú", 820, 550, 130, 32);
            btnVolver.Click += delegate { Close(); };
            CancelButton = btnVolver;

            lblAyuda = CrearLabel("Seleccione un idioma para editar sus textos. Para crear uno nuevo, escriba el nombre, prepare la tabla, complete la columna Nueva traducción y presione Crear idioma. Para borrar un idioma personalizado, selecciónelo y presione Eliminar idioma.", 25, 135, 900, 48, 9F, true);

            lblPermisoAdministrador = CrearLabel("Cambio de idioma disponible. La administración de traducciones queda reservada al administrador.", 25, 135, 900, 48, 9F, true);
            lblPermisoAdministrador.Visible = false;

            dgvTraducciones = new DataGridView();
            dgvTraducciones.Left = 25;
            dgvTraducciones.Top = 195;
            dgvTraducciones.Width = 920;
            dgvTraducciones.Height = 340;
            dgvTraducciones.AllowUserToAddRows = false;
            dgvTraducciones.AllowUserToDeleteRows = false;
            dgvTraducciones.AutoGenerateColumns = false;
            dgvTraducciones.RowHeadersVisible = false;
            dgvTraducciones.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTraducciones.MultiSelect = false;
            dgvTraducciones.BackgroundColor = Color.White;
            dgvTraducciones.Columns.Add(CrearColumnaTexto(ColControl, "Clave interna", 160, true));
            dgvTraducciones.Columns.Add(CrearColumnaTexto(ColElemento, "Texto / pantalla", 260, true));
            dgvTraducciones.Columns.Add(CrearColumnaTexto(ColTextoBase, "Texto original en español", 300, true));
            dgvTraducciones.Columns.Add(CrearColumnaTexto(ColTraduccion, "Texto del idioma", 330, false));
            Controls.Add(dgvTraducciones);

            btnGuardarTraducciones = CrearBoton("Guardar cambios", 25, 550, 190, 32);
            btnGuardarTraducciones.Click += btnGuardarTraducciones_Click;
        }

        private Label CrearLabel(string texto, int left, int top, int width, int height, float size, bool bold)
        {
            Label label = new Label();
            label.Text = texto;
            label.Left = left;
            label.Top = top;
            label.Width = width;
            label.Height = height;
            label.ForeColor = Color.White;
            label.BackColor = Color.Transparent;
            label.Font = new Font("Segoe UI", size, bold ? FontStyle.Bold : FontStyle.Regular);
            Controls.Add(label);
            return label;
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

        private DataGridViewTextBoxColumn CrearColumnaTexto(string dataProperty, string header, int width, bool readOnly)
        {
            DataGridViewTextBoxColumn columna = new DataGridViewTextBoxColumn();
            columna.DataPropertyName = dataProperty;
            columna.Name = dataProperty;
            columna.HeaderText = header;
            columna.Width = width;
            columna.ReadOnly = readOnly;
            return columna;
        }

        private void ConfigurarPermisosDePantalla()
        {
            bool permiteAdministrar = usuarioActualEsAdministrador;

            lblTitulo.Visible = permiteAdministrar;
            lblNuevoIdioma.Visible = permiteAdministrar;
            txtNuevoIdioma.Visible = permiteAdministrar;
            btnPrepararNuevoIdioma.Visible = permiteAdministrar;
            btnCrearIdioma.Visible = permiteAdministrar;
            btnEliminarIdioma.Visible = permiteAdministrar;
            lblAyuda.Visible = permiteAdministrar;
            dgvTraducciones.Visible = permiteAdministrar;
            btnGuardarTraducciones.Visible = permiteAdministrar;
            lblPermisoAdministrador.Visible = !permiteAdministrar;

            if (!permiteAdministrar)
            {
                Width = 760;
                Height = 260;
                btnVolver.Left = 25;
                btnVolver.Top = 170;
            }
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

            return usuarioActual.IdRol == 1 || string.Equals(usuarioActual.NombreRol, "Administrador", StringComparison.OrdinalIgnoreCase);
        }

        private void CargarIdiomas(int idSeleccionar)
        {
            cargandoIdiomas = true;

            List<IdiomaBE> idiomas = idiomaBLL.ListarIdiomas();
            cmbIdiomas.DataSource = null;
            cmbIdiomas.DataSource = idiomas;
            cmbIdiomas.DisplayMember = "Nombre";
            cmbIdiomas.ValueMember = "Id";

            int idASeleccionar = idSeleccionar;
            if (idASeleccionar <= 0 && TratamientoIdioma.Instancia.IdiomaActual != null)
            {
                idASeleccionar = TratamientoIdioma.Instancia.IdiomaActual.Id;
            }

            if (idASeleccionar > 0)
            {
                cmbIdiomas.SelectedValue = idASeleccionar;
            }

            cargandoIdiomas = false;

            if (cmbIdiomas.SelectedItem != null)
            {
                CargarTraduccionesIdioma(((IdiomaBE)cmbIdiomas.SelectedItem).Id);
            }
        }

        private void CargarTraduccionesIdioma(int idIdioma)
        {
            if (!usuarioActualEsAdministrador)
            {
                return;
            }

            modoNuevoIdioma = false;
            idIdiomaSeleccionado = idIdioma;
            btnCrearIdioma.Enabled = false;
            btnGuardarTraducciones.Enabled = true;
            btnEliminarIdioma.Enabled = idIdioma > 2;

            DataTable tabla = CrearTablaTraducciones();
            Dictionary<string, string> traduccionesActuales = ObtenerDiccionarioTraducciones(idIdioma);
            Dictionary<string, string> textosBase = ObtenerTextosBaseSistema();
            List<string> controles = ObtenerControles(textosBase, traduccionesActuales);

            foreach (string control in controles)
            {
                string textoBase = textosBase.ContainsKey(control) ? textosBase[control] : string.Empty;
                string traduccion = traduccionesActuales.ContainsKey(control) ? traduccionesActuales[control] : string.Empty;
                tabla.Rows.Add(control, ObtenerNombreAmigable(control), textoBase, traduccion);
            }

            dgvTraducciones.DataSource = tabla;
            ConfigurarColumnasGrilla(false);

            bool esIdiomaBaseEspanol = idIdioma == 1;
            btnGuardarTraducciones.Enabled = !esIdiomaBaseEspanol;
            if (esIdiomaBaseEspanol)
            {
                lblAyuda.Text = TraducirLocal("lblAyudaIdiomasBase", "Español es el idioma base del sistema y se usa como referencia. No se edita desde esta pantalla para evitar alterar los textos originales.", "Spanish is the system base language and is used as reference. It cannot be edited here to avoid altering original texts.");
            }
        }

        private void PrepararTablaNuevoIdioma()
        {
            if (!usuarioActualEsAdministrador)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNuevoIdioma.Text))
            {
                MessageBox.Show(TraducirLocal("msgIngresarNombreIdioma", "Ingresar nombre del idioma.", "Enter the language name."));
                return;
            }

            modoNuevoIdioma = true;
            idIdiomaSeleccionado = 0;
            btnCrearIdioma.Enabled = true;
            btnGuardarTraducciones.Enabled = false;
            btnEliminarIdioma.Enabled = false;

            DataTable tabla = CrearTablaTraducciones();
            Dictionary<string, string> textosBase = ObtenerTextosBaseSistema();
            List<string> controles = ObtenerControles(textosBase, new Dictionary<string, string>());

            foreach (string control in controles)
            {
                string textoBase = textosBase.ContainsKey(control) ? textosBase[control] : string.Empty;
                tabla.Rows.Add(control, ObtenerNombreAmigable(control), textoBase, string.Empty);
            }

            dgvTraducciones.DataSource = tabla;
            ConfigurarColumnasGrilla(true);
        }

        private DataTable CrearTablaTraducciones()
        {
            DataTable tabla = new DataTable();
            tabla.Columns.Add(ColControl, typeof(string));
            tabla.Columns.Add(ColElemento, typeof(string));
            tabla.Columns.Add(ColTextoBase, typeof(string));
            tabla.Columns.Add(ColTraduccion, typeof(string));
            return tabla;
        }

        private Dictionary<string, string> ObtenerDiccionarioTraducciones(int idIdioma)
        {
            Dictionary<string, string> diccionario = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            List<TraduccionBE> traducciones = traduccionBLL.Listar(idIdioma);

            foreach (TraduccionBE traduccion in traducciones)
            {
                if (!diccionario.ContainsKey(traduccion.NombreControl))
                {
                    diccionario.Add(traduccion.NombreControl, traduccion.Traduccion);
                }
            }

            return diccionario;
        }


        private static Dictionary<string, string> ObtenerTextosEspanolBase()
        {
            Dictionary<string, string> textos = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            textos["bitacoraItem"] = "Bitácora";
            textos["btnCrearIdioma"] = "Crear idioma";
            textos["btnEliminarIdioma"] = "Eliminar idioma";
            textos["btnGuardarTraducciones"] = "Guardar cambios";
            textos["btnIngresar"] = "Ingresar";
            textos["btnOlvidaste"] = "¿Olvidaste tu contraseña?";
            textos["btnPrepararNuevoIdioma"] = "Preparar tabla";
            textos["btnRegistrar"] = "Registrar";
            textos["btnSalir"] = "Salir";
            textos["btnVolver"] = "Volver";
            textos["btnVolverMenu"] = "Volver al menú";
            textos["clubMenu"] = "Club";
            textos["colControl"] = "Control";
            textos["colNuevaTraduccion"] = "Nueva traducción";
            textos["colTextoBase"] = "Texto base";
            textos["colTraduccion"] = "Traducción";
            textos["comunicadosItem"] = "Comunicados";
            textos["configuracionCuotasItem"] = "Configuración de cuotas y fees";
            textos["controlCambiosItem"] = "Control de cambios";
            textos["convocatoriasItem"] = "Convocatorias";
            textos["cuentaMenu"] = "Cuenta";
            textos["equiposDisponibilidadItem"] = "Equipos y disponibilidad";
            textos["eventosDisponiblesItem"] = "Eventos disponibles";
            textos["eventosItem"] = "Eventos deportivos";
            textos["finanzasItem"] = "Ingresos y egresos";
            textos["frmIdiomas"] = "Gestión de idiomas";
            textos["frmMenu"] = "Menú Principal";
            textos["historialMailItem"] = "Historial de mail";
            textos["idiomasItem"] = "Idiomas";
            textos["insigniasItem"] = "Insignias";
            textos["integridadItem"] = "Dígitos verificadores";
            textos["inventarioItem"] = "Inventario";
            textos["jugadoresItem"] = "Jugadores";
            textos["lblApellido"] = "Apellido";
            textos["lblAyudaIdiomas"] = "Seleccione un idioma para editar sus traducciones. Para crear uno nuevo, escriba el nombre, prepare la tabla, complete los textos y presione Crear idioma.";
            textos["lblContraseña"] = "Contraseña";
            textos["lblFecNac"] = "Fecha nacimiento";
            textos["lblIdioma"] = "Idioma";
            textos["lblIdiomaExistente"] = "Idioma existente";
            textos["lblMail"] = "Mail";
            textos["lblNacionalidad"] = "Nacionalidad";
            textos["lblNombre"] = "Nombre";
            textos["lblNroDoc"] = "Nro. documento";
            textos["lblNuevoIdioma"] = "Nuevo idioma";
            textos["lblPermisoIdiomas"] = "Cambio de idioma disponible. La administración de traducciones queda reservada al administrador.";
            textos["lblRol"] = "Rol";
            textos["lblTelefono"] = "Teléfono";
            textos["lblTipDoc"] = "Tipo documento";
            textos["lblUsuario"] = "Usuario";
            textos["logoutItem"] = "Cerrar sesión";
            textos["miHistorialItem"] = "Mi historial";
            textos["miPerfilItem"] = "Mi perfil";
            textos["misConvocatoriasItem"] = "Mis convocatorias";
            textos["misEntradasItem"] = "Comprar entrada / Mis entradas";
            textos["misInsigniasItem"] = "Mis insignias";
            textos["misPagosItem"] = "Pagar / ver mis cuotas";
            textos["msgConfirmarEliminarIdioma"] = "Se eliminará el idioma seleccionado y todas sus traducciones. ¿Desea continuar?";
            textos["msgErrorIntegridadLogin"] = "Error de integridad en login: El registro fue modificado o alterado por fuera del sistema.";
            textos["msgIdiomaEliminado"] = "Idioma eliminado correctamente.";
            textos["msgLoginExitoso"] = "Login exitoso.";
            textos["msgNoEliminarIdiomaBase"] = "No se pueden eliminar Español ni English porque son idiomas base del sistema.";
            textos["msgPasswordBloqueada"] = "Contraseña incorrecta. La cuenta fue bloqueada por superar los 3 intentos fallidos.";
            textos["msgPasswordIncorrecta"] = "Contraseña incorrecta.";
            textos["msgUsuarioInexistente"] = "Usuario inexistente.";
            textos["pagosItem"] = "Pagos y cuotas";
            textos["perfilesItem"] = "Perfiles";
            textos["portalSocioMenu"] = "Portal socio";
            textos["publicacionesItem"] = "Comunicación interna";
            textos["reportesItem"] = "Reportes";
            textos["resultadosItem"] = "Resultados";
            textos["seccionAdministracion"] = "Administración";
            textos["seccionInsigniasDestacadas"] = "Insignias destacadas";
            textos["seccionParticipacionDeportiva"] = "Participación deportiva";
            textos["seccionPortalSocioPleno"] = "Portal de socio pleno";
            textos["seccionPortalSocioSimple"] = "Portal de socio simple";
            textos["seccionSeguridad"] = "Seguridad";
            textos["seguridadMenu"] = "Seguridad";
            textos["sociosItem"] = "Socios";
            textos["tituloConfirmarEliminarIdioma"] = "Confirmar eliminación";
            textos["tituloIdiomas"] = "Administrar idiomas y traducciones";
            textos["ventasItem"] = "Ventas";
            return textos;
        }

        private static Dictionary<string, string> ObtenerTextosInglesBase()
        {
            Dictionary<string, string> textos = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            textos["bitacoraItem"] = "Audit log";
            textos["btnCrearIdioma"] = "Create language";
            textos["btnEliminarIdioma"] = "Delete language";
            textos["btnGuardarTraducciones"] = "Save changes";
            textos["btnIngresar"] = "Login";
            textos["btnOlvidaste"] = "Forgot password?";
            textos["btnPrepararNuevoIdioma"] = "Prepare table";
            textos["btnRegistrar"] = "Register";
            textos["btnSalir"] = "Exit";
            textos["btnVolver"] = "Back";
            textos["btnVolverMenu"] = "Back to menu";
            textos["clubMenu"] = "Club";
            textos["colControl"] = "Control";
            textos["colNuevaTraduccion"] = "New translation";
            textos["colTextoBase"] = "Base text";
            textos["colTraduccion"] = "Translation";
            textos["comunicadosItem"] = "Announcements";
            textos["configuracionCuotasItem"] = "Fees and dues configuration";
            textos["controlCambiosItem"] = "Change control";
            textos["convocatoriasItem"] = "Calls";
            textos["cuentaMenu"] = "Account";
            textos["equiposDisponibilidadItem"] = "Teams and availability";
            textos["eventosDisponiblesItem"] = "Available events";
            textos["eventosItem"] = "Sports events";
            textos["finanzasItem"] = "Income and expenses";
            textos["frmIdiomas"] = "Language management";
            textos["frmMenu"] = "Main Menu";
            textos["historialMailItem"] = "Mail history";
            textos["idiomasItem"] = "Languages";
            textos["insigniasItem"] = "Badges";
            textos["integridadItem"] = "Check digits";
            textos["inventarioItem"] = "Inventory";
            textos["jugadoresItem"] = "Players";
            textos["lblApellido"] = "Last name";
            textos["lblAyudaIdiomas"] = "Select a language to edit its translations. To create a new one, type its name, prepare the table, complete the texts, and press Create language.";
            textos["lblContraseña"] = "Password";
            textos["lblFecNac"] = "Birth date";
            textos["lblIdioma"] = "Language";
            textos["lblIdiomaExistente"] = "Existing language";
            textos["lblMail"] = "Email";
            textos["lblNacionalidad"] = "Nationality";
            textos["lblNombre"] = "Name";
            textos["lblNroDoc"] = "Document number";
            textos["lblNuevoIdioma"] = "New language";
            textos["lblPermisoIdiomas"] = "Language change available. Translation administration is reserved to the administrator.";
            textos["lblRol"] = "Role";
            textos["lblTelefono"] = "Phone";
            textos["lblTipDoc"] = "Document type";
            textos["lblUsuario"] = "User";
            textos["logoutItem"] = "Logout";
            textos["miHistorialItem"] = "My history";
            textos["miPerfilItem"] = "My profile";
            textos["misConvocatoriasItem"] = "My call-ups";
            textos["misEntradasItem"] = "Buy ticket / My tickets";
            textos["misInsigniasItem"] = "My badges";
            textos["misPagosItem"] = "Pay / view my dues";
            textos["msgConfirmarEliminarIdioma"] = "The selected language and all its translations will be deleted. Do you want to continue?";
            textos["msgErrorIntegridadLogin"] = "Integrity error on login: The record was modified or altered outside the system.";
            textos["msgIdiomaEliminado"] = "Language deleted successfully.";
            textos["msgLoginExitoso"] = "Login successful.";
            textos["msgNoEliminarIdiomaBase"] = "Spanish and English cannot be deleted because they are system base languages.";
            textos["msgPasswordBloqueada"] = "Incorrect password. The account was blocked after 3 failed attempts.";
            textos["msgPasswordIncorrecta"] = "Incorrect password.";
            textos["msgUsuarioInexistente"] = "User does not exist.";
            textos["pagosItem"] = "Payments and fees";
            textos["perfilesItem"] = "Profiles";
            textos["portalSocioMenu"] = "Member portal";
            textos["publicacionesItem"] = "Internal communication";
            textos["reportesItem"] = "Reports";
            textos["resultadosItem"] = "Results";
            textos["seccionAdministracion"] = "Administration";
            textos["seccionInsigniasDestacadas"] = "Featured badges";
            textos["seccionParticipacionDeportiva"] = "Sports participation";
            textos["seccionPortalSocioPleno"] = "Full member portal";
            textos["seccionPortalSocioSimple"] = "Basic member portal";
            textos["seccionSeguridad"] = "Security";
            textos["seguridadMenu"] = "Security";
            textos["sociosItem"] = "Members";
            textos["tituloConfirmarEliminarIdioma"] = "Confirm deletion";
            textos["tituloIdiomas"] = "Manage languages and translations";
            textos["ventasItem"] = "Sales";
            return textos;
        }

        private Dictionary<string, string> ObtenerTextosBaseSistema()
        {
            return ObtenerTextosEspanolBase();
        }

        private string ObtenerNombreAmigable(string nombreControl)
        {
            Dictionary<string, string> textos = ObtenerTextosEspanolBase();
            if (textos.ContainsKey(nombreControl))
            {
                return textos[nombreControl];
            }

            return nombreControl;
        }

        private List<string> ObtenerControles(Dictionary<string, string> baseUno, Dictionary<string, string> baseDos)
        {
            Dictionary<string, bool> claves = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);

            foreach (string key in baseUno.Keys)
            {
                if (!claves.ContainsKey(key)) claves.Add(key, true);
            }

            foreach (string key in baseDos.Keys)
            {
                if (!claves.ContainsKey(key)) claves.Add(key, true);
            }

            List<string> controles = new List<string>(claves.Keys);
            controles.Sort(StringComparer.OrdinalIgnoreCase);
            return controles;
        }

        private void ConfigurarColumnasGrilla(bool nuevoIdioma)
        {
            if (dgvTraducciones.Columns.Contains(ColControl))
            {
                dgvTraducciones.Columns[ColControl].HeaderText = TraducirLocal("colControl", "Clave interna", "Internal key");
                dgvTraducciones.Columns[ColControl].ReadOnly = true;
                dgvTraducciones.Columns[ColControl].Visible = false;
            }

            if (dgvTraducciones.Columns.Contains(ColElemento))
            {
                dgvTraducciones.Columns[ColElemento].HeaderText = TraducirLocal("colElemento", "Texto / pantalla", "Text / screen");
                dgvTraducciones.Columns[ColElemento].ReadOnly = true;
            }

            if (dgvTraducciones.Columns.Contains(ColTextoBase))
            {
                dgvTraducciones.Columns[ColTextoBase].HeaderText = TraducirLocal("colTextoBase", "Texto original en español", "Original Spanish text");
                dgvTraducciones.Columns[ColTextoBase].ReadOnly = true;
            }

            if (dgvTraducciones.Columns.Contains(ColTraduccion))
            {
                dgvTraducciones.Columns[ColTraduccion].HeaderText = nuevoIdioma
                    ? TraducirLocal("colNuevaTraduccion", "Nueva traducción", "New translation")
                    : TraducirLocal("colTraduccion", "Texto del idioma seleccionado", "Selected language text");
                dgvTraducciones.Columns[ColTraduccion].ReadOnly = idIdiomaSeleccionado == 1;
            }
        }

        private void btnPrepararNuevoIdioma_Click(object sender, EventArgs e)
        {
            PrepararTablaNuevoIdioma();
        }

        private void btnCrearIdioma_Click(object sender, EventArgs e)
        {
            try
            {
                if (!usuarioActualEsAdministrador)
                {
                    MessageBox.Show(TraducirLocal("msgSoloAdminIdiomas", "Solo un usuario administrador puede crear nuevos idiomas.", "Only an administrator can create new languages."));
                    return;
                }

                if (!modoNuevoIdioma)
                {
                    PrepararTablaNuevoIdioma();
                    return;
                }

                string nombreIdioma = txtNuevoIdioma.Text.Trim();
                int idNuevoIdioma = idiomaBLL.AltaIdioma(nombreIdioma, ObtenerUsuarioActual());
                GuardarFilasTraduccion(idNuevoIdioma, true);

                txtNuevoIdioma.Text = string.Empty;
                modoNuevoIdioma = false;
                CargarIdiomas(idNuevoIdioma);

                IdiomaBE nuevoIdioma = ObtenerIdiomaSeleccionado();
                if (nuevoIdioma != null)
                {
                    TratamientoIdioma.Instancia.IdiomaActual = nuevoIdioma;
                    TratamientoIdioma.Instancia.Notificar();
                }

                MessageBox.Show(TraducirLocal("msgIdiomaCreado", "Idioma creado correctamente y disponible en los menús.", "Language created and available in menus."));
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminarIdioma_Click(object sender, EventArgs e)
        {
            try
            {
                if (!usuarioActualEsAdministrador)
                {
                    MessageBox.Show(TraducirLocal("msgSoloAdminIdiomas", "Solo un usuario administrador puede eliminar idiomas.", "Only an administrator can delete languages."));
                    return;
                }

                IdiomaBE idioma = ObtenerIdiomaSeleccionado();
                if (idioma == null || idioma.Id <= 0)
                {
                    MessageBox.Show(TraducirLocal("msgSeleccionarIdioma", "Seleccionar idioma.", "Select a language."));
                    return;
                }

                if (idioma.Id == 1 || idioma.Id == 2)
                {
                    MessageBox.Show(TraducirLocal("msgNoEliminarIdiomaBase", "No se pueden eliminar Español ni English porque son idiomas base del sistema.", "Spanish and English cannot be deleted because they are system base languages."));
                    return;
                }

                DialogResult confirmacion = MessageBox.Show(
                    TraducirLocal("msgConfirmarEliminarIdioma", "Se eliminará el idioma seleccionado y todas sus traducciones. ¿Desea continuar?", "The selected language and all its translations will be deleted. Do you want to continue?"),
                    TraducirLocal("tituloConfirmarEliminarIdioma", "Confirmar eliminación", "Confirm deletion"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (confirmacion != DialogResult.Yes)
                {
                    return;
                }

                int idEliminado = idioma.Id;
                idiomaBLL.BajaIdioma(idEliminado, ObtenerUsuarioActual());

                if (TratamientoIdioma.Instancia.IdiomaActual != null && TratamientoIdioma.Instancia.IdiomaActual.Id == idEliminado)
                {
                    TratamientoIdioma.Instancia.IdiomaActual = null;
                }

                CargarIdiomas(1);

                IdiomaBE idiomaActual = ObtenerIdiomaSeleccionado();
                if (idiomaActual != null)
                {
                    TratamientoIdioma.Instancia.IdiomaActual = idiomaActual;
                    TratamientoIdioma.Instancia.Notificar();
                }

                MessageBox.Show(TraducirLocal("msgIdiomaEliminado", "Idioma eliminado correctamente.", "Language deleted successfully."));
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGuardarTraducciones_Click(object sender, EventArgs e)
        {
            try
            {
                if (!usuarioActualEsAdministrador)
                {
                    MessageBox.Show(TraducirLocal("msgSoloAdminTraducciones", "Solo un usuario administrador puede editar traducciones.", "Only an administrator can edit translations."));
                    return;
                }

                IdiomaBE idioma = ObtenerIdiomaSeleccionado();
                if (idioma == null || idioma.Id <= 0)
                {
                    MessageBox.Show(TraducirLocal("msgSeleccionarIdioma", "Seleccionar idioma.", "Select a language."));
                    return;
                }

                GuardarFilasTraduccion(idioma.Id, false);

                if (TratamientoIdioma.Instancia.IdiomaActual != null && TratamientoIdioma.Instancia.IdiomaActual.Id == idioma.Id)
                {
                    TratamientoIdioma.Instancia.Notificar();
                }

                CargarTraduccionesIdioma(idioma.Id);
                MessageBox.Show(TraducirLocal("msgTraduccionesGuardadas", "Traducciones guardadas correctamente.", "Translations saved successfully."));
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GuardarFilasTraduccion(int idIdioma, bool usarTextoBaseSiVacio)
        {
            if (idIdioma == 1)
            {
                throw new Exception(TraducirLocal("msgNoEditarEspanolBase", "Español es el idioma base del sistema. Use el script de restauración si necesita corregirlo.", "Spanish is the system base language. Use the restore script if you need to fix it."));
            }

            DataTable tabla = dgvTraducciones.DataSource as DataTable;
            if (tabla == null)
            {
                return;
            }

            foreach (DataRow row in tabla.Rows)
            {
                string nombreControl = Convert.ToString(row[ColControl]).Trim();
                string textoBase = Convert.ToString(row[ColTextoBase]).Trim();
                string traduccion = Convert.ToString(row[ColTraduccion]).Trim();

                if (string.IsNullOrWhiteSpace(nombreControl))
                {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(traduccion) && usarTextoBaseSiVacio)
                {
                    traduccion = textoBase;
                }

                if (string.IsNullOrWhiteSpace(traduccion))
                {
                    continue;
                }

                traduccionBLL.GuardarTraduccion(idIdioma, nombreControl, traduccion, ObtenerUsuarioActual());
            }
        }

        private IdiomaBE ObtenerIdiomaSeleccionado()
        {
            return cmbIdiomas.SelectedItem as IdiomaBE;
        }

        private void cmbIdiomas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cargandoIdiomas)
            {
                return;
            }

            IdiomaBE idioma = ObtenerIdiomaSeleccionado();
            if (idioma == null)
            {
                return;
            }

            if (usuarioActualEsAdministrador)
            {
                CargarTraduccionesIdioma(idioma.Id);
            }

            TratamientoIdioma.Instancia.IdiomaActual = idioma;
            TratamientoIdioma.Instancia.Notificar();
            ActualizarIdioma();
        }

        public void ActualizarIdioma()
        {
            Text = TraducirLocal("frmIdiomas", "Gestión de idiomas", "Language management");
            lblTitulo.Text = TraducirLocal("tituloIdiomas", "Administrar idiomas y traducciones", "Manage languages and translations");
            lblIdiomaExistente.Text = TraducirLocal("lblIdiomaExistente", "Idioma existente", "Existing language");
            lblNuevoIdioma.Text = TraducirLocal("lblNuevoIdioma", "Nuevo idioma", "New language");
            btnPrepararNuevoIdioma.Text = TraducirLocal("btnPrepararNuevoIdioma", "Preparar tabla", "Prepare table");
            btnCrearIdioma.Text = TraducirLocal("btnCrearIdioma", "Crear idioma", "Create language");
            btnEliminarIdioma.Text = TraducirLocal("btnEliminarIdioma", "Eliminar idioma", "Delete language");
            btnGuardarTraducciones.Text = TraducirLocal("btnGuardarTraducciones", "Guardar cambios", "Save changes");
            btnVolver.Text = TraducirLocal("btnVolverMenu", "Volver al menú", "Back to menu");
            lblAyuda.Text = TraducirLocal("lblAyudaIdiomas", "Seleccione un idioma para editar sus textos. Para crear uno nuevo, escriba el nombre, prepare la tabla, complete la columna Nueva traducción y presione Crear idioma. El texto original en español se muestra solo como referencia y no se modifica desde esta pantalla.", "Select a language to edit its texts. To create a new one, type its name, prepare the table, complete the New translation column, and press Create language. The original Spanish text is shown only as reference and is not modified from this screen.");
            lblPermisoAdministrador.Text = TraducirLocal("lblPermisoIdiomas", "Cambio de idioma disponible. La administración de traducciones queda reservada al administrador.", "Language change available. Translation administration is reserved to the administrator.");
            ConfigurarColumnasGrilla(modoNuevoIdioma);
        }

        private string TraducirLocal(string control, string textoEspanol, string textoIngles)
        {
            try
            {
                if (TratamientoIdioma.Instancia.IdiomaActual != null)
                {
                    List<TraduccionBE> traducciones = traduccionBLL.Listar(TratamientoIdioma.Instancia.IdiomaActual.Id);
                    foreach (TraduccionBE traduccion in traducciones)
                    {
                        if (string.Equals(traduccion.NombreControl, control, StringComparison.OrdinalIgnoreCase))
                        {
                            return traduccion.Traduccion;
                        }
                    }
                }
            }
            catch
            {
            }

            return IdiomaActualEsIngles() ? textoIngles : textoEspanol;
        }

        private bool IdiomaActualEsIngles()
        {
            IdiomaBE idiomaActual = TratamientoIdioma.Instancia.IdiomaActual;
            return idiomaActual != null &&
                   (idiomaActual.Id == 2 || string.Equals(idiomaActual.Nombre, "English", StringComparison.OrdinalIgnoreCase));
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            TratamientoIdioma.Instancia.Desuscribir(this);
            base.OnFormClosed(e);
        }

        private string ObtenerUsuarioActual()
        {
            return SessionManager.SesionIniciada && SessionManager.ObtenerUsuarioActual() != null
                ? SessionManager.ObtenerUsuarioActual().Username
                : "SIN_SESION";
        }
    }
}
