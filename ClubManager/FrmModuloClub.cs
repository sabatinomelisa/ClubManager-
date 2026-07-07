using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using BLL;
using SERVICIOS;

namespace ClubManager
{
    public class FrmModuloClub : Form
    {
        private readonly string tipoModulo;
        private readonly ModuloClubBLL moduloClubBLL;
        private DataGridView dgvDatos;
        private TextBox txtCampoUno;
        private TextBox txtCampoDos;
        private TextBox txtCampoTres;
        private TextBox txtCampoCuatro;
        private Label lblCampoUno;
        private Label lblCampoDos;
        private Label lblCampoTres;
        private Label lblCampoCuatro;
        private Label lblAyuda;
        private Button btnGuardar;
        private Button btnActualizar;
        private Button btnVolver;
        private ComboBox cmbTipoMovimiento;
        private ComboBox cmbTipoPublicacion;
        private ComboBox cmbTipoVenta;
        private ComboBox cmbNivelInsignia;
        private ComboBox cmbEventoDeporte;
        private ComboBox cmbEventoLugar;
        private ComboBox cmbInventarioEstado;
        private ComboBox cmbRespuestaConvocatoria;
        private ComboBox cmbJugadorDeporte;
        private ComboBox cmbJugadorDisponible;
        private Label lblEventoCupo;
        private Label lblEventoEntrada;
        private Label lblEventoFee;
        private TextBox txtEventoCupo;
        private TextBox txtEventoEntrada;
        private TextBox txtEventoFee;
        private Button btnBuscarInventario;
        private Button btnSumarInventario;
        private Button btnRestarInventario;
        private Label lblBalance;
        private DataGridView dgvNivelesInsignia;
        private Button btnAgregarNivelInsignia;
        private Button btnCargarInsigniaCatalogo;
        private Button btnNuevoEvento;
        private int idEventoSeleccionado;
        private int idPublicacionSeleccionada;
        private int idInsigniaCatalogoSeleccionada;
        private int idSocioInsigniaSeleccionado;

        private ComboBox cmbInsigniasDisponibles;
        private Label lblNuevaInsignia;
        private Label lblNuevaInsigniaNombre;
        private Label lblNuevaInsigniaDescripcion;
        private Label lblNuevaInsigniaRequisitos;
        private Label lblNuevaInsigniaImagen;
        private TextBox txtNuevaInsigniaNombre;
        private TextBox txtNuevaInsigniaDescripcion;
        private TextBox txtNuevaInsigniaRequisitos;
        private TextBox txtNuevaInsigniaImagen;
        private Button btnSeleccionarImagen;
        private Button btnCrearInsignia;
        private Button btnActualizarInsigniaCatalogo;
        private Button btnNuevaInsigniaCatalogo;
        private ComboBox cmbInsigniaCatalogoEditar;
        private string rutaImagenSeleccionada;

        public FrmModuloClub(string tipoModulo)
        {
            this.tipoModulo = tipoModulo;
            moduloClubBLL = new ModuloClubBLL();
            InicializarControles();
            ConfigurarModulo();
            CargarDatos();
        }

        private void InicializarControles()
        {
            Text = ObtenerTituloFormulario();
            StartPosition = FormStartPosition.CenterScreen;
            Width = 1120;
            Height = 680;
            KeyPreview = true;
            KeyDown += FrmModuloClub_KeyDown;

            lblCampoUno = CrearLabel(20, 165, 230);
            txtCampoUno = CrearTextBox(20, 193, 230);
            lblCampoDos = CrearLabel(270, 165, 230);
            txtCampoDos = CrearTextBox(270, 193, 230);
            lblCampoTres = CrearLabel(520, 165, 230);
            txtCampoTres = CrearTextBox(520, 193, 230);
            lblCampoCuatro = CrearLabel(770, 165, 230);
            txtCampoCuatro = CrearTextBox(770, 193, 230);

            lblAyuda = CrearLabel(20, 238, 1040);
            lblAyuda.Height = 44;
            lblAyuda.Font = new Font("Segoe UI", 9F, FontStyle.Regular);

            btnGuardar = CrearBoton("Guardar", 20, 290, 150, 32);
            btnGuardar.Click += btnGuardar_Click;

            btnActualizar = CrearBoton("Actualizar", 185, 290, 150, 32);
            btnActualizar.Click += btnActualizar_Click;

            btnVolver = CrearBoton("Volver al menú", 350, 290, 165, 32);
            btnVolver.Click += delegate { Close(); };
            CancelButton = btnVolver;

            dgvDatos = new DataGridView();
            dgvDatos.Left = 20;
            dgvDatos.Top = 355;
            dgvDatos.Width = 1060;
            dgvDatos.Height = 245;
            dgvDatos.ReadOnly = true;
            dgvDatos.AllowUserToAddRows = false;
            dgvDatos.AllowUserToDeleteRows = false;
            dgvDatos.AutoGenerateColumns = true;
            dgvDatos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            dgvDatos.SelectionChanged += dgvDatos_SelectionChanged;
            dgvDatos.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;

            Controls.Add(btnGuardar);
            Controls.Add(btnActualizar);
            Controls.Add(btnVolver);
            Controls.Add(dgvDatos);

            VisualStyleHelper.AplicarEstiloBase(this);
        }

        private string ObtenerTituloFormulario()
        {
            if (tipoModulo == "Pagos") return "Pagos y cuotas";
            if (tipoModulo == "Jugadores") return "Gestión de jugadores";
            if (tipoModulo == "Eventos") return "Gestión de eventos deportivos";
            if (tipoModulo == "Finanzas") return "Ingresos y egresos";
            if (tipoModulo == "Comunicación") return "Comunicación interna";
            if (tipoModulo == "Insignias") return "Gestión de insignias";
            if (tipoModulo == "Inventario") return "Inventario del club";
            if (tipoModulo == "Ventas") return "Ventas";
            if (tipoModulo == "Convocatorias") return "Convocatorias";
            if (tipoModulo == "Resultados") return "Resultados deportivos";
            if (tipoModulo == "Cuotas y fees") return "Configuración de cuotas y fees";
            if (tipoModulo == "Reportes") return "Reportes";
            return tipoModulo;
        }

        private Label CrearLabel(int left, int top, int width)
        {
            Label label = new Label();
            label.Left = left;
            label.Top = top;
            label.Width = width;
            label.Height = 24;
            label.ForeColor = Color.White;
            label.BackColor = Color.Transparent;
            label.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
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

        private ComboBox CrearComboBox(int left, int top, int width, params string[] valores)
        {
            ComboBox combo = new ComboBox();
            combo.Left = left;
            combo.Top = top;
            combo.Width = width;
            combo.DropDownStyle = ComboBoxStyle.DropDownList;
            if (valores != null && valores.Length > 0)
            {
                combo.Items.AddRange(valores);
                combo.SelectedIndex = 0;
            }
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

        private void RestaurarLayoutBase()
        {
            txtCampoDos.Multiline = false;
            txtCampoDos.Height = 22;
            txtCampoDos.Width = 230;
            txtCampoTres.Multiline = false;
            txtCampoTres.Height = 22;
            txtCampoCuatro.Multiline = false;
            txtCampoCuatro.Height = 22;
            lblAyuda.Top = 238;
            btnGuardar.Top = 290;
            btnActualizar.Top = 290;
            btnVolver.Top = 290;
            dgvDatos.Top = 355;
            dgvDatos.Height = Math.Max(200, ClientSize.Height - dgvDatos.Top - 35);
        }

        private string ValorCombo(ComboBox combo, TextBox fallback)
        {
            if (combo != null && combo.Visible && combo.SelectedItem != null) return combo.SelectedItem.ToString();
            return fallback == null ? string.Empty : fallback.Text.Trim();
        }

        private void ConfigurarLayoutExtendido(int topBotones, int topGrilla)
        {
            btnGuardar.Top = topBotones;
            btnActualizar.Top = topBotones;
            btnVolver.Top = topBotones;
            dgvDatos.Top = topGrilla;
            dgvDatos.Height = Math.Max(180, ClientSize.Height - dgvDatos.Top - 35);
        }

        private void ConfigurarModulo()
        {
            MostrarCampo(lblCampoUno, txtCampoUno, true);
            MostrarCampo(lblCampoDos, txtCampoDos, true);
            MostrarCampo(lblCampoTres, txtCampoTres, true);
            MostrarCampo(lblCampoCuatro, txtCampoCuatro, true);
            txtCampoUno.Enabled = true;
            txtCampoDos.Enabled = true;
            txtCampoTres.Enabled = true;
            txtCampoCuatro.Enabled = true;
            btnGuardar.Enabled = true;
            btnGuardar.Text = "Guardar";
            btnActualizar.Text = "Actualizar";
            lblAyuda.Text = string.Empty;
            if (cmbTipoMovimiento != null) cmbTipoMovimiento.Visible = false;
            if (cmbTipoPublicacion != null) cmbTipoPublicacion.Visible = false;
            if (cmbTipoVenta != null) cmbTipoVenta.Visible = false;
            if (cmbNivelInsignia != null) cmbNivelInsignia.Visible = false;
            if (cmbEventoDeporte != null) cmbEventoDeporte.Visible = false;
            if (cmbEventoLugar != null) cmbEventoLugar.Visible = false;
            if (cmbInventarioEstado != null) cmbInventarioEstado.Visible = false;
            if (cmbRespuestaConvocatoria != null) cmbRespuestaConvocatoria.Visible = false;
            if (cmbJugadorDeporte != null) cmbJugadorDeporte.Visible = false;
            if (cmbJugadorDisponible != null) cmbJugadorDisponible.Visible = false;
            if (lblEventoCupo != null) lblEventoCupo.Visible = false;
            if (lblEventoEntrada != null) lblEventoEntrada.Visible = false;
            if (lblEventoFee != null) lblEventoFee.Visible = false;
            if (txtEventoCupo != null) txtEventoCupo.Visible = false;
            if (txtEventoEntrada != null) txtEventoEntrada.Visible = false;
            if (txtEventoFee != null) txtEventoFee.Visible = false;
            if (btnBuscarInventario != null) btnBuscarInventario.Visible = false;
            if (btnSumarInventario != null) btnSumarInventario.Visible = false;
            if (btnRestarInventario != null) btnRestarInventario.Visible = false;
            if (lblBalance != null) lblBalance.Visible = false;
            if (btnNuevoEvento != null) btnNuevoEvento.Visible = false;
            if (dgvNivelesInsignia != null) dgvNivelesInsignia.Visible = false;
            if (btnAgregarNivelInsignia != null) btnAgregarNivelInsignia.Visible = false;
            if (btnCargarInsigniaCatalogo != null) btnCargarInsigniaCatalogo.Visible = false;
            if (btnCrearInsignia != null) btnCrearInsignia.Visible = false;
            if (btnActualizarInsigniaCatalogo != null) btnActualizarInsigniaCatalogo.Visible = false;
            if (btnNuevaInsigniaCatalogo != null) btnNuevaInsigniaCatalogo.Visible = false;
            if (cmbInsigniaCatalogoEditar != null) cmbInsigniaCatalogoEditar.Visible = false;
            RestaurarLayoutBase();

            if (tipoModulo == "Pagos")
            {
                lblCampoUno.Text = "N° de socio";
                lblCampoDos.Text = "Concepto de pago";
                lblCampoTres.Text = "Importe";
                lblCampoCuatro.Text = "Estado";
                lblAyuda.Text = "Complete el socio, concepto, importe y estado del pago. Ejemplo de estado: Pagado o Pendiente.";
            }
            else if (tipoModulo == "Jugadores")
            {
                lblCampoUno.Text = "N° de socio / búsqueda";
                lblCampoDos.Text = "Deporte";
                lblCampoTres.Text = "Posición / categoría";
                lblCampoCuatro.Text = "Disponible";
                btnGuardar.Text = "Guardar jugador";
                btnActualizar.Text = "Buscar";

                txtCampoDos.Visible = false;
                txtCampoDos.Enabled = false;
                if (cmbJugadorDeporte == null)
                {
                    cmbJugadorDeporte = CrearComboBox(txtCampoDos.Left, txtCampoDos.Top, txtCampoDos.Width, "Fútbol", "Básquet", "Tenis", "Volley", "Natación", "Hockey", "Running");
                }
                cmbJugadorDeporte.Visible = true;
                cmbJugadorDeporte.BringToFront();

                txtCampoCuatro.Visible = false;
                txtCampoCuatro.Enabled = false;
                if (cmbJugadorDisponible == null)
                {
                    cmbJugadorDisponible = CrearComboBox(txtCampoCuatro.Left, txtCampoCuatro.Top, txtCampoCuatro.Width, "S", "N");
                }
                cmbJugadorDisponible.Visible = true;
                cmbJugadorDisponible.BringToFront();

                lblAyuda.Text = "Use el buscador para filtrar por socio, deporte o posición. Seleccione una fila para editar y luego guarde los cambios.";
            }
            else if (tipoModulo == "Eventos")
            {
                lblCampoUno.Text = "Título del evento";
                lblCampoDos.Text = "Deporte";
                lblCampoTres.Text = "Lugar";
                lblCampoCuatro.Text = "Fecha y hora";
                if (string.IsNullOrWhiteSpace(txtCampoCuatro.Text)) txtCampoCuatro.Text = DateTime.Now.AddDays(7).ToString("yyyy-MM-dd HH:mm");
                btnGuardar.Text = "Guardar evento";
                btnActualizar.Text = "Buscar";

                txtCampoDos.Visible = false;
                txtCampoDos.Enabled = false;
                if (cmbEventoDeporte == null)
                {
                    cmbEventoDeporte = CrearComboBox(txtCampoDos.Left, txtCampoDos.Top, txtCampoDos.Width, "Fútbol", "Básquet", "Tenis", "Volley", "Natación", "Hockey", "Running");
                }
                cmbEventoDeporte.Visible = true;
                cmbEventoDeporte.BringToFront();

                txtCampoTres.Visible = false;
                txtCampoTres.Enabled = false;
                if (cmbEventoLugar == null)
                {
                    cmbEventoLugar = CrearComboBox(txtCampoTres.Left, txtCampoTres.Top, txtCampoTres.Width, "Cancha principal", "Cancha de fútbol 1", "Sector tenis", "Sector basket", "Sector volley", "Pileta", "Salón 1", "Gimnasio");
                }
                cmbEventoLugar.Visible = true;
                cmbEventoLugar.BringToFront();

                if (lblEventoCupo == null) lblEventoCupo = CrearLabel(20, 235, 190);
                if (txtEventoCupo == null) txtEventoCupo = CrearTextBox(20, 263, 190);
                if (lblEventoEntrada == null) lblEventoEntrada = CrearLabel(230, 235, 190);
                if (txtEventoEntrada == null) txtEventoEntrada = CrearTextBox(230, 263, 190);
                if (lblEventoFee == null) lblEventoFee = CrearLabel(440, 235, 190);
                if (txtEventoFee == null) txtEventoFee = CrearTextBox(440, 263, 190);

                lblEventoCupo.Text = "Cupo espectadores";
                lblEventoEntrada.Text = "Entrada espectador";
                lblEventoFee.Text = "Fee jugador";
                lblEventoCupo.Visible = txtEventoCupo.Visible = true;
                lblEventoEntrada.Visible = txtEventoEntrada.Visible = true;
                lblEventoFee.Visible = txtEventoFee.Visible = true;
                if (string.IsNullOrWhiteSpace(txtEventoCupo.Text)) txtEventoCupo.Text = "100";
                if (string.IsNullOrWhiteSpace(txtEventoEntrada.Text)) txtEventoEntrada.Text = "3000";
                if (string.IsNullOrWhiteSpace(txtEventoFee.Text)) txtEventoFee.Text = "2000";

                if (btnNuevoEvento == null)
                {
                    btnNuevoEvento = CrearBoton("Nuevo evento", 515, 348, 150, 32);
                    btnNuevoEvento.Click += btnNuevoEvento_Click;
                }
                btnNuevoEvento.Visible = true;
                btnNuevoEvento.BringToFront();

                lblAyuda.Top = 300;
                lblAyuda.Text = "Buscar filtra la tabla por título, deporte o lugar. Seleccione un evento para editarlo; Nuevo evento limpia la selección para crear uno nuevo.";
                ConfigurarLayoutExtendido(348, 405);
            }
            else if (tipoModulo == "Finanzas")
            {
                lblCampoUno.Text = "Tipo de movimiento";
                lblCampoDos.Text = "Concepto";
                lblCampoTres.Text = "Importe";
                MostrarCampo(lblCampoCuatro, txtCampoCuatro, false);
                txtCampoUno.Visible = false;
                txtCampoUno.Enabled = false;
                if (cmbTipoMovimiento == null)
                {
                    cmbTipoMovimiento = CrearComboBox(txtCampoUno.Left, txtCampoUno.Top, txtCampoUno.Width, "INGRESO", "EGRESO");
                }
                cmbTipoMovimiento.Visible = true;
                cmbTipoMovimiento.BringToFront();
                if (lblBalance == null)
                {
                    lblBalance = CrearLabel(540, 255, 500);
                    lblBalance.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                }
                lblBalance.Visible = true;
                lblAyuda.Text = "Seleccione INGRESO o EGRESO. El importe se ingresa en positivo; el balance se calcula automáticamente.";
            }
            else if (tipoModulo == "Comunicación")
            {
                lblCampoUno.Text = "Título de la comunicación";
                lblCampoDos.Text = "Mensaje / contenido";
                lblCampoTres.Text = "Categoría";
                MostrarCampo(lblCampoCuatro, txtCampoCuatro, false);
                txtCampoDos.Width = 360;
                txtCampoDos.Multiline = true;
                txtCampoDos.Height = 50;
                txtCampoTres.Visible = false;
                txtCampoTres.Enabled = false;
                if (cmbTipoPublicacion == null)
                {
                    cmbTipoPublicacion = CrearComboBox(txtCampoTres.Left + 130, txtCampoTres.Top, txtCampoTres.Width, "AVISO", "EVENTO", "URGENTE", "NOVEDAD", "GENERAL");
                }
                cmbTipoPublicacion.Visible = true;
                cmbTipoPublicacion.BringToFront();
                lblAyuda.Top = 255;
                lblAyuda.Text = "Publica comunicaciones internas visibles para los socios. Categoría indica si es aviso general, evento, urgencia o novedad.";
                ConfigurarLayoutExtendido(305, 360);
            }
            else if (tipoModulo == "Insignias")
            {
                ConfigurarModuloInsignias();
            }
            else if (tipoModulo == "Inventario")
            {
                lblCampoUno.Text = "Artículo / búsqueda";
                lblCampoDos.Text = "Cantidad";
                lblCampoTres.Text = "Ubicación";
                lblCampoCuatro.Text = "Estado";
                txtCampoCuatro.Visible = false;
                txtCampoCuatro.Enabled = false;
                if (cmbInventarioEstado == null)
                {
                    cmbInventarioEstado = CrearComboBox(txtCampoCuatro.Left, txtCampoCuatro.Top, txtCampoCuatro.Width, "Disponible", "En uso", "Mantenimiento", "Baja");
                }
                cmbInventarioEstado.Visible = true;
                cmbInventarioEstado.BringToFront();
                if (btnBuscarInventario == null)
                {
                    btnBuscarInventario = CrearBoton("Buscar", 20, 330, 120, 30);
                    btnBuscarInventario.Click += delegate { CargarDatos(); };
                    btnSumarInventario = CrearBoton("Sumar stock", 155, 330, 140, 30);
                    btnSumarInventario.Click += delegate { AjustarStockInventario(true); };
                    btnRestarInventario = CrearBoton("Restar stock", 310, 330, 140, 30);
                    btnRestarInventario.Click += delegate { AjustarStockInventario(false); };
                }
                btnBuscarInventario.Visible = true;
                btnSumarInventario.Visible = true;
                btnRestarInventario.Visible = true;
                lblAyuda.Text = "Use Artículo para buscar. Guardar crea o actualiza el artículo. Para sumar/restar stock, seleccione una fila e indique la cantidad.";
                ConfigurarLayoutExtendido(290, 385);
            }
            else if (tipoModulo == "Ventas")
            {
                lblCampoUno.Text = "Tipo de venta";
                lblCampoDos.Text = "Detalle de la venta";
                lblCampoTres.Text = "Importe";
                MostrarCampo(lblCampoCuatro, txtCampoCuatro, false);
                txtCampoUno.Visible = false;
                txtCampoUno.Enabled = false;
                if (cmbTipoVenta == null)
                {
                    cmbTipoVenta = CrearComboBox(txtCampoUno.Left, txtCampoUno.Top, txtCampoUno.Width, "ENTRADA", "BUFFET", "MERCHANDISING", "ALQUILER", "OTRO");
                }
                cmbTipoVenta.Visible = true;
                cmbTipoVenta.BringToFront();
                btnGuardar.Text = "Registrar venta";
                btnActualizar.Text = "Buscar ventas";
                lblAyuda.Text = "Registra ingresos no vinculados a cuotas: entradas, buffet, merchandising, alquileres u otros conceptos. Buscar ventas filtra por tipo o descripción.";
            }
            else if (tipoModulo == "Convocatorias")
            {
                lblCampoUno.Text = "N° de evento";
                lblCampoDos.Text = "N° de jugador";
                lblCampoTres.Text = "Respuesta";
                MostrarCampo(lblCampoCuatro, txtCampoCuatro, false);
                txtCampoTres.Visible = false;
                txtCampoTres.Enabled = false;
                if (cmbRespuestaConvocatoria == null)
                {
                    cmbRespuestaConvocatoria = CrearComboBox(txtCampoTres.Left, txtCampoTres.Top, txtCampoTres.Width, "PENDIENTE", "CONFIRMADA", "ASISTIO", "RECHAZADA");
                }
                cmbRespuestaConvocatoria.Visible = true;
                cmbRespuestaConvocatoria.BringToFront();
                btnActualizar.Text = "Buscar";
                btnGuardar.Text = "Guardar cambios";
                lblAyuda.Text = "Use N° de evento y Buscar para filtrar. Seleccione una fila para editar la respuesta. CONFIRMADA o ASISTIO agrega automáticamente el fee del evento a la cuota del socio.";
            }
            else if (tipoModulo == "Resultados")
            {
                lblCampoUno.Text = "N° de evento";
                lblCampoDos.Text = "Equipo local";
                lblCampoTres.Text = "Equipo visitante";
                lblCampoCuatro.Text = "Resultado";
                lblAyuda.Text = "Cargue el resultado deportivo asociado a un evento.";
            }
            else if (tipoModulo == "Cuotas y fees")
            {
                lblCampoUno.Text = "N° configuración";
                lblCampoDos.Text = "Concepto";
                lblCampoTres.Text = "Importe";
                lblCampoCuatro.Text = "Activo S/N";
                txtCampoUno.Enabled = false;
                txtCampoDos.Enabled = false;
                btnGuardar.Enabled = true;
                btnGuardar.Text = "Guardar cambios";
                lblAyuda.Text = "Seleccione una fila de la tabla, edite el importe o el estado activo y presione Guardar cambios. Incluye cuotas de socios y fees default de eventos.";
            }
            else if (tipoModulo == "Reportes")
            {
                lblCampoUno.Text = "Reporte";
                txtCampoUno.Text = "Indicadores generales del club";
                MostrarCampo(lblCampoDos, txtCampoDos, false);
                MostrarCampo(lblCampoTres, txtCampoTres, false);
                MostrarCampo(lblCampoCuatro, txtCampoCuatro, false);
                txtCampoUno.Enabled = false;
                btnGuardar.Enabled = false;
                lblAyuda.Text = "Muestra indicadores consolidados: socios, pagos, deuda, ingresos, egresos, eventos, convocatorias, inventario e insignias.";
            }
        }

        private void MostrarCampo(Label label, TextBox textBox, bool visible)
        {
            label.Visible = visible;
            textBox.Visible = visible;
            textBox.Enabled = visible;
        }

        private void ConfigurarModuloInsignias()
        {
            Width = 1180;
            Height = 820;

            lblCampoUno.Text = "N° de socio o usuario";
            lblCampoDos.Text = "Insignia a asignar";
            lblCampoTres.Text = "Nivel";
            lblCampoCuatro.Text = "Motivo / observación";
            btnGuardar.Text = "Asignar insignia";
            btnActualizar.Text = "Buscar socio";
            lblAyuda.Text = "Busque un socio para ver sus insignias. Para asignar, indique socio o usuario, insignia, nivel y motivo.";

            txtCampoDos.Visible = false;
            txtCampoDos.Enabled = false;
            txtCampoTres.Visible = false;
            txtCampoTres.Enabled = false;

            if (cmbInsigniasDisponibles == null)
            {
                cmbInsigniasDisponibles = new ComboBox();
                cmbInsigniasDisponibles.Left = txtCampoDos.Left;
                cmbInsigniasDisponibles.Top = txtCampoDos.Top;
                cmbInsigniasDisponibles.Width = txtCampoDos.Width;
                cmbInsigniasDisponibles.DropDownStyle = ComboBoxStyle.DropDownList;
                cmbInsigniasDisponibles.SelectedIndexChanged += cmbInsigniasDisponibles_SelectedIndexChanged;
                Controls.Add(cmbInsigniasDisponibles);
            }
            cmbInsigniasDisponibles.Visible = true;

            if (cmbNivelInsignia == null)
            {
                cmbNivelInsignia = CrearComboBox(txtCampoTres.Left, txtCampoTres.Top, txtCampoTres.Width);
            }
            cmbNivelInsignia.Visible = true;

            lblNuevaInsignia = CrearLabel(20, 320, 900);
            lblNuevaInsignia.Text = "Crear o editar insignia del catálogo";
            lblNuevaInsignia.Font = new Font("Segoe UI", 10F, FontStyle.Bold);

            Label lblCatalogoEditar = CrearLabel(20, 350, 210);
            lblCatalogoEditar.Text = "Insignia existente para editar";
            if (cmbInsigniaCatalogoEditar == null)
            {
                cmbInsigniaCatalogoEditar = new ComboBox();
                cmbInsigniaCatalogoEditar.Left = 20;
                cmbInsigniaCatalogoEditar.Top = 377;
                cmbInsigniaCatalogoEditar.Width = 210;
                cmbInsigniaCatalogoEditar.DropDownStyle = ComboBoxStyle.DropDownList;
                Controls.Add(cmbInsigniaCatalogoEditar);
            }
            cmbInsigniaCatalogoEditar.Visible = true;

            btnCargarInsigniaCatalogo = CrearBoton("Cargar para editar", 245, 375, 150, 28);
            btnCargarInsigniaCatalogo.Click += btnCargarInsigniaCatalogo_Click;

            btnNuevaInsigniaCatalogo = CrearBoton("Nueva insignia", 410, 375, 140, 28);
            btnNuevaInsigniaCatalogo.Click += btnNuevaInsigniaCatalogo_Click;

            lblNuevaInsigniaNombre = CrearLabel(20, 420, 190);
            lblNuevaInsigniaNombre.Text = "Nombre de la insignia";
            txtNuevaInsigniaNombre = CrearTextBox(20, 447, 190);

            lblNuevaInsigniaDescripcion = CrearLabel(230, 420, 300);
            lblNuevaInsigniaDescripcion.Text = "Descripción general";
            txtNuevaInsigniaDescripcion = CrearTextBox(230, 447, 300);

            lblNuevaInsigniaImagen = CrearLabel(550, 420, 250);
            lblNuevaInsigniaImagen.Text = "Imagen";
            txtNuevaInsigniaImagen = CrearTextBox(550, 447, 220);
            txtNuevaInsigniaImagen.ReadOnly = true;

            btnSeleccionarImagen = CrearBoton("Buscar imagen", 785, 445, 120, 28);
            btnSeleccionarImagen.Click += btnSeleccionarImagen_Click;

            lblNuevaInsigniaRequisitos = CrearLabel(20, 490, 1030);
            lblNuevaInsigniaRequisitos.Text = "Niveles: use + Agregar nivel para sumar filas. Complete título, descripción y requisito de cada nivel.";

            dgvNivelesInsignia = new DataGridView();
            dgvNivelesInsignia.Left = 20;
            dgvNivelesInsignia.Top = 525;
            dgvNivelesInsignia.Width = 1050;
            dgvNivelesInsignia.Height = 110;
            dgvNivelesInsignia.AllowUserToAddRows = false;
            dgvNivelesInsignia.AllowUserToDeleteRows = true;
            dgvNivelesInsignia.AutoGenerateColumns = false;
            dgvNivelesInsignia.Columns.Add("Nivel", "Nivel");
            dgvNivelesInsignia.Columns.Add("Titulo", "Título del nivel");
            dgvNivelesInsignia.Columns.Add("Descripcion", "Descripción del nivel");
            dgvNivelesInsignia.Columns.Add("Requisito", "Requisito");
            dgvNivelesInsignia.Columns[0].Width = 70;
            dgvNivelesInsignia.Columns[1].Width = 180;
            dgvNivelesInsignia.Columns[2].Width = 420;
            dgvNivelesInsignia.Columns[3].Width = 340;
            Controls.Add(dgvNivelesInsignia);

            btnAgregarNivelInsignia = CrearBoton("+ Agregar nivel", 20, 650, 160, 32);
            btnAgregarNivelInsignia.Click += btnAgregarNivelInsignia_Click;

            btnCrearInsignia = CrearBoton("Crear insignia", 200, 650, 160, 32);
            btnCrearInsignia.Click += btnCrearInsignia_Click;

            btnActualizarInsigniaCatalogo = CrearBoton("Actualizar insignia", 380, 650, 180, 32);
            btnActualizarInsigniaCatalogo.Click += btnActualizarInsigniaCatalogo_Click;

            dgvDatos.Top = 695;
            dgvDatos.Height = 70;
            CargarCatalogoInsignias();
            LimpiarEditorInsigniaCatalogo();
        }

        private void CargarCatalogoInsignias()
        {
            if (cmbInsigniasDisponibles == null)
            {
                return;
            }

            try
            {
                DataTable catalogo = moduloClubBLL.ConsultarCatalogoInsignias();
                cmbInsigniasDisponibles.DataSource = catalogo.Copy();
                cmbInsigniasDisponibles.DisplayMember = "Nombre";
                cmbInsigniasDisponibles.ValueMember = "Nombre";
                if (cmbInsigniaCatalogoEditar != null)
                {
                    cmbInsigniaCatalogoEditar.DataSource = catalogo.Copy();
                    cmbInsigniaCatalogoEditar.DisplayMember = "Nombre";
                    cmbInsigniaCatalogoEditar.ValueMember = "Nombre";
                }
                ActualizarComboNivelesAsignacion();
            }
            catch
            {
                cmbInsigniasDisponibles.DataSource = null;
                if (cmbInsigniaCatalogoEditar != null) cmbInsigniaCatalogoEditar.DataSource = null;
            }
        }

        private void ActualizarComboNivelesAsignacion()
        {
            if (cmbNivelInsignia == null) return;
            cmbNivelInsignia.Items.Clear();
            string requisitos = string.Empty;
            DataRowView fila = cmbInsigniasDisponibles != null ? cmbInsigniasDisponibles.SelectedItem as DataRowView : null;
            if (fila != null && fila.Row.Table.Columns.Contains("RequisitoNiveles")) requisitos = Convert.ToString(fila["RequisitoNiveles"]);
            if (!string.IsNullOrWhiteSpace(requisitos))
            {
                foreach (string linea in requisitos.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    string nivel = linea.Split('|')[0].Trim();
                    if (!string.IsNullOrWhiteSpace(nivel) && !cmbNivelInsignia.Items.Contains(nivel)) cmbNivelInsignia.Items.Add(nivel);
                }
            }
            if (cmbNivelInsignia.Items.Count == 0) cmbNivelInsignia.Items.Add("1");
            cmbNivelInsignia.SelectedIndex = 0;
        }

        private void LimpiarEditorInsigniaCatalogo()
        {
            if (txtNuevaInsigniaNombre != null) txtNuevaInsigniaNombre.Clear();
            if (txtNuevaInsigniaDescripcion != null) txtNuevaInsigniaDescripcion.Clear();
            if (txtNuevaInsigniaImagen != null) txtNuevaInsigniaImagen.Clear();
            if (dgvNivelesInsignia != null) dgvNivelesInsignia.Rows.Clear();
            rutaImagenSeleccionada = null;
        }

        private void btnNuevaInsigniaCatalogo_Click(object sender, EventArgs e)
        {
            LimpiarEditorInsigniaCatalogo();
            if (txtNuevaInsigniaNombre != null) txtNuevaInsigniaNombre.Focus();
        }

        private void cmbInsigniasDisponibles_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActualizarComboNivelesAsignacion();
        }

        private void btnCargarInsigniaCatalogo_Click(object sender, EventArgs e)
        {
            CargarInsigniaSeleccionadaEnFormulario();
        }

        private void CargarInsigniaSeleccionadaEnFormulario()
        {
            DataRowView fila = cmbInsigniaCatalogoEditar != null ? cmbInsigniaCatalogoEditar.SelectedItem as DataRowView : null;
            if (fila == null)
            {
                return;
            }

            txtNuevaInsigniaNombre.Text = Convert.ToString(fila["Nombre"]);
            txtNuevaInsigniaDescripcion.Text = Convert.ToString(fila["Descripcion"]);
            txtNuevaInsigniaImagen.Text = Convert.ToString(fila["Imagen"]);
            rutaImagenSeleccionada = null;
            CargarNivelesDesdeTexto(Convert.ToString(fila["RequisitoNiveles"]));
        }

        private void CargarNivelesDesdeTexto(string texto)
        {
            if (dgvNivelesInsignia == null) return;
            dgvNivelesInsignia.Rows.Clear();
            if (string.IsNullOrWhiteSpace(texto))
            {
                AgregarFilaNivelInsignia();
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
                dgvNivelesInsignia.Rows.Add(nivel, titulo, descripcion, requisito);
            }
        }

        private void btnAgregarNivelInsignia_Click(object sender, EventArgs e)
        {
            AgregarFilaNivelInsignia();
        }

        private void AgregarFilaNivelInsignia()
        {
            if (dgvNivelesInsignia == null) return;
            int siguienteNivel = 1;
            foreach (DataGridViewRow row in dgvNivelesInsignia.Rows)
            {
                if (row.IsNewRow) continue;
                int nivelExistente;
                if (int.TryParse(Convert.ToString(row.Cells["Nivel"].Value), out nivelExistente) && nivelExistente >= siguienteNivel)
                {
                    siguienteNivel = nivelExistente + 1;
                }
            }

            int indice = dgvNivelesInsignia.Rows.Add(siguienteNivel.ToString(), string.Empty, string.Empty, string.Empty);
            dgvNivelesInsignia.CurrentCell = dgvNivelesInsignia.Rows[indice].Cells["Titulo"];
            dgvNivelesInsignia.BeginEdit(true);
        }

        private string SerializarNivelesInsignia()
        {
            if (dgvNivelesInsignia == null) return string.Empty;
            List<string> niveles = new List<string>();
            foreach (DataGridViewRow row in dgvNivelesInsignia.Rows)
            {
                if (row.IsNewRow) continue;
                string nivel = Convert.ToString(row.Cells["Nivel"].Value).Trim();
                string titulo = Convert.ToString(row.Cells["Titulo"].Value).Trim();
                string descripcion = Convert.ToString(row.Cells["Descripcion"].Value).Trim();
                string requisito = Convert.ToString(row.Cells["Requisito"].Value).Trim();
                if (string.IsNullOrWhiteSpace(nivel)) continue;
                niveles.Add(nivel + "|" + titulo + "|" + descripcion + "|" + requisito);
            }
            return string.Join(";", niveles.ToArray());
        }

        private void CargarDatos()
        {
            try
            {
                DataTable tabla;

                if (tipoModulo == "Pagos") tabla = moduloClubBLL.ConsultarPagos();
                else if (tipoModulo == "Jugadores")
                {
                    tabla = moduloClubBLL.ConsultarJugadores();
                    string filtroJugador = txtCampoUno.Text.Trim().Replace("'", "''");
                    if (!string.IsNullOrWhiteSpace(filtroJugador))
                    {
                        DataView vista = new DataView(tabla);
                        string filtro = "Convert(IdSocio, 'System.String') LIKE '%" + filtroJugador + "%'";
                        if (tabla.Columns.Contains("Socio")) filtro += " OR Convert(Socio, 'System.String') LIKE '%" + filtroJugador + "%'";
                        if (tabla.Columns.Contains("Deporte")) filtro += " OR Convert(Deporte, 'System.String') LIKE '%" + filtroJugador + "%'";
                        if (tabla.Columns.Contains("Posicion")) filtro += " OR Convert(Posicion, 'System.String') LIKE '%" + filtroJugador + "%'";
                        vista.RowFilter = filtro;
                        tabla = vista.ToTable();
                    }
                }
                else if (tipoModulo == "Eventos")
                {
                    tabla = moduloClubBLL.ConsultarEventos();
                    string filtroEvento = txtCampoUno.Text.Trim().Replace("'", "''");
                    if (!string.IsNullOrWhiteSpace(filtroEvento))
                    {
                        DataView vista = new DataView(tabla);
                        string filtro = "Convert(Nombre, 'System.String') LIKE '%" + filtroEvento + "%'";
                        if (tabla.Columns.Contains("Deporte")) filtro += " OR Convert(Deporte, 'System.String') LIKE '%" + filtroEvento + "%'";
                        if (tabla.Columns.Contains("Lugar")) filtro += " OR Convert(Lugar, 'System.String') LIKE '%" + filtroEvento + "%'";
                        vista.RowFilter = filtro;
                        tabla = vista.ToTable();
                    }
                }
                else if (tipoModulo == "Finanzas") tabla = moduloClubBLL.ConsultarMovimientosFinancieros();
                else if (tipoModulo == "Comunicación") tabla = moduloClubBLL.ConsultarPublicaciones();
                else if (tipoModulo == "Insignias")
                {
                    tabla = moduloClubBLL.ConsultarInsignias();
                    string filtroSocio = txtCampoUno.Text.Trim().Replace("'", "''");
                    if (!string.IsNullOrWhiteSpace(filtroSocio))
                    {
                        DataView vista = new DataView(tabla);
                        List<string> filtrosInsignia = new List<string>();
                        int idSocioFiltro;
                        if (tabla.Columns.Contains("IdSocio"))
                        {
                            if (int.TryParse(filtroSocio, out idSocioFiltro))
                            {
                                filtrosInsignia.Add("IdSocio = " + idSocioFiltro.ToString());
                            }
                            else
                            {
                                try
                                {
                                    int idResuelto = moduloClubBLL.ResolverIdSocio(filtroSocio);
                                    filtrosInsignia.Add("IdSocio = " + idResuelto.ToString());
                                }
                                catch { }
                            }
                        }
                        if (tabla.Columns.Contains("Socio")) filtrosInsignia.Add("Convert(Socio, 'System.String') LIKE '%" + filtroSocio + "%'");
                        if (tabla.Columns.Contains("Nombre")) filtrosInsignia.Add("Convert(Nombre, 'System.String') LIKE '%" + filtroSocio + "%'");
                        if (filtrosInsignia.Count > 0)
                        {
                            vista.RowFilter = string.Join(" OR ", filtrosInsignia.ToArray());
                            tabla = vista.ToTable();
                        }
                        try { idSocioInsigniaSeleccionado = moduloClubBLL.ResolverIdSocio(filtroSocio); } catch { }
                        txtCampoUno.Text = string.Empty;
                    }
                }
                else if (tipoModulo == "Inventario")
                {
                    tabla = moduloClubBLL.ConsultarInventario();
                    string filtroInventario = txtCampoUno.Text.Trim().Replace("'", "''");
                    if (!string.IsNullOrWhiteSpace(filtroInventario))
                    {
                        DataView vista = new DataView(tabla);
                        string filtro = "Convert(Nombre, 'System.String') LIKE '%" + filtroInventario + "%'";
                        if (tabla.Columns.Contains("Ubicacion")) filtro += " OR Convert(Ubicacion, 'System.String') LIKE '%" + filtroInventario + "%'";
                        if (tabla.Columns.Contains("Estado")) filtro += " OR Convert(Estado, 'System.String') LIKE '%" + filtroInventario + "%'";
                        vista.RowFilter = filtro;
                        tabla = vista.ToTable();
                    }
                }
                else if (tipoModulo == "Ventas")
                {
                    tabla = moduloClubBLL.ConsultarVentas();
                    string filtroVenta = txtCampoDos.Text.Trim().Replace("'", "''");
                    if (!string.IsNullOrWhiteSpace(filtroVenta))
                    {
                        DataView vista = new DataView(tabla);
                        string filtro = "Convert(Descripcion, 'System.String') LIKE '%" + filtroVenta + "%'";
                        if (tabla.Columns.Contains("TipoVenta")) filtro += " OR Convert(TipoVenta, 'System.String') LIKE '%" + filtroVenta + "%'";
                        vista.RowFilter = filtro;
                        tabla = vista.ToTable();
                    }
                }
                else if (tipoModulo == "Convocatorias")
                {
                    tabla = moduloClubBLL.ConsultarConvocatorias();
                    List<string> filtros = new List<string>();
                    int idEventoFiltro;
                    int idJugadorFiltro;
                    if (int.TryParse(txtCampoUno.Text.Trim(), out idEventoFiltro) && tabla.Columns.Contains("IdEvento"))
                    {
                        filtros.Add("IdEvento = " + idEventoFiltro.ToString());
                    }
                    if (int.TryParse(txtCampoDos.Text.Trim(), out idJugadorFiltro) && tabla.Columns.Contains("IdJugador"))
                    {
                        filtros.Add("IdJugador = " + idJugadorFiltro.ToString());
                    }
                    if (filtros.Count > 0)
                    {
                        DataView vista = new DataView(tabla);
                        vista.RowFilter = string.Join(" AND ", filtros.ToArray());
                        tabla = vista.ToTable();
                    }
                }
                else if (tipoModulo == "Resultados") tabla = moduloClubBLL.ConsultarResultadosPartidos();
                else if (tipoModulo == "Cuotas y fees") tabla = moduloClubBLL.ConsultarConfiguracionCuotas();
                else tabla = moduloClubBLL.ConsultarReportes();

                dgvDatos.DataSource = tabla;
                HumanizarGrilla();
                ActualizarResumenFinanciero(tabla);
                if (tipoModulo == "Cuotas y fees")
                {
                    CargarConfiguracionSeleccionada();
                }
                else if (tipoModulo == "Eventos" && dgvDatos.Rows.Count > 0)
                {
                    dgvDatos.ClearSelection();
                    dgvDatos.Rows[0].Selected = true;
                    dgvDatos.CurrentCell = dgvDatos.Rows[0].Cells[0];
                    CargarEventoSeleccionado();
                }
                else if (tipoModulo == "Jugadores" && dgvDatos.Rows.Count > 0)
                {
                    dgvDatos.ClearSelection();
                    dgvDatos.Rows[0].Selected = true;
                    dgvDatos.CurrentCell = dgvDatos.Rows[0].Cells[0];
                    CargarJugadorSeleccionado();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HumanizarGrilla()
        {
            Dictionary<string, string> headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "IdSocio", "N° socio" },
                { "IdPago", "N° pago" },
                { "IdJugador", "N° jugador" },
                { "IdEvento", "N° evento" },
                { "IdMovimiento", "N° movimiento" },
                { "IdPublicacion", "N° publicación" },
                { "IdInsignia", "N° insignia" },
                { "IdInventario", "N° inventario" },
                { "IdVenta", "N° venta" },
                { "IdConvocatoria", "N° convocatoria" },
                { "IdResultado", "N° resultado" },
                { "FechaPago", "Fecha de pago" },
                { "FechaEvento", "Fecha del evento" },
                { "FechaMovimiento", "Fecha" },
                { "FechaPublicacion", "Fecha de publicación" },
                { "FechaOtorgamiento", "Fecha de asignación" },
                { "FechaVenta", "Fecha de venta" },
                { "FechaCarga", "Fecha de carga" },
                { "Nombre", "Nombre" },
                { "Titulo", "Título" },
                { "TituloNivel", "Título" },
                { "TipoPublicacion", "Tipo de comunicación" },
                { "UsuarioAutor", "Autor" },
                { "Contenido", "Contenido" },
                { "TipoMovimiento", "Tipo" },
                { "TipoVenta", "Tipo de venta" },
                { "Descripcion", "Descripción" },
                { "Motivo", "Motivo" },
                { "Deporte", "Deporte" },
                { "Posicion", "Posición" },
                { "Disponible", "Disponible" },
                { "Lugar", "Lugar" },
                { "Estado", "Estado" },
                { "CupoEspectadores", "Cupo espectadores" },
                { "PrecioEntradaEspectador", "Entrada espectador" },
                { "PrecioParticipacionJugador", "Fee jugador" },
                { "Importe", "Importe" },
                { "Socio", "Socio" },
                { "Ubicacion", "Ubicación" },
                { "Cantidad", "Cantidad" },
                { "EstadoRespuesta", "Respuesta" },
                { "EquipoLocal", "Equipo local" },
                { "EquipoVisitante", "Equipo visitante" },
                { "Resultado", "Resultado" },
                { "Rol", "Rol" },
                { "Concepto", "Concepto" },
                { "Activo", "Activo" },
                { "Nivel", "Nivel" },
                { "Regla", "Regla" },
                { "Imagen", "Archivo de imagen" },
                { "RequisitoNiveles", "Requisitos por nivel" },
                { "Indicador", "Indicador" },
                { "Valor", "Valor" }
            };

            foreach (DataGridViewColumn columna in dgvDatos.Columns)
            {
                string header;
                if (headers.TryGetValue(columna.Name, out header))
                {
                    columna.HeaderText = header;
                }
            }

            if (tipoModulo == "Insignias" && dgvDatos.Columns.Contains("Imagen"))
            {
                dgvDatos.Columns["Imagen"].Visible = false;
            }
        }

        private void ActualizarResumenFinanciero(DataTable tabla)
        {
            if (tipoModulo != "Finanzas" || lblBalance == null || tabla == null)
            {
                return;
            }

            decimal ingresos = 0;
            decimal egresos = 0;
            foreach (DataRow row in tabla.Rows)
            {
                decimal importe;
                if (!decimal.TryParse(Convert.ToString(row["Importe"]), out importe)) continue;
                string tipo = Convert.ToString(row["TipoMovimiento"]).ToUpperInvariant();
                if (tipo == "INGRESO") ingresos += Math.Abs(importe);
                else if (tipo == "EGRESO") egresos += Math.Abs(importe);
            }
            decimal balance = ingresos - egresos;
            lblBalance.Text = "Balance actual: $" + balance.ToString("N2") + "   Ingresos: $" + ingresos.ToString("N2") + "   Egresos: $" + egresos.ToString("N2");
        }

        private void dgvDatos_SelectionChanged(object sender, EventArgs e)
        {
            if (tipoModulo == "Cuotas y fees")
            {
                CargarConfiguracionSeleccionada();
            }
            else if (tipoModulo == "Eventos")
            {
                CargarEventoSeleccionado();
            }
            else if (tipoModulo == "Jugadores")
            {
                CargarJugadorSeleccionado();
            }
            else if (tipoModulo == "Inventario")
            {
                CargarInventarioSeleccionado();
            }
            else if (tipoModulo == "Convocatorias")
            {
                CargarConvocatoriaSeleccionada();
            }
            else if (tipoModulo == "Comunicación")
            {
                CargarPublicacionSeleccionada();
            }
        }


        private void CargarPublicacionSeleccionada()
        {
            if (dgvDatos.CurrentRow == null || dgvDatos.CurrentRow.IsNewRow) return;
            try
            {
                if (dgvDatos.CurrentRow.Cells["IdPublicacion"] != null) idPublicacionSeleccionada = Convert.ToInt32(dgvDatos.CurrentRow.Cells["IdPublicacion"].Value);
                if (dgvDatos.CurrentRow.Cells["Titulo"] != null) txtCampoUno.Text = Convert.ToString(dgvDatos.CurrentRow.Cells["Titulo"].Value);
                if (dgvDatos.CurrentRow.Cells["Contenido"] != null) txtCampoDos.Text = Convert.ToString(dgvDatos.CurrentRow.Cells["Contenido"].Value);
                if (cmbTipoPublicacion != null && dgvDatos.CurrentRow.Cells["TipoPublicacion"] != null)
                {
                    string tipo = Convert.ToString(dgvDatos.CurrentRow.Cells["TipoPublicacion"].Value).Trim().ToUpperInvariant();
                    if (!cmbTipoPublicacion.Items.Contains(tipo)) cmbTipoPublicacion.Items.Add(tipo);
                    cmbTipoPublicacion.SelectedItem = tipo;
                }
                btnGuardar.Text = "Guardar cambios";
            }
            catch { }
        }

        private void CargarJugadorSeleccionado()
        {
            if (dgvDatos.CurrentRow == null || dgvDatos.CurrentRow.IsNewRow) return;
            try
            {
                if (dgvDatos.CurrentRow.Cells["IdSocio"] != null) txtCampoUno.Text = Convert.ToString(dgvDatos.CurrentRow.Cells["IdSocio"].Value);
                if (cmbJugadorDeporte != null && dgvDatos.CurrentRow.Cells["Deporte"] != null)
                {
                    string deporte = Convert.ToString(dgvDatos.CurrentRow.Cells["Deporte"].Value);
                    if (cmbJugadorDeporte.Items.Contains(deporte)) cmbJugadorDeporte.SelectedItem = deporte;
                }
                if (dgvDatos.CurrentRow.Cells["Posicion"] != null) txtCampoTres.Text = Convert.ToString(dgvDatos.CurrentRow.Cells["Posicion"].Value);
                if (cmbJugadorDisponible != null && dgvDatos.CurrentRow.Cells["Disponible"] != null)
                {
                    string disponible = Convert.ToString(dgvDatos.CurrentRow.Cells["Disponible"].Value).Trim().ToUpperInvariant();
                    if (cmbJugadorDisponible.Items.Contains(disponible)) cmbJugadorDisponible.SelectedItem = disponible;
                }
                btnGuardar.Text = "Guardar cambios";
            }
            catch { }
        }

        private void CargarEventoSeleccionado()
        {
            if (dgvDatos.CurrentRow == null || dgvDatos.CurrentRow.IsNewRow) return;
            try
            {
                if (dgvDatos.CurrentRow.Cells["IdEvento"] != null) idEventoSeleccionado = Convert.ToInt32(dgvDatos.CurrentRow.Cells["IdEvento"].Value);
                if (dgvDatos.CurrentRow.Cells["Nombre"] != null) txtCampoUno.Text = Convert.ToString(dgvDatos.CurrentRow.Cells["Nombre"].Value);
                if (cmbEventoDeporte != null && dgvDatos.CurrentRow.Cells["Deporte"] != null)
                {
                    string deporte = Convert.ToString(dgvDatos.CurrentRow.Cells["Deporte"].Value);
                    if (!cmbEventoDeporte.Items.Contains(deporte)) cmbEventoDeporte.Items.Add(deporte);
                    cmbEventoDeporte.SelectedItem = deporte;
                }
                if (cmbEventoLugar != null && dgvDatos.CurrentRow.Cells["Lugar"] != null)
                {
                    string lugar = Convert.ToString(dgvDatos.CurrentRow.Cells["Lugar"].Value);
                    if (!cmbEventoLugar.Items.Contains(lugar)) cmbEventoLugar.Items.Add(lugar);
                    cmbEventoLugar.SelectedItem = lugar;
                }
                if (dgvDatos.CurrentRow.Cells["FechaEvento"] != null)
                {
                    DateTime fecha;
                    if (DateTime.TryParse(Convert.ToString(dgvDatos.CurrentRow.Cells["FechaEvento"].Value), out fecha)) txtCampoCuatro.Text = fecha.ToString("yyyy-MM-dd HH:mm");
                }
                if (txtEventoCupo != null && dgvDatos.CurrentRow.Cells["CupoEspectadores"] != null) txtEventoCupo.Text = Convert.ToString(dgvDatos.CurrentRow.Cells["CupoEspectadores"].Value);
                if (txtEventoEntrada != null && dgvDatos.CurrentRow.Cells["PrecioEntradaEspectador"] != null) txtEventoEntrada.Text = Convert.ToString(dgvDatos.CurrentRow.Cells["PrecioEntradaEspectador"].Value);
                if (txtEventoFee != null && dgvDatos.CurrentRow.Cells["PrecioParticipacionJugador"] != null) txtEventoFee.Text = Convert.ToString(dgvDatos.CurrentRow.Cells["PrecioParticipacionJugador"].Value);
                btnGuardar.Text = "Guardar cambios";
            }
            catch { }
        }

        private void CargarInventarioSeleccionado()
        {
            if (dgvDatos.CurrentRow == null || dgvDatos.CurrentRow.IsNewRow) return;
            try
            {
                if (dgvDatos.CurrentRow.Cells["Nombre"] != null) txtCampoUno.Text = Convert.ToString(dgvDatos.CurrentRow.Cells["Nombre"].Value);
                if (dgvDatos.CurrentRow.Cells["Cantidad"] != null) txtCampoDos.Text = Convert.ToString(dgvDatos.CurrentRow.Cells["Cantidad"].Value);
                if (dgvDatos.CurrentRow.Cells["Ubicacion"] != null) txtCampoTres.Text = Convert.ToString(dgvDatos.CurrentRow.Cells["Ubicacion"].Value);
                if (cmbInventarioEstado != null && dgvDatos.CurrentRow.Cells["Estado"] != null)
                {
                    string estado = Convert.ToString(dgvDatos.CurrentRow.Cells["Estado"].Value);
                    if (cmbInventarioEstado.Items.Contains(estado)) cmbInventarioEstado.SelectedItem = estado;
                }
            }
            catch { }
        }

        private void CargarConvocatoriaSeleccionada()
        {
            if (dgvDatos.CurrentRow == null || dgvDatos.CurrentRow.IsNewRow) return;
            try
            {
                if (dgvDatos.CurrentRow.Cells["IdEvento"] != null) txtCampoUno.Text = Convert.ToString(dgvDatos.CurrentRow.Cells["IdEvento"].Value);
                if (dgvDatos.CurrentRow.Cells["IdJugador"] != null) txtCampoDos.Text = Convert.ToString(dgvDatos.CurrentRow.Cells["IdJugador"].Value);
                if (cmbRespuestaConvocatoria != null && dgvDatos.CurrentRow.Cells["EstadoRespuesta"] != null)
                {
                    string respuesta = Convert.ToString(dgvDatos.CurrentRow.Cells["EstadoRespuesta"].Value).Trim().ToUpperInvariant();
                    if (cmbRespuestaConvocatoria.Items.Contains(respuesta)) cmbRespuestaConvocatoria.SelectedItem = respuesta;
                }
            }
            catch { }
        }

        private void AjustarStockInventario(bool sumar)
        {
            try
            {
                if (dgvDatos.CurrentRow == null || dgvDatos.CurrentRow.IsNewRow)
                    throw new Exception("Seleccione un artículo de la tabla.");
                int idInventario = Convert.ToInt32(dgvDatos.CurrentRow.Cells["IdInventario"].Value);
                int cantidad;
                if (!int.TryParse(txtCampoDos.Text.Trim(), out cantidad) || cantidad <= 0)
                    throw new Exception("Ingrese una cantidad mayor a cero.");
                string usuario = SessionManager.SesionIniciada ? SessionManager.ObtenerUsuarioActual().Username : "SIN_SESION";
                moduloClubBLL.ActualizarStockInventario(idInventario, cantidad, sumar, usuario);
                CargarDatos();
                MessageBox.Show(sumar ? "Stock sumado correctamente." : "Stock restado correctamente.");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarConfiguracionSeleccionada()
        {
            if (dgvDatos.CurrentRow == null || dgvDatos.CurrentRow.IsNewRow)
            {
                return;
            }
            try
            {
                txtCampoUno.Text = Convert.ToString(dgvDatos.CurrentRow.Cells["IdConfiguracion"].Value);
                txtCampoDos.Text = Convert.ToString(dgvDatos.CurrentRow.Cells["Concepto"].Value);
                txtCampoTres.Text = Convert.ToString(dgvDatos.CurrentRow.Cells["Importe"].Value);
                txtCampoCuatro.Text = Convert.ToString(dgvDatos.CurrentRow.Cells["Activo"].Value);
            }
            catch
            {
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                string usuario = SessionManager.SesionIniciada ? SessionManager.ObtenerUsuarioActual().Username : "SIN_SESION";

                if (tipoModulo == "Pagos")
                {
                    moduloClubBLL.RegistrarPago(Convert.ToInt32(txtCampoUno.Text), DateTime.Now, txtCampoDos.Text, Convert.ToDecimal(txtCampoTres.Text), txtCampoCuatro.Text, usuario);
                }
                else if (tipoModulo == "Jugadores")
                {
                    string deporteJugador = cmbJugadorDeporte != null && cmbJugadorDeporte.SelectedItem != null ? cmbJugadorDeporte.SelectedItem.ToString() : txtCampoDos.Text;
                    string disponibleJugador = cmbJugadorDisponible != null && cmbJugadorDisponible.SelectedItem != null ? cmbJugadorDisponible.SelectedItem.ToString() : txtCampoCuatro.Text;
                    moduloClubBLL.RegistrarJugador(Convert.ToInt32(txtCampoUno.Text), deporteJugador, txtCampoTres.Text, disponibleJugador, usuario);
                }
                else if (tipoModulo == "Eventos")
                {
                    DateTime fechaEvento;
                    if (!DateTime.TryParse(txtCampoCuatro.Text, out fechaEvento))
                    {
                        throw new Exception("Ingrese una fecha válida. Formato sugerido: yyyy-mm-dd hh:mm.");
                    }

                    int cupo;
                    decimal entrada;
                    decimal fee;
                    if (!int.TryParse(txtEventoCupo.Text.Trim(), out cupo)) throw new Exception("Ingrese un cupo de espectadores válido.");
                    if (!decimal.TryParse(txtEventoEntrada.Text.Trim(), out entrada)) throw new Exception("Ingrese una entrada de espectador válida.");
                    if (!decimal.TryParse(txtEventoFee.Text.Trim(), out fee)) throw new Exception("Ingrese un fee de jugador válido.");

                    if (idEventoSeleccionado > 0)
                    {
                        moduloClubBLL.ActualizarEvento(idEventoSeleccionado, txtCampoUno.Text, ValorCombo(cmbEventoDeporte, txtCampoDos), fechaEvento, ValorCombo(cmbEventoLugar, txtCampoTres), "PROGRAMADO", cupo, entrada, fee, usuario);
                    }
                    else
                    {
                        moduloClubBLL.RegistrarEvento(txtCampoUno.Text, ValorCombo(cmbEventoDeporte, txtCampoDos), fechaEvento, ValorCombo(cmbEventoLugar, txtCampoTres), "PROGRAMADO", usuario, cupo, entrada, fee);
                    }
                }
                else if (tipoModulo == "Finanzas")
                {
                    string tipoMovimiento = cmbTipoMovimiento != null && cmbTipoMovimiento.SelectedItem != null ? cmbTipoMovimiento.SelectedItem.ToString() : txtCampoUno.Text;
                    moduloClubBLL.RegistrarMovimientoFinanciero(DateTime.Now, tipoMovimiento, txtCampoDos.Text, Convert.ToDecimal(txtCampoTres.Text), usuario);
                }
                else if (tipoModulo == "Comunicación")
                {
                    string tipoPublicacion = cmbTipoPublicacion != null && cmbTipoPublicacion.SelectedItem != null ? cmbTipoPublicacion.SelectedItem.ToString() : txtCampoTres.Text;
                    if (idPublicacionSeleccionada > 0)
                    {
                        moduloClubBLL.ActualizarPublicacion(idPublicacionSeleccionada, txtCampoUno.Text, txtCampoDos.Text, tipoPublicacion, usuario);
                    }
                    else
                    {
                        moduloClubBLL.RegistrarPublicacion(txtCampoUno.Text, txtCampoDos.Text, tipoPublicacion, usuario);
                    }
                }
                else if (tipoModulo == "Insignias")
                {
                    if (cmbInsigniasDisponibles == null || cmbInsigniasDisponibles.SelectedValue == null)
                    {
                        throw new Exception("Seleccione una insignia válida.");
                    }

                    int idSocio = !string.IsNullOrWhiteSpace(txtCampoUno.Text) ? moduloClubBLL.ResolverIdSocio(txtCampoUno.Text) : idSocioInsigniaSeleccionado;
                    if (idSocio <= 0) throw new Exception("Busque o ingrese un socio antes de asignar una insignia.");
                    string nivelTexto = cmbNivelInsignia != null && cmbNivelInsignia.SelectedItem != null ? cmbNivelInsignia.SelectedItem.ToString() : txtCampoTres.Text;
                    int nivel = ExtraerPrimerEntero(nivelTexto);
                    moduloClubBLL.AsignarInsigniaSocio(idSocio, cmbInsigniasDisponibles.SelectedValue.ToString(), nivel, txtCampoCuatro.Text, usuario);
                }
                else if (tipoModulo == "Cuotas y fees")
                {
                    int idConfiguracion;
                    decimal importe;
                    if (!int.TryParse(txtCampoUno.Text.Trim(), out idConfiguracion))
                    {
                        throw new Exception("Seleccione una configuración de la tabla.");
                    }
                    if (!decimal.TryParse(txtCampoTres.Text.Trim(), out importe))
                    {
                        throw new Exception("Ingrese un importe válido.");
                    }
                    moduloClubBLL.GuardarConfiguracionGeneral(idConfiguracion, importe, txtCampoCuatro.Text, usuario);
                }
                else if (tipoModulo == "Inventario")
                {
                    string estadoInventario = cmbInventarioEstado != null && cmbInventarioEstado.SelectedItem != null ? cmbInventarioEstado.SelectedItem.ToString() : txtCampoCuatro.Text;
                    moduloClubBLL.RegistrarInventario(txtCampoUno.Text, Convert.ToInt32(txtCampoDos.Text), txtCampoTres.Text, estadoInventario, usuario);
                }
                else if (tipoModulo == "Ventas")
                {
                    string tipoVenta = cmbTipoVenta != null && cmbTipoVenta.SelectedItem != null ? cmbTipoVenta.SelectedItem.ToString() : txtCampoUno.Text;
                    moduloClubBLL.RegistrarVenta(DateTime.Now, tipoVenta, txtCampoDos.Text, Convert.ToDecimal(txtCampoTres.Text), usuario);
                }
                else if (tipoModulo == "Convocatorias")
                {
                    int idEvento;
                    int idJugador;
                    if (!int.TryParse(txtCampoUno.Text.Trim(), out idEvento)) throw new Exception("Ingrese un N° de evento válido o seleccione una convocatoria de la tabla.");
                    if (!int.TryParse(txtCampoDos.Text.Trim(), out idJugador)) throw new Exception("Ingrese un N° de jugador válido o seleccione una convocatoria de la tabla.");
                    string respuesta = cmbRespuestaConvocatoria != null && cmbRespuestaConvocatoria.SelectedItem != null ? cmbRespuestaConvocatoria.SelectedItem.ToString() : txtCampoTres.Text;
                    moduloClubBLL.RegistrarConvocatoria(idEvento, idJugador, respuesta, usuario);
                }
                else if (tipoModulo == "Resultados")
                {
                    moduloClubBLL.RegistrarResultadoPartido(Convert.ToInt32(txtCampoUno.Text), txtCampoDos.Text, txtCampoTres.Text, txtCampoCuatro.Text, usuario);
                }

                if (tipoModulo != "Convocatorias")
                {
                    LimpiarCampos();
                }
                CargarDatos();
                MessageBox.Show(tipoModulo == "Convocatorias" ? "Convocatoria actualizada correctamente." : "Registro guardado correctamente.");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int ExtraerPrimerEntero(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto)) throw new Exception("Seleccione un nivel válido.");
            string digitos = string.Empty;
            foreach (char caracter in texto.Trim())
            {
                if (char.IsDigit(caracter)) digitos += caracter;
                else if (digitos.Length > 0) break;
            }
            int valor;
            if (!int.TryParse(digitos, out valor) || valor <= 0) throw new Exception("Seleccione un nivel válido.");
            return valor;
        }

        private void btnActualizarInsigniaCatalogo_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbInsigniaCatalogoEditar == null || cmbInsigniaCatalogoEditar.SelectedValue == null)
                {
                    throw new Exception("Seleccione la insignia existente que desea modificar.");
                }
                string usuario = SessionManager.SesionIniciada ? SessionManager.ObtenerUsuarioActual().Username : "SIN_SESION";
                string nombre = txtNuevaInsigniaNombre.Text.Trim();
                string descripcion = txtNuevaInsigniaDescripcion.Text.Trim();
                string requisitos = SerializarNivelesInsignia();
                if (string.IsNullOrWhiteSpace(nombre)) throw new Exception("Ingrese el nombre de la insignia.");
                if (string.IsNullOrWhiteSpace(descripcion)) throw new Exception("Ingrese la descripción de la insignia.");
                if (string.IsNullOrWhiteSpace(requisitos)) throw new Exception("Agregue al menos un nivel con el botón + Agregar nivel.");

                string nombreArchivo = CopiarImagenInsignia(nombre, rutaImagenSeleccionada);
                if (string.IsNullOrWhiteSpace(nombreArchivo) && !string.IsNullOrWhiteSpace(txtNuevaInsigniaImagen.Text))
                {
                    nombreArchivo = txtNuevaInsigniaImagen.Text.Trim();
                }

                int idCatalogo = Convert.ToInt32(cmbInsigniaCatalogoEditar.SelectedValue);
                moduloClubBLL.ActualizarInsigniaCatalogo(idCatalogo, nombre, descripcion, nombreArchivo, "S", requisitos, usuario);
                CargarCatalogoInsignias();
                CargarDatos();
                MessageBox.Show("Insignia actualizada correctamente.");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSeleccionarImagen_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialogo = new OpenFileDialog())
            {
                dialogo.Filter = "Imágenes|*.png;*.jpg;*.jpeg;*.webp;*.bmp";
                dialogo.Title = "Seleccionar imagen de insignia";
                if (dialogo.ShowDialog() == DialogResult.OK)
                {
                    rutaImagenSeleccionada = dialogo.FileName;
                    txtNuevaInsigniaImagen.Text = Path.GetFileName(dialogo.FileName);
                }
            }
        }

        private void btnCrearInsignia_Click(object sender, EventArgs e)
        {
            try
            {
                string usuario = SessionManager.SesionIniciada ? SessionManager.ObtenerUsuarioActual().Username : "SIN_SESION";
                string nombre = txtNuevaInsigniaNombre.Text.Trim();
                string descripcion = txtNuevaInsigniaDescripcion.Text.Trim();
                string requisitos = SerializarNivelesInsignia();
                if (string.IsNullOrWhiteSpace(nombre)) throw new Exception("Ingrese el nombre de la insignia.");
                if (string.IsNullOrWhiteSpace(descripcion)) throw new Exception("Ingrese la descripción de la insignia.");
                if (string.IsNullOrWhiteSpace(requisitos))
                {
                    throw new Exception("Agregue al menos un nivel para la insignia con el botón + Agregar nivel.");
                }

                string nombreArchivo = CopiarImagenInsignia(nombre, rutaImagenSeleccionada);
                if (string.IsNullOrWhiteSpace(nombreArchivo) && !string.IsNullOrWhiteSpace(txtNuevaInsigniaImagen.Text))
                {
                    nombreArchivo = txtNuevaInsigniaImagen.Text.Trim();
                }

                moduloClubBLL.GuardarInsigniaCatalogo(nombre, descripcion, nombreArchivo, "S", requisitos, usuario);

                txtNuevaInsigniaNombre.Clear();
                txtNuevaInsigniaDescripcion.Clear();
                if (dgvNivelesInsignia != null) dgvNivelesInsignia.Rows.Clear();
                txtNuevaInsigniaImagen.Clear();
                rutaImagenSeleccionada = null;
                CargarCatalogoInsignias();
                CargarDatos();
                MessageBox.Show("Insignia creada correctamente. Ya está disponible para asignar a socios.");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string CopiarImagenInsignia(string nombre, string rutaOrigen)
        {
            if (string.IsNullOrWhiteSpace(rutaOrigen) || !File.Exists(rutaOrigen))
            {
                return string.Empty;
            }

            string carpetaDestino = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Insignias");
            if (!Directory.Exists(carpetaDestino))
            {
                Directory.CreateDirectory(carpetaDestino);
            }

            string extension = Path.GetExtension(rutaOrigen);
            string nombreLimpio = LimpiarNombreArchivo(nombre);
            string nombreArchivo = nombreLimpio + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
            string rutaDestino = Path.Combine(carpetaDestino, nombreArchivo);
            File.Copy(rutaOrigen, rutaDestino, true);
            return nombreArchivo;
        }

        private string LimpiarNombreArchivo(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
            {
                return "insignia";
            }

            StringBuilder resultado = new StringBuilder();
            foreach (char c in valor.ToLowerInvariant())
            {
                if (char.IsLetterOrDigit(c)) resultado.Append(c);
                else if (c == ' ' || c == '-' || c == '_') resultado.Append('_');
            }

            return resultado.Length == 0 ? "insignia" : resultado.ToString();
        }

        private void LimpiarCampos()
        {
            txtCampoUno.Clear();
            txtCampoDos.Clear();
            txtCampoTres.Clear();
            txtCampoCuatro.Clear();
            if (tipoModulo == "Comunicación")
            {
                idPublicacionSeleccionada = 0;
                if (btnGuardar != null) btnGuardar.Text = "Guardar";
            }
            if (tipoModulo == "Eventos")
            {
                idEventoSeleccionado = 0;
                txtCampoCuatro.Text = DateTime.Now.AddDays(7).ToString("yyyy-MM-dd HH:mm");
                if (txtEventoCupo != null) txtEventoCupo.Text = "100";
                if (txtEventoEntrada != null) txtEventoEntrada.Text = "3000";
                if (txtEventoFee != null) txtEventoFee.Text = "2000";
                if (btnGuardar != null) btnGuardar.Text = "Guardar evento";
                if (dgvDatos != null) dgvDatos.ClearSelection();
            }
        }

        private void btnNuevoEvento_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            CargarDatos();
        }

        private void FrmModuloClub_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }
    }
}
