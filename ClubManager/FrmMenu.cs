using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using BE;
using BLL;
using SERVICIOS;
using SERVICIOS.Composite;
using SERVICIOS.Observer;

namespace ClubManager
{
    public partial class FrmMenu : Form, IOberverIdioma
    {
        private FlowLayoutPanel panelAccesos;
        private ComboBox cmbIdiomaMenu;
        private Label lblIdiomaMenu;
        private bool cargandoIdiomaMenu;

        public FrmMenu()
        {
            InitializeComponent();
            Width = 1120;
            Height = 760;
            MinimumSize = new Size(1000, 700);
            StartPosition = FormStartPosition.CenterScreen;
            VisualStyleHelper.AplicarEstiloBase(this);
            ConfigurarMenuPrincipal();
            ConfigurarPanelPrincipal();
            ConfigurarSelectorIdiomaMenu();
            TratamientoIdioma.Instancia.Suscribir(this);
            if (TratamientoIdioma.Instancia.IdiomaActual != null)
            {
                ActualizarIdioma();
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams parametros = base.CreateParams;
                parametros.ExStyle |= 0x02000000; // WS_EX_COMPOSITED: reduce parpadeo/redibujado.
                return parametros;
            }
        }

        private void ConfigurarMenuPrincipal()
        {
            // La navegación principal queda en botones visibles.
            // Los menús superiores Seguridad/Club resultaban confusos porque duplicaban opciones.
            menuStrip1.Items.Clear();
            menuStrip1.Visible = false;

            if (menuStrip2 != null)
            {
                menuStrip2.Visible = false;
            }
        }

        private void ConfigurarMenuAdministrador()
        {
            ToolStripMenuItem seguridadMenu = CrearMenuItem("seguridadMenu", "Seguridad", null);
            seguridadMenu.DropDownItems.Add(CrearMenuItem("bitacoraItem", "Bitácora", bitacoraItem_Click));
            seguridadMenu.DropDownItems.Add(CrearMenuItem("perfilesItem", "Perfiles", perfilesItem_Click));
            seguridadMenu.DropDownItems.Add(CrearMenuItem("controlCambiosItem", "Control de cambios", controlCambiosItem_Click));
            seguridadMenu.DropDownItems.Add(CrearMenuItem("idiomasItem", "Administrar idiomas", idiomasItem_Click));
            seguridadMenu.DropDownItems.Add(CrearMenuItem("logoutItem", "Cerrar sesión", logoutItem_Click));

            ToolStripMenuItem clubMenu = CrearMenuItem("clubMenu", "Club", null);
            clubMenu.DropDownItems.Add(CrearMenuItem("sociosItem", "Gestión de socios", sociosItem_Click));
            clubMenu.DropDownItems.Add(CrearMenuItem("pagosItem", "Pagos y cuotas", pagosItem_Click));
            clubMenu.DropDownItems.Add(CrearMenuItem("configuracionCuotasItem", "Configuración de cuotas y fees", configuracionCuotasItem_Click));
            clubMenu.DropDownItems.Add(CrearMenuItem("jugadoresItem", "Jugadores", jugadoresItem_Click));
            clubMenu.DropDownItems.Add(CrearMenuItem("eventosItem", "Eventos deportivos", eventosItem_Click));
            clubMenu.DropDownItems.Add(CrearMenuItem("finanzasItem", "Ingresos y egresos", finanzasItem_Click));
            clubMenu.DropDownItems.Add(CrearMenuItem("publicacionesItem", "Comunicación interna", publicacionesItem_Click));
            clubMenu.DropDownItems.Add(CrearMenuItem("insigniasItem", "Insignias", insigniasItem_Click));
            clubMenu.DropDownItems.Add(CrearMenuItem("reportesItem", "Reportes", reportesItem_Click));
            clubMenu.DropDownItems.Add(CrearMenuItem("inventarioItem", "Inventario", inventarioItem_Click));
            clubMenu.DropDownItems.Add(CrearMenuItem("ventasItem", "Ventas", ventasItem_Click));
            clubMenu.DropDownItems.Add(CrearMenuItem("convocatoriasItem", "Convocatorias", convocatoriasItem_Click));
            clubMenu.DropDownItems.Add(CrearMenuItem("resultadosItem", "Resultados", resultadosItem_Click));

            menuStrip1.Items.Add(seguridadMenu);
            menuStrip1.Items.Add(clubMenu);
        }

        private void ConfigurarMenuSocio()
        {
            ToolStripMenuItem portalMenu = CrearMenuItem("portalSocioMenu", "Portal socio", null);
            portalMenu.DropDownItems.Add(CrearMenuItem("miPerfilItem", "Mi perfil", miPerfilItem_Click));
            portalMenu.DropDownItems.Add(CrearMenuItem("misPagosItem", "Mis pagos", misPagosItem_Click));
            portalMenu.DropDownItems.Add(CrearMenuItem("eventosDisponiblesItem", "Eventos disponibles", eventosDisponiblesItem_Click));
            portalMenu.DropDownItems.Add(CrearMenuItem("misEntradasItem", "Comprar entrada / Mis entradas", misEntradasItem_Click));
            portalMenu.DropDownItems.Add(CrearMenuItem("comunicadosItem", "Comunicados", comunicadosItem_Click));
            portalMenu.DropDownItems.Add(CrearMenuItem("miHistorialItem", "Mi historial", miHistorialItem_Click));
            portalMenu.DropDownItems.Add(CrearMenuItem("historialMailItem", "Historial de mail", historialMailItem_Click));

            if (UsuarioActualEsSocioPleno())
            {
                portalMenu.DropDownItems.Add(CrearMenuItem("equiposDisponibilidadItem", "Equipos y disponibilidad", equiposDisponibilidadItem_Click));
                portalMenu.DropDownItems.Add(CrearMenuItem("misConvocatoriasItem", "Mis convocatorias", misConvocatoriasItem_Click));
                portalMenu.DropDownItems.Add(CrearMenuItem("misInsigniasItem", "Mis insignias", misInsigniasItem_Click));
                portalMenu.DropDownItems.Add(CrearMenuItem("resultadosItem", "Resultados", resultadosItem_Click));
            }

            ToolStripMenuItem cuentaMenu = CrearMenuItem("cuentaMenu", "Cuenta", null);
            cuentaMenu.DropDownItems.Add(CrearMenuItem("logoutItem", "Cerrar sesión", logoutItem_Click));

            menuStrip1.Items.Add(portalMenu);
            menuStrip1.Items.Add(cuentaMenu);
        }


        private void ConfigurarSelectorIdiomaMenu()
        {
            try
            {
                cargandoIdiomaMenu = true;

                if (lblIdiomaMenu == null)
                {
                    lblIdiomaMenu = new Label();
                    lblIdiomaMenu.AutoSize = false;
                    lblIdiomaMenu.Width = 70;
                    lblIdiomaMenu.Height = 24;
                    lblIdiomaMenu.ForeColor = Color.White;
                    lblIdiomaMenu.BackColor = Color.Transparent;
                    lblIdiomaMenu.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                    Controls.Add(lblIdiomaMenu);
                }

                if (cmbIdiomaMenu == null)
                {
                    cmbIdiomaMenu = new ComboBox();
                    cmbIdiomaMenu.Width = 170;
                    cmbIdiomaMenu.DropDownStyle = ComboBoxStyle.DropDownList;
                    cmbIdiomaMenu.SelectedIndexChanged += cmbIdiomaMenu_SelectedIndexChanged;
                    Controls.Add(cmbIdiomaMenu);
                }

                lblIdiomaMenu.Text = Traducir("lblIdioma", "Idioma");

                RecargarIdiomasSelectorMenu();

                PosicionarSelectorIdiomaMenu();
                Resize -= FrmMenu_Resize;
                Resize += FrmMenu_Resize;

                lblIdiomaMenu.BringToFront();
                cmbIdiomaMenu.BringToFront();
            }
            catch
            {
                if (lblIdiomaMenu != null) lblIdiomaMenu.Visible = false;
                if (cmbIdiomaMenu != null) cmbIdiomaMenu.Visible = false;
            }
            finally
            {
                cargandoIdiomaMenu = false;
            }
        }

        private void RecargarIdiomasSelectorMenu()
        {
            if (cmbIdiomaMenu == null)
            {
                return;
            }

            try
            {
                int idSeleccionado = 0;
                if (TratamientoIdioma.Instancia.IdiomaActual != null)
                {
                    idSeleccionado = TratamientoIdioma.Instancia.IdiomaActual.Id;
                }

                cargandoIdiomaMenu = true;
                IdiomaBLL idiomaBLL = new IdiomaBLL();
                List<IdiomaBE> idiomas = idiomaBLL.ListarIdiomas();
                cmbIdiomaMenu.DataSource = null;
                cmbIdiomaMenu.DataSource = idiomas;
                cmbIdiomaMenu.DisplayMember = "Nombre";
                cmbIdiomaMenu.ValueMember = "Id";

                if (idSeleccionado > 0)
                {
                    cmbIdiomaMenu.SelectedValue = idSeleccionado;
                }
            }
            catch
            {
            }
            finally
            {
                cargandoIdiomaMenu = false;
            }
        }

        private void PosicionarSelectorIdiomaMenu()
        {
            if (lblIdiomaMenu == null || cmbIdiomaMenu == null)
            {
                return;
            }

            int top = 18;
            cmbIdiomaMenu.Left = ClientSize.Width - cmbIdiomaMenu.Width - 30;
            cmbIdiomaMenu.Top = top;
            lblIdiomaMenu.Left = cmbIdiomaMenu.Left - lblIdiomaMenu.Width - 8;
            lblIdiomaMenu.Top = top + 3;
            lblIdiomaMenu.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cmbIdiomaMenu.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        }

        private void FrmMenu_Resize(object sender, EventArgs e)
        {
            PosicionarSelectorIdiomaMenu();
        }

        private void cmbIdiomaMenu_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cargandoIdiomaMenu || cmbIdiomaMenu.SelectedItem == null)
            {
                return;
            }

            TratamientoIdioma.Instancia.IdiomaActual = (IdiomaBE)cmbIdiomaMenu.SelectedItem;
            TratamientoIdioma.Instancia.Notificar();
        }

        private void ConfigurarPanelPrincipal()
        {
            SuspendLayout();

            if (panelAccesos != null)
            {
                Controls.Remove(panelAccesos);
                panelAccesos.Dispose();
            }

            panelAccesos = new FlowLayoutPanel();
            VisualStyleHelper.HabilitarDobleBuffer(panelAccesos);
            panelAccesos.SuspendLayout();
            panelAccesos.Dock = DockStyle.Fill;
            panelAccesos.FlowDirection = FlowDirection.LeftToRight;
            panelAccesos.WrapContents = true;
            panelAccesos.AutoScroll = true;
            panelAccesos.Padding = UsuarioActualEsSocioPleno() && !UsuarioActualEsAdministrador()
                ? new Padding(48, 62, 35, 35)
                : new Padding(60, 80, 45, 45);
            panelAccesos.BackColor = Color.Transparent;

            UsuarioBE usuarioActual = SessionManager.ObtenerUsuarioActual();
            string nombreUsuario = usuarioActual != null ? usuarioActual.Username : string.Empty;
            string rolUsuario = ObtenerNombreRolActual();

            Label titulo = new Label();
            titulo.AutoSize = false;
            titulo.Width = 960;
            titulo.Height = 42;
            titulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            titulo.ForeColor = Color.White;
            titulo.BackColor = Color.FromArgb(140, 0, 0, 0);
            titulo.Text = Traducir("frmMenu", "Menú Principal") + (string.IsNullOrWhiteSpace(nombreUsuario) ? string.Empty : " - " + nombreUsuario);
            titulo.Margin = new Padding(0, 0, 0, 8);
            panelAccesos.Controls.Add(titulo);

            Label subtitulo = new Label();
            subtitulo.AutoSize = false;
            subtitulo.Width = 960;
            subtitulo.Height = 28;
            subtitulo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            subtitulo.ForeColor = Color.White;
            subtitulo.BackColor = Color.FromArgb(140, 0, 0, 0);
            subtitulo.Text = Traducir("lblRol", "Rol") + ": " + TraducirRol(rolUsuario);
            subtitulo.Margin = UsuarioActualEsSocioPleno() && !UsuarioActualEsAdministrador() ? new Padding(0, 0, 0, 12) : new Padding(0, 0, 0, 22);
            panelAccesos.Controls.Add(subtitulo);

            if (UsuarioActualEsSocioPleno())
            {
                AgregarResumenInsigniasSocioPleno();
            }

            if (UsuarioActualEsAdministrador())
            {
                AgregarBotonesAdministrador();
            }
            else
            {
                AgregarBotonesSocio();
            }

            Controls.Add(panelAccesos);
            panelAccesos.BringToFront();
            menuStrip1.BringToFront();
            if (lblIdiomaMenu != null) lblIdiomaMenu.BringToFront();
            if (cmbIdiomaMenu != null) cmbIdiomaMenu.BringToFront();
            panelAccesos.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }


        private void AgregarResumenInsigniasSocioPleno()
        {
            try
            {
                UsuarioBE usuarioActual = SessionManager.ObtenerUsuarioActual();
                if (usuarioActual == null || usuarioActual.IdSocio <= 0)
                {
                    return;
                }

                AgregarTituloSeccion(Traducir("seccionInsigniasDestacadas", "Insignias destacadas"));

                FlowLayoutPanel panelInsignias = new FlowLayoutPanel();
                panelInsignias.Width = 960;
                panelInsignias.Height = 116;
                panelInsignias.FlowDirection = FlowDirection.LeftToRight;
                panelInsignias.WrapContents = false;
                panelInsignias.BackColor = Color.FromArgb(110, 0, 0, 0);
                panelInsignias.Margin = new Padding(0, 0, 0, 10);
                panelInsignias.Padding = new Padding(8, 8, 8, 8);

                ModuloClubBLL moduloBLL = new ModuloClubBLL();
                DataTable insignias = moduloBLL.ConsultarInsigniasCalculadasSocio(usuarioActual.IdSocio);

                foreach (DataRow fila in insignias.Rows)
                {
                    panelInsignias.Controls.Add(CrearTarjetaInsigniaResumen(fila));
                }

                panelAccesos.Controls.Add(panelInsignias);
            }
            catch
            {
                // El menú no debe quedar inutilizable si falla el cálculo de insignias.
            }
        }

        private Panel CrearTarjetaInsigniaResumen(DataRow fila)
        {
            Panel tarjeta = new Panel();
            tarjeta.Width = 225;
            tarjeta.Height = 96;
            tarjeta.BackColor = Color.FromArgb(80, 75, 12, 24);
            tarjeta.Margin = new Padding(4, 4, 8, 4);

            PictureBox imagen = new PictureBox();
            imagen.Left = 8;
            imagen.Top = 8;
            imagen.Width = 60;
            imagen.Height = 60;
            imagen.SizeMode = PictureBoxSizeMode.Zoom;
            imagen.BackColor = Color.Transparent;

            Image imagenInsignia = CargarImagenInsignia(ObtenerValorFila(fila, "Imagen"));
            if (imagenInsignia != null)
            {
                imagen.Image = imagenInsignia;
            }

            Label nivel = new Label();
            nivel.Left = 48;
            nivel.Top = 48;
            nivel.Width = 28;
            nivel.Height = 22;
            nivel.TextAlign = ContentAlignment.MiddleCenter;
            nivel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            nivel.ForeColor = Color.White;
            nivel.BackColor = Color.FromArgb(150, 75, 12, 24);
            nivel.Text = ObtenerValorFila(fila, "Nivel");

            Label titulo = new Label();
            titulo.Left = 78;
            titulo.Top = 8;
            titulo.Width = 132;
            titulo.Height = 30;
            titulo.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            titulo.ForeColor = Color.White;
            titulo.BackColor = Color.Transparent;
            titulo.Text = ObtenerValorFila(fila, "TituloNivel");

            Label progreso = new Label();
            progreso.Left = 78;
            progreso.Top = 40;
            progreso.Width = 132;
            progreso.Height = 26;
            progreso.Font = new Font("Segoe UI", 7F, FontStyle.Regular);
            progreso.ForeColor = Color.Gainsboro;
            progreso.BackColor = Color.Transparent;
            progreso.Text = TraducirProgresoInsignia(ObtenerValorFila(fila, "Progreso"));

            Label estado = new Label();
            estado.Left = 8;
            estado.Top = 68;
            estado.Width = 204;
            estado.Height = 20;
            estado.TextAlign = ContentAlignment.MiddleCenter;
            estado.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold);
            estado.ForeColor = Color.White;
            estado.BackColor = string.Equals(ObtenerValorFila(fila, "Estado"), "Obtenida", StringComparison.OrdinalIgnoreCase)
                ? Color.FromArgb(105, 130, 20)
                : Color.FromArgb(80, 80, 80);
            estado.Text = TraducirEstadoInsignia(ObtenerValorFila(fila, "Estado"));

            tarjeta.Controls.Add(imagen);
            tarjeta.Controls.Add(nivel);
            tarjeta.Controls.Add(titulo);
            tarjeta.Controls.Add(progreso);
            tarjeta.Controls.Add(estado);

            return tarjeta;
        }

        private Image CargarImagenInsignia(string nombreArchivo)
        {
            if (string.IsNullOrWhiteSpace(nombreArchivo))
            {
                return null;
            }

            string ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Insignias", nombreArchivo);
            if (!File.Exists(ruta))
            {
                return null;
            }

            using (FileStream stream = new FileStream(ruta, FileMode.Open, FileAccess.Read))
            using (Image imagen = Image.FromStream(stream))
            {
                return new Bitmap(imagen);
            }
        }

        private string ObtenerValorFila(DataRow fila, string columna)
        {
            if (fila == null || !fila.Table.Columns.Contains(columna) || fila[columna] == DBNull.Value)
            {
                return string.Empty;
            }

            return fila[columna].ToString();
        }

        private void AgregarBotonesAdministrador()
        {
            AgregarTituloSeccion(Traducir("seccionAdministracion", "Administración"));
            AgregarBoton(Traducir("sociosItem", "Gestión de socios"), sociosItem_Click);
            AgregarBoton(Traducir("pagosItem", "Pagos y cuotas"), pagosItem_Click);
            AgregarBoton(Traducir("configuracionCuotasItem", "Configuración de cuotas y fees"), configuracionCuotasItem_Click);
            AgregarBoton(Traducir("jugadoresItem", "Jugadores"), jugadoresItem_Click);
            AgregarBoton(Traducir("eventosItem", "Eventos deportivos"), eventosItem_Click);
            AgregarBoton(Traducir("finanzasItem", "Ingresos y egresos"), finanzasItem_Click);
            AgregarBoton(Traducir("publicacionesItem", "Comunicación interna"), publicacionesItem_Click);
            AgregarBoton(Traducir("insigniasItem", "Insignias"), insigniasItem_Click);
            AgregarBoton(Traducir("reportesItem", "Reportes"), reportesItem_Click);
            AgregarBoton(Traducir("inventarioItem", "Inventario"), inventarioItem_Click);
            AgregarBoton(Traducir("ventasItem", "Ventas"), ventasItem_Click);
            AgregarBoton(Traducir("convocatoriasItem", "Convocatorias"), convocatoriasItem_Click);
            AgregarBoton(Traducir("resultadosItem", "Resultados"), resultadosItem_Click);
            AgregarSeparadorPanel();
            AgregarTituloSeccion(Traducir("seccionSeguridad", "Seguridad"));
            AgregarBoton(Traducir("bitacoraItem", "Bitácora"), bitacoraItem_Click);
            AgregarBoton(Traducir("perfilesItem", "Perfiles"), perfilesItem_Click);
            AgregarBoton(Traducir("controlCambiosItem", "Control de cambios"), controlCambiosItem_Click);
            AgregarBoton(Traducir("idiomasItem", "Administrar idiomas y traducciones"), idiomasItem_Click);
            AgregarBoton(Traducir("logoutItem", "Cerrar sesión"), logoutItem_Click);
        }

        private void AgregarBotonesSocio()
        {
            AgregarTituloSeccion(UsuarioActualEsSocioPleno()
                ? Traducir("seccionPortalSocioPleno", "Portal de socio pleno")
                : Traducir("seccionPortalSocioSimple", "Portal de socio simple"));
            AgregarBoton(Traducir("miPerfilItem", "Mi perfil"), miPerfilItem_Click);
            AgregarBoton(Traducir("misPagosItem", "Pagar / ver mis cuotas"), misPagosItem_Click);
            AgregarBoton(Traducir("eventosDisponiblesItem", "Eventos disponibles"), eventosDisponiblesItem_Click);
            AgregarBoton(Traducir("misEntradasItem", "Comprar entrada / Mis entradas"), misEntradasItem_Click);
            AgregarBoton(Traducir("comunicadosItem", "Comunicados"), comunicadosItem_Click);
            AgregarBoton(Traducir("miHistorialItem", "Mi historial"), miHistorialItem_Click);
            AgregarBoton(Traducir("historialMailItem", "Historial de mail"), historialMailItem_Click);

            if (UsuarioActualEsSocioPleno())
            {
                AgregarSeparadorPanel();
                AgregarTituloSeccion(Traducir("seccionParticipacionDeportiva", "Participación deportiva"));
                AgregarBoton(Traducir("equiposDisponibilidadItem", "Equipos y disponibilidad"), equiposDisponibilidadItem_Click);
                AgregarBoton(Traducir("misConvocatoriasItem", "Mis convocatorias"), misConvocatoriasItem_Click);
                AgregarBoton(Traducir("misInsigniasItem", "Mis insignias"), misInsigniasItem_Click);
                AgregarBoton(Traducir("resultadosItem", "Resultados"), resultadosItem_Click);
            }

            AgregarSeparadorPanel();
            AgregarBoton(Traducir("logoutItem", "Cerrar sesión"), logoutItem_Click);
        }

        private void AgregarTituloSeccion(string texto)
        {
            Label label = new Label();
            label.AutoSize = false;
            label.Width = 960;
            label.Height = 22;
            label.Text = texto;
            label.ForeColor = Color.White;
            label.BackColor = Color.FromArgb(130, 0, 0, 0);
            label.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label.Margin = new Padding(0, 5, 0, 5);
            panelAccesos.Controls.Add(label);
        }

        private void AgregarBoton(string texto, EventHandler accionClick)
        {
            Button boton = new Button();
            boton.Text = texto;
            boton.Width = UsuarioActualEsSocioPleno() && !UsuarioActualEsAdministrador() ? 220 : 260;
            boton.Height = UsuarioActualEsSocioPleno() && !UsuarioActualEsAdministrador() ? 34 : 38;
            boton.Margin = UsuarioActualEsSocioPleno() && !UsuarioActualEsAdministrador() ? new Padding(0, 0, 8, 7) : new Padding(0, 0, 0, 8);
            boton.TextAlign = ContentAlignment.MiddleCenter;
            boton.BackColor = Color.FromArgb(75, 12, 24);
            boton.ForeColor = Color.White;
            boton.FlatStyle = FlatStyle.Flat;
            boton.FlatAppearance.BorderColor = Color.White;
            boton.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            if (accionClick != null)
            {
                boton.Click += accionClick;
            }

            panelAccesos.Controls.Add(boton);
        }

        private void AgregarSeparadorPanel()
        {
            Label separador = new Label();
            separador.AutoSize = false;
            separador.Width = 960;
            separador.Height = UsuarioActualEsSocioPleno() && !UsuarioActualEsAdministrador() ? 4 : 10;
            separador.Margin = UsuarioActualEsSocioPleno() && !UsuarioActualEsAdministrador() ? new Padding(0, 2, 0, 2) : new Padding(0, 6, 0, 6);
            separador.BackColor = Color.Transparent;
            panelAccesos.Controls.Add(separador);
        }

        private string Traducir(string nombreControl, string valorDefault)
        {
            try
            {
                if (TratamientoIdioma.Instancia.IdiomaActual != null)
                {
                    TraduccionBLL traduccionBLL = new TraduccionBLL();
                    List<TraduccionBE> traducciones = traduccionBLL.Listar(TratamientoIdioma.Instancia.IdiomaActual.Id);

                    foreach (TraduccionBE traduccion in traducciones)
                    {
                        if (traduccion.NombreControl == nombreControl)
                        {
                            return traduccion.Traduccion;
                        }
                    }
                }
            }
            catch
            {
            }

            return TraducirFallback(nombreControl, valorDefault);
        }

        private string TraducirFallback(string nombreControl, string valorDefault)
        {
            if (!IdiomaActualEsIngles())
            {
                return valorDefault;
            }

            Dictionary<string, string> ingles = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "frmMenu", "Main Menu" },
                { "lblIdioma", "Language" },
                { "lblRol", "Role" },
                { "portalSocioMenu", "Member portal" },
                { "cuentaMenu", "Account" },
                { "logoutItem", "Logout" },
                { "seccionInsigniasDestacadas", "Featured badges" },
                { "seccionPortalSocioSimple", "Basic member portal" },
                { "seccionPortalSocioPleno", "Full member portal" },
                { "seccionParticipacionDeportiva", "Sports participation" },
                { "seccionAdministracion", "Administration" },
                { "seccionSeguridad", "Security" },
                { "miPerfilItem", "My profile" },
                { "misPagosItem", "Pay / view my dues" },
                { "eventosDisponiblesItem", "Available events" },
                { "misEntradasItem", "Buy ticket / My tickets" },
                { "comunicadosItem", "Announcements" },
                { "miHistorialItem", "My history" },
                { "historialMailItem", "Mail history" },
                { "equiposDisponibilidadItem", "Teams and availability" },
                { "misConvocatoriasItem", "My call-ups" },
                { "misInsigniasItem", "My badges" },
                { "resultadosItem", "Results" },
                { "sociosItem", "Member management" },
                { "pagosItem", "Payments and dues" },
                { "configuracionCuotasItem", "Dues and fees setup" },
                { "jugadoresItem", "Players" },
                { "eventosItem", "Sports events" },
                { "finanzasItem", "Income and expenses" },
                { "publicacionesItem", "Internal communication" },
                { "insigniasItem", "Badges" },
                { "reportesItem", "Reports" },
                { "inventarioItem", "Inventory" },
                { "ventasItem", "Sales" },
                { "convocatoriasItem", "Call-ups" },
                { "bitacoraItem", "Audit log" },
                { "perfilesItem", "Profiles" },
                { "controlCambiosItem", "Change control" },
                { "idiomasItem", "Manage languages and translations" }
            };

            string traduccion;
            return ingles.TryGetValue(nombreControl, out traduccion) ? traduccion : valorDefault;
        }

        private bool IdiomaActualEsIngles()
        {
            IdiomaBE idiomaActual = TratamientoIdioma.Instancia.IdiomaActual;
            return idiomaActual != null &&
                   (idiomaActual.Id == 2 || string.Equals(idiomaActual.Nombre, "English", StringComparison.OrdinalIgnoreCase));
        }

        private string TraducirRol(string rol)
        {
            if (!IdiomaActualEsIngles())
            {
                return rol;
            }

            if (string.Equals(rol, "Administrador", StringComparison.OrdinalIgnoreCase)) return "Administrator";
            if (string.Equals(rol, "Socio Pleno", StringComparison.OrdinalIgnoreCase)) return "Full Member";
            if (string.Equals(rol, "Socio Simple", StringComparison.OrdinalIgnoreCase)) return "Basic Member";
            return rol;
        }

        private string TraducirEstadoInsignia(string estado)
        {
            if (!IdiomaActualEsIngles())
            {
                return estado;
            }

            if (string.Equals(estado, "Obtenida", StringComparison.OrdinalIgnoreCase)) return "Unlocked";
            if (string.Equals(estado, "Pendiente", StringComparison.OrdinalIgnoreCase)) return "Pending";
            return estado;
        }

        private string TraducirProgresoInsignia(string progreso)
        {
            if (!IdiomaActualEsIngles() || string.IsNullOrWhiteSpace(progreso))
            {
                return progreso;
            }

            return progreso
                .Replace("años de socio", "years as member")
                .Replace("partidos ganados", "matches won")
                .Replace("convocatorias asistidas", "call-ups attended")
                .Replace("deportes distintos", "different sports");
        }

        private bool UsuarioActualEsAdministrador()
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

        private bool UsuarioActualEsSocioPleno()
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

            return usuarioActual.IdRol == 3 || string.Equals(usuarioActual.NombreRol, "Socio Pleno", StringComparison.OrdinalIgnoreCase);
        }

        private string ObtenerNombreRolActual()
        {
            if (!SessionManager.SesionIniciada)
            {
                return string.Empty;
            }

            UsuarioBE usuarioActual = SessionManager.ObtenerUsuarioActual();

            if (usuarioActual == null)
            {
                return string.Empty;
            }

            if (!string.IsNullOrWhiteSpace(usuarioActual.NombreRol))
            {
                return usuarioActual.NombreRol;
            }

            if (usuarioActual.IdRol == 1) return "Administrador";
            if (usuarioActual.IdRol == 3) return "Socio Pleno";
            return "Socio Simple";
        }

        private ToolStripMenuItem CrearMenuItem(string nombre, string texto, EventHandler accionClick)
        {
            ToolStripMenuItem item = new ToolStripMenuItem(Traducir(nombre, texto));
            item.Name = nombre;
            item.ForeColor = Color.White;

            if (accionClick != null)
            {
                item.Click += accionClick;
            }

            return item;
        }

        private void AbrirFormularioHijo(Form formulario)
        {
            formulario.StartPosition = FormStartPosition.CenterScreen;
            formulario.FormClosed += delegate
            {
                BeginInvoke(new MethodInvoker(delegate
                {
                    Show();
                    Activate();
                    BringToFront();
                    Invalidate();
                }));
            };

            Hide();
            formulario.Show();
        }

        public void ActualizarIdioma()
        {
            try
            {
                if (TratamientoIdioma.Instancia.IdiomaActual == null)
                {
                    return;
                }

                Text = Traducir("frmMenu", "Menú Principal");
                ConfigurarMenuPrincipal();
                TraduccionBLL traduccionBLL = new TraduccionBLL();
                List<TraduccionBE> traducciones = traduccionBLL.Listar(TratamientoIdioma.Instancia.IdiomaActual.Id);

                foreach (TraduccionBE traduccion in traducciones)
                {
                    AplicarTraduccionMenu(menuStrip1.Items, traduccion.NombreControl, traduccion.Traduccion);

                    if (traduccion.NombreControl == "frmMenu")
                    {
                        Text = traduccion.Traduccion;
                    }
                }

                if (lblIdiomaMenu != null)
                {
                    lblIdiomaMenu.Text = Traducir("lblIdioma", "Idioma");
                }

                if (cmbIdiomaMenu != null && TratamientoIdioma.Instancia.IdiomaActual != null)
                {
                    RecargarIdiomasSelectorMenu();
                }

                ConfigurarPanelPrincipal();
                PosicionarSelectorIdiomaMenu();
            }
            catch
            {
            }
        }

        private void AplicarTraduccionMenu(ToolStripItemCollection items, string nombreControl, string texto)
        {
            foreach (ToolStripItem item in items)
            {
                if (item.Name == nombreControl)
                {
                    item.Text = texto;
                }

                ToolStripMenuItem menuItem = item as ToolStripMenuItem;
                if (menuItem != null && menuItem.DropDownItems.Count > 0)
                {
                    AplicarTraduccionMenu(menuItem.DropDownItems, nombreControl, texto);
                }
            }
        }

        private void bitacoraItem_Click(object sender, EventArgs e)
        {
            AbrirFormularioHijo(new FrmBitacora());
        }

        private void perfilesItem_Click(object sender, EventArgs e)
        {
            AbrirFormularioHijo(new FrmPerfiles());
        }

        private void controlCambiosItem_Click(object sender, EventArgs e)
        {
            AbrirFormularioHijo(new FrmControlCambios());
        }

        private void idiomasItem_Click(object sender, EventArgs e)
        {
            if (UsuarioActualEsAdministrador())
            {
                AbrirFormularioHijo(new FrmIdiomas());
            }
        }

        private void sociosItem_Click(object sender, EventArgs e)
        {
            AbrirFormularioHijo(new FrmSocios());
        }

        private void pagosItem_Click(object sender, EventArgs e)
        {
            AbrirFormularioHijo(new FrmModuloClub("Pagos"));
        }

        private void configuracionCuotasItem_Click(object sender, EventArgs e)
        {
            AbrirFormularioHijo(new FrmModuloClub("Cuotas y fees"));
        }

        private void jugadoresItem_Click(object sender, EventArgs e)
        {
            AbrirFormularioHijo(new FrmModuloClub("Jugadores"));
        }

        private void eventosItem_Click(object sender, EventArgs e)
        {
            AbrirFormularioHijo(new FrmModuloClub("Eventos"));
        }

        private void finanzasItem_Click(object sender, EventArgs e)
        {
            AbrirFormularioHijo(new FrmModuloClub("Finanzas"));
        }

        private void publicacionesItem_Click(object sender, EventArgs e)
        {
            AbrirFormularioHijo(new FrmModuloClub("Comunicación"));
        }

        private void insigniasItem_Click(object sender, EventArgs e)
        {
            AbrirFormularioHijo(new FrmInsigniasAdmin());
        }

        private void reportesItem_Click(object sender, EventArgs e)
        {
            AbrirFormularioHijo(new FrmModuloClub("Reportes"));
        }

        private void inventarioItem_Click(object sender, EventArgs e)
        {
            AbrirFormularioHijo(new FrmModuloClub("Inventario"));
        }

        private void ventasItem_Click(object sender, EventArgs e)
        {
            AbrirFormularioHijo(new FrmModuloClub("Ventas"));
        }

        private void convocatoriasItem_Click(object sender, EventArgs e)
        {
            AbrirFormularioHijo(new FrmModuloClub("Convocatorias"));
        }


        private void misInsigniasItem_Click(object sender, EventArgs e)
        {
            AbrirFormularioHijo(new FrmPortalSocio("Mis insignias"));
        }

        private void resultadosItem_Click(object sender, EventArgs e)
        {
            if (UsuarioActualEsAdministrador())
            {
                AbrirFormularioHijo(new FrmModuloClub("Resultados"));
            }
            else
            {
                AbrirFormularioHijo(new FrmPortalSocio("Resultados"));
            }
        }

        private void miPerfilItem_Click(object sender, EventArgs e)
        {
            AbrirFormularioHijo(new FrmPortalSocio("Mi perfil"));
        }

        private void misPagosItem_Click(object sender, EventArgs e)
        {
            AbrirFormularioHijo(new FrmPortalSocio("Mis pagos"));
        }

        private void eventosDisponiblesItem_Click(object sender, EventArgs e)
        {
            AbrirFormularioHijo(new FrmPortalSocio("Eventos disponibles"));
        }

        private void misEntradasItem_Click(object sender, EventArgs e)
        {
            AbrirFormularioHijo(new FrmPortalSocio("Mis entradas"));
        }

        private void comunicadosItem_Click(object sender, EventArgs e)
        {
            AbrirFormularioHijo(new FrmPortalSocio("Comunicados"));
        }

        private void miHistorialItem_Click(object sender, EventArgs e)
        {
            AbrirFormularioHijo(new FrmPortalSocio("Mi historial"));
        }

        private void historialMailItem_Click(object sender, EventArgs e)
        {
            AbrirFormularioHijo(new FrmPortalSocio("Historial de mail"));
        }

        private void equiposDisponibilidadItem_Click(object sender, EventArgs e)
        {
            AbrirFormularioHijo(new FrmPortalSocio("Equipos y disponibilidad"));
        }

        private void misConvocatoriasItem_Click(object sender, EventArgs e)
        {
            AbrirFormularioHijo(new FrmPortalSocio("Mis convocatorias"));
        }

        private void logoutItem_Click(object sender, EventArgs e)
        {
            try
            {
                UsuarioBLL usuarioBLL = new UsuarioBLL();
                usuarioBLL.Logout();
                Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            TratamientoIdioma.Instancia.Desuscribir(this);

            if (SessionManager.SesionIniciada)
            {
                try
                {
                    UsuarioBLL usuarioBLL = new UsuarioBLL();
                    usuarioBLL.Logout();
                }
                catch
                {
                }
            }

            base.OnFormClosing(e);
        }
    }
}
