using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using BE;
using BLL;
using SERVICIOS;

namespace ClubManager
{
    public class FrmPortalSocio : Form
    {
        private readonly string seccion;
        private readonly ModuloClubBLL moduloClubBLL;
        private readonly SocioBLL socioBLL;
        private readonly ControlCambioBLL controlCambioBLL;
        private DataGridView dgvDatos;
        private Label lblTitulo;
        private Label lblDescripcion;
        private Label lblCampoUno;
        private Label lblCampoDos;
        private Label lblCampoTres;
        private TextBox txtCampoUno;
        private TextBox txtCampoDos;
        private TextBox txtCampoTres;
        private Button btnAccion;
        private Button btnActualizar;
        private Button btnVolver;
        private FlowLayoutPanel panelInsignias;

        public FrmPortalSocio(string seccion)
        {
            this.seccion = seccion;
            moduloClubBLL = new ModuloClubBLL();
            socioBLL = new SocioBLL();
            controlCambioBLL = new ControlCambioBLL();
            InicializarControles();
            ConfigurarSeccion();
            CargarDatos();
        }

        private void InicializarControles()
        {
            Text = seccion;
            StartPosition = FormStartPosition.CenterScreen;
            Width = 960;
            Height = 700;

            lblTitulo = new Label();
            lblTitulo.Left = 25;
            lblTitulo.Top = 25;
            lblTitulo.Width = 850;
            lblTitulo.Height = 34;
            lblTitulo.Font = new Font("Segoe UI", 15F, FontStyle.Bold);

            lblDescripcion = new Label();
            lblDescripcion.Left = 25;
            lblDescripcion.Top = 62;
            lblDescripcion.Width = 850;
            lblDescripcion.Height = 42;

            lblCampoUno = CrearLabel(25, 122, "Campo 1");
            txtCampoUno = CrearTextBox(25, 145);
            lblCampoDos = CrearLabel(250, 122, "Campo 2");
            txtCampoDos = CrearTextBox(250, 145);
            lblCampoTres = CrearLabel(475, 122, "Campo 3");
            txtCampoTres = CrearTextBox(475, 145);

            btnAccion = new Button();
            btnAccion.Left = 700;
            btnAccion.Top = 142;
            btnAccion.Width = 180;
            btnAccion.Height = 28;
            btnAccion.Click += btnAccion_Click;

            btnActualizar = new Button();
            btnActualizar.Text = "Actualizar";
            btnActualizar.Left = 700;
            btnActualizar.Top = 178;
            btnActualizar.Width = 180;
            btnActualizar.Height = 28;
            btnActualizar.Click += delegate { CargarDatos(); };

            btnVolver = new Button();
            btnVolver.Text = "Volver al menú";
            btnVolver.Left = 700;
            btnVolver.Top = 214;
            btnVolver.Width = 180;
            btnVolver.Height = 28;
            btnVolver.Click += delegate { Close(); };
            CancelButton = btnVolver;

            panelInsignias = new FlowLayoutPanel();
            panelInsignias.Left = 25;
            panelInsignias.Top = 115;
            panelInsignias.Width = 890;
            panelInsignias.Height = 300;
            panelInsignias.FlowDirection = FlowDirection.LeftToRight;
            panelInsignias.WrapContents = true;
            panelInsignias.AutoScroll = true;
            panelInsignias.BackColor = Color.FromArgb(105, 0, 0, 0);
            panelInsignias.Padding = new Padding(10);
            panelInsignias.Visible = false;

            dgvDatos = new DataGridView();
            dgvDatos.Left = 25;
            dgvDatos.Top = 285;
            dgvDatos.Width = 890;
            dgvDatos.Height = 335;
            dgvDatos.ReadOnly = true;
            dgvDatos.AllowUserToAddRows = false;
            dgvDatos.AllowUserToDeleteRows = false;
            dgvDatos.AutoGenerateColumns = true;
            dgvDatos.SelectionChanged += dgvDatos_SelectionChanged;

            Controls.Add(lblTitulo);
            Controls.Add(lblDescripcion);
            Controls.Add(lblCampoUno);
            Controls.Add(txtCampoUno);
            Controls.Add(lblCampoDos);
            Controls.Add(txtCampoDos);
            Controls.Add(lblCampoTres);
            Controls.Add(txtCampoTres);
            Controls.Add(btnAccion);
            Controls.Add(btnActualizar);
            Controls.Add(btnVolver);
            Controls.Add(panelInsignias);
            Controls.Add(dgvDatos);

            VisualStyleHelper.AplicarEstiloBase(this);
        }

        private Label CrearLabel(int left, int top, string texto)
        {
            Label label = new Label();
            label.Text = texto;
            label.Left = left;
            label.Top = top;
            label.Width = 210;
            return label;
        }

        private TextBox CrearTextBox(int left, int top)
        {
            TextBox textBox = new TextBox();
            textBox.Left = left;
            textBox.Top = top;
            textBox.Width = 205;
            return textBox;
        }

        private void ConfigurarSeccion()
        {
            lblTitulo.Text = seccion;
            lblDescripcion.Text = "Consulta y gestión personal del socio conectado.";
            btnAccion.Visible = false;
            btnActualizar.Visible = true;
            btnActualizar.Top = 178;
            btnVolver.Visible = true;
            btnVolver.Top = 214;
            panelInsignias.Visible = false;
            dgvDatos.Top = 285;
            dgvDatos.Height = 335;
            MostrarCampos(false);
            txtCampoUno.Enabled = true;
            txtCampoDos.Enabled = true;
            txtCampoTres.Enabled = true;

            if (seccion == "Mi perfil")
            {
                lblDescripcion.Text = "Datos personales del socio conectado. Las modificaciones administrativas se hacen desde Gestión de socios.";
            }
            else if (seccion == "Mis pagos")
            {
                lblDescripcion.Text = "Historial de cuotas y pagos del socio conectado. El importe se calcula según el tipo de socio.";
                MostrarCampos(true);
                lblCampoUno.Text = "Concepto";
                lblCampoDos.Text = "Importe";
                lblCampoTres.Text = "Estado";
                ConfigurarCuotaPredeterminada();
                txtCampoTres.Text = "PAGADO";
                btnAccion.Text = "Registrar pago";
                btnAccion.Visible = true;
            }
            else if (seccion == "Eventos disponibles")
            {
                lblDescripcion.Text = UsuarioActualEsSocioPleno()
                    ? "Eventos deportivos publicados. Puede comprar entrada como espectador o registrarse como participante."
                    : "Eventos deportivos publicados. El Socio Simple puede pagar entrada para asistir como espectador, pero no puede participar como jugador.";
                MostrarCampos(true);
                lblCampoUno.Text = "Id evento";
                lblCampoDos.Text = "Tipo asistencia";
                lblCampoTres.Text = "Importe";
                txtCampoDos.Text = "ESPECTADOR";
                txtCampoDos.Enabled = UsuarioActualEsSocioPleno();
                txtCampoTres.Text = "Automático";
                txtCampoTres.Enabled = false;
                btnAccion.Text = UsuarioActualEsSocioPleno() ? "Registrar asistencia" : "Comprar entrada";
                btnAccion.Visible = true;
            }
            else if (seccion == "Mis entradas")
            {
                lblDescripcion.Text = "Entradas y asistencias del socio conectado. Para comprar una entrada, ingrese el Id del evento.";
                MostrarCampos(true);
                lblCampoUno.Text = "Id evento";
                lblCampoDos.Text = "Tipo asistencia";
                lblCampoTres.Text = "Importe";
                txtCampoDos.Text = "ESPECTADOR";
                txtCampoDos.Enabled = UsuarioActualEsSocioPleno();
                txtCampoTres.Text = "Automático";
                txtCampoTres.Enabled = false;
                btnAccion.Text = UsuarioActualEsSocioPleno() ? "Registrar asistencia" : "Comprar entrada";
                btnAccion.Visible = true;
            }
            else if (seccion == "Comunicados")
            {
                lblDescripcion.Text = "Noticias, avisos y comunicación interna del club.";
            }
            else if (seccion == "Mi historial")
            {
                lblDescripcion.Text = "Resumen personal de pagos e insignias.";
            }
            else if (seccion == "Historial de mail")
            {
                lblDescripcion.Text = "Historial de mails del socio conectado. Seleccione un registro anterior para restaurarlo.";
                lblCampoUno.Visible = true;
                txtCampoUno.Visible = true;
                lblCampoUno.Text = "Id histórico";
                txtCampoUno.Enabled = true;
                lblCampoDos.Visible = false;
                txtCampoDos.Visible = false;
                lblCampoTres.Visible = false;
                txtCampoTres.Visible = false;
                btnAccion.Text = "Volver al mail seleccionado";
                btnAccion.Visible = true;
            }
            else if (seccion == "Equipos y disponibilidad")
            {
                lblDescripcion.Text = "Alta o actualización de disponibilidad deportiva del socio pleno.";
                MostrarCampos(true);
                lblCampoUno.Text = "Deporte";
                lblCampoDos.Text = "Posición";
                lblCampoTres.Text = "Disponible S/N";
                txtCampoTres.Text = "S";
                btnAccion.Text = "Anotarme";
                btnAccion.Visible = true;
            }
            else if (seccion == "Mis convocatorias")
            {
                lblDescripcion.Text = "Convocatorias deportivas asociadas al jugador del socio conectado.";
            }
            else if (seccion == "Mis insignias")
            {
                lblDescripcion.Text = "Insignias automáticas del socio pleno. Cada tarjeta explica el significado del nivel alcanzado.";
                btnActualizar.Top = 76;
                btnVolver.Top = 112;
                panelInsignias.Top = 155;
                panelInsignias.Height = 265;
                panelInsignias.Visible = true;
                dgvDatos.Top = 440;
                dgvDatos.Height = 180;
            }
            else if (seccion == "Resultados")
            {
                lblDescripcion.Text = "Resultados de partidos y eventos deportivos.";
            }
        }

        private void MostrarCampos(bool mostrar)
        {
            lblCampoUno.Visible = mostrar;
            lblCampoDos.Visible = mostrar;
            lblCampoTres.Visible = mostrar;
            txtCampoUno.Visible = mostrar;
            txtCampoDos.Visible = mostrar;
            txtCampoTres.Visible = mostrar;
        }

        private void CargarDatos()
        {
            try
            {
                int idSocio = ObtenerIdSocioActual();

                if (seccion == "Mi perfil")
                {
                    dgvDatos.DataSource = CrearTablaPerfil(idSocio);
                }
                else if (seccion == "Mis pagos")
                {
                    dgvDatos.DataSource = FiltrarPorEntero(moduloClubBLL.ConsultarPagos(), "IdSocio", idSocio);
                }
                else if (seccion == "Eventos disponibles")
                {
                    dgvDatos.DataSource = moduloClubBLL.ConsultarEventos();
                }
                else if (seccion == "Mis entradas")
                {
                    dgvDatos.DataSource = moduloClubBLL.ConsultarAsistenciasSocio(idSocio);
                }
                else if (seccion == "Comunicados")
                {
                    dgvDatos.DataSource = moduloClubBLL.ConsultarPublicaciones();
                }
                else if (seccion == "Mi historial")
                {
                    dgvDatos.DataSource = CrearTablaHistorial(idSocio);
                }
                else if (seccion == "Historial de mail")
                {
                    dgvDatos.DataSource = controlCambioBLL.ListarHistorialMailSocio(idSocio);
                    ConfigurarGrillaHistorialMail();
                }
                else if (seccion == "Equipos y disponibilidad")
                {
                    dgvDatos.DataSource = FiltrarPorEntero(moduloClubBLL.ConsultarJugadores(), "IdSocio", idSocio);
                }
                else if (seccion == "Mis convocatorias")
                {
                    dgvDatos.DataSource = FiltrarConvocatoriasDelSocio(idSocio);
                }
                else if (seccion == "Mis insignias")
                {
                    DataTable insignias = moduloClubBLL.ConsultarInsigniasCalculadasSocio(idSocio);
                    dgvDatos.DataSource = insignias;
                    ConfigurarGrillaInsignias();
                    CargarTarjetasInsignias(insignias);
                }
                else if (seccion == "Resultados")
                {
                    dgvDatos.DataSource = moduloClubBLL.ConsultarResultadosPartidos();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarGrillaHistorialMail()
        {
            if (dgvDatos.Columns == null || dgvDatos.Columns.Count == 0)
            {
                return;
            }

            RenombrarColumnaSiExiste("IdHistorico", "Id histórico");
            RenombrarColumnaSiExiste("IdSocio", "Id socio");
            RenombrarColumnaSiExiste("Mail", "Mail anterior");
            RenombrarColumnaSiExiste("FechaCreacion", "Fecha registro");

            AjustarAnchoColumnaSiExiste("IdHistorico", 90);
            AjustarAnchoColumnaSiExiste("IdSocio", 80);
            AjustarAnchoColumnaSiExiste("Mail", 260);
            AjustarAnchoColumnaSiExiste("FechaCreacion", 160);
        }

        private void ConfigurarGrillaInsignias()
        {
            if (dgvDatos.Columns == null || dgvDatos.Columns.Count == 0)
            {
                return;
            }

            OcultarColumnaSiExiste("Imagen");

            RenombrarColumnaSiExiste("Insignia", "Insignia");
            RenombrarColumnaSiExiste("Nivel", "Nivel");
            RenombrarColumnaSiExiste("TituloNivel", "Título");
            RenombrarColumnaSiExiste("Progreso", "Progreso");
            RenombrarColumnaSiExiste("DescripcionNivel", "Descripción");
            RenombrarColumnaSiExiste("Regla", "Regla");
            RenombrarColumnaSiExiste("Estado", "Estado");

            AjustarAnchoColumnaSiExiste("Insignia", 130);
            AjustarAnchoColumnaSiExiste("Nivel", 55);
            AjustarAnchoColumnaSiExiste("TituloNivel", 130);
            AjustarAnchoColumnaSiExiste("Progreso", 150);
            AjustarAnchoColumnaSiExiste("DescripcionNivel", 300);
            AjustarAnchoColumnaSiExiste("Regla", 230);
            AjustarAnchoColumnaSiExiste("Estado", 90);
        }

        private void OcultarColumnaSiExiste(string nombreColumna)
        {
            if (dgvDatos.Columns.Contains(nombreColumna))
            {
                dgvDatos.Columns[nombreColumna].Visible = false;
            }
        }

        private void RenombrarColumnaSiExiste(string nombreColumna, string encabezado)
        {
            if (dgvDatos.Columns.Contains(nombreColumna))
            {
                dgvDatos.Columns[nombreColumna].HeaderText = encabezado;
            }
        }

        private void AjustarAnchoColumnaSiExiste(string nombreColumna, int ancho)
        {
            if (dgvDatos.Columns.Contains(nombreColumna))
            {
                dgvDatos.Columns[nombreColumna].Width = ancho;
            }
        }

        private void btnAccion_Click(object sender, EventArgs e)
        {
            try
            {
                int idSocio = ObtenerIdSocioActual();
                string usuario = SessionManager.ObtenerUsuarioActual().Username;

                if (seccion == "Mis pagos")
                {
                    decimal importe;
                    if (!decimal.TryParse(txtCampoDos.Text.Trim(), out importe))
                    {
                        MessageBox.Show("Ingresar un importe válido.");
                        return;
                    }

                    moduloClubBLL.RegistrarPago(idSocio, DateTime.Now, txtCampoUno.Text.Trim(), importe, txtCampoTres.Text.Trim(), usuario);
                    MessageBox.Show("Pago registrado correctamente.");
                }
                else if (seccion == "Equipos y disponibilidad")
                {
                    moduloClubBLL.RegistrarJugador(idSocio, txtCampoUno.Text.Trim(), txtCampoDos.Text.Trim(), txtCampoTres.Text.Trim(), usuario);
                    MessageBox.Show("Disponibilidad deportiva registrada correctamente.");
                }
                else if (seccion == "Eventos disponibles" || seccion == "Mis entradas")
                {
                    int idEvento;
                    if (!int.TryParse(txtCampoUno.Text.Trim(), out idEvento))
                    {
                        MessageBox.Show("Ingresar un Id de evento válido.");
                        return;
                    }

                    string tipoAsistencia = UsuarioActualEsSocioPleno() ? txtCampoDos.Text.Trim().ToUpper() : "ESPECTADOR";
                    moduloClubBLL.RegistrarAsistenciaEventoSocio(idSocio, idEvento, tipoAsistencia, usuario);
                    MessageBox.Show(tipoAsistencia == "PARTICIPANTE"
                        ? "Participación deportiva registrada y cobrada correctamente."
                        : "Entrada de espectador registrada y cobrada correctamente.");
                }
                else if (seccion == "Historial de mail")
                {
                    int idHistorico;
                    if (!int.TryParse(txtCampoUno.Text.Trim(), out idHistorico))
                    {
                        MessageBox.Show("Seleccionar un mail histórico válido.");
                        return;
                    }

                    controlCambioBLL.VolverAlMailHistorico(idSocio, idHistorico, usuario);
                    MessageBox.Show("Mail histórico restaurado correctamente.");
                }

                CargarDatos();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void CargarTarjetasInsignias(DataTable insignias)
        {
            panelInsignias.Controls.Clear();

            if (insignias == null)
            {
                return;
            }

            foreach (DataRow fila in insignias.Rows)
            {
                panelInsignias.Controls.Add(CrearTarjetaInsignia(fila));
            }
        }

        private Panel CrearTarjetaInsignia(DataRow fila)
        {
            Panel tarjeta = new Panel();
            tarjeta.Width = 410;
            tarjeta.Height = 130;
            tarjeta.BackColor = Color.FromArgb(95, 75, 12, 24);
            tarjeta.Margin = new Padding(6);

            PictureBox imagen = new PictureBox();
            imagen.Left = 8;
            imagen.Top = 8;
            imagen.Width = 112;
            imagen.Height = 112;
            imagen.SizeMode = PictureBoxSizeMode.Zoom;
            imagen.BackColor = Color.Transparent;

            Image imagenInsignia = CargarImagenInsignia(ObtenerValorFila(fila, "Imagen"));
            if (imagenInsignia != null)
            {
                imagen.Image = imagenInsignia;
            }

            Label numero = new Label();
            numero.Left = 88;
            numero.Top = 88;
            numero.Width = 34;
            numero.Height = 28;
            numero.TextAlign = ContentAlignment.MiddleCenter;
            numero.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            numero.ForeColor = Color.White;
            numero.BackColor = Color.FromArgb(170, 75, 12, 24);
            numero.Text = ObtenerValorFila(fila, "Nivel");

            Label titulo = new Label();
            titulo.Left = 130;
            titulo.Top = 8;
            titulo.Width = 260;
            titulo.Height = 28;
            titulo.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            titulo.ForeColor = Color.White;
            titulo.BackColor = Color.Transparent;
            titulo.Text = ObtenerValorFila(fila, "TituloNivel");

            Label descripcion = new Label();
            descripcion.Left = 130;
            descripcion.Top = 38;
            descripcion.Width = 260;
            descripcion.Height = 46;
            descripcion.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular);
            descripcion.ForeColor = Color.Gainsboro;
            descripcion.BackColor = Color.Transparent;
            descripcion.Text = ObtenerValorFila(fila, "DescripcionNivel");

            Label progreso = new Label();
            progreso.Left = 130;
            progreso.Top = 88;
            progreso.Width = 260;
            progreso.Height = 18;
            progreso.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            progreso.ForeColor = Color.White;
            progreso.BackColor = Color.Transparent;
            progreso.Text = ObtenerValorFila(fila, "Progreso");

            Label regla = new Label();
            regla.Left = 130;
            regla.Top = 108;
            regla.Width = 260;
            regla.Height = 18;
            regla.Font = new Font("Segoe UI", 7.5F, FontStyle.Italic);
            regla.ForeColor = Color.LightGray;
            regla.BackColor = Color.Transparent;
            regla.Text = ObtenerValorFila(fila, "Regla");

            tarjeta.Controls.Add(imagen);
            tarjeta.Controls.Add(numero);
            tarjeta.Controls.Add(titulo);
            tarjeta.Controls.Add(descripcion);
            tarjeta.Controls.Add(progreso);
            tarjeta.Controls.Add(regla);

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

        private void dgvDatos_SelectionChanged(object sender, EventArgs e)
        {
            if (seccion != "Historial de mail" || dgvDatos.CurrentRow == null)
            {
                return;
            }

            object valor = null;
            try
            {
                if (dgvDatos.Columns.Contains("IdHistorico"))
                {
                    valor = dgvDatos.CurrentRow.Cells["IdHistorico"].Value;
                }
                else if (dgvDatos.Columns.Contains("Id histórico"))
                {
                    valor = dgvDatos.CurrentRow.Cells["Id histórico"].Value;
                }
            }
            catch
            {
                valor = null;
            }

            if (valor != null)
            {
                txtCampoUno.Text = valor.ToString();
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

        private bool UsuarioActualEsSocioPleno()
        {
            if (!SessionManager.SesionIniciada)
            {
                return false;
            }

            UsuarioBE usuario = SessionManager.ObtenerUsuarioActual();
            if (usuario == null)
            {
                return false;
            }

            return usuario.IdRol == 3 || string.Equals(usuario.NombreRol, "Socio Pleno", StringComparison.OrdinalIgnoreCase);
        }

        private void ConfigurarCuotaPredeterminada()
        {
            try
            {
                int idSocio = ObtenerIdSocioActual();
                DataTable cuota = moduloClubBLL.ObtenerCuotaSocio(idSocio);
                if (cuota.Rows.Count > 0)
                {
                    txtCampoUno.Text = cuota.Rows[0]["Concepto"].ToString();
                    txtCampoDos.Text = cuota.Rows[0]["Importe"].ToString();
                    return;
                }
            }
            catch
            {
            }

            txtCampoUno.Text = UsuarioActualEsSocioPleno() ? "Cuota mensual Socio Pleno" : "Cuota mensual Socio Simple";
            txtCampoDos.Text = UsuarioActualEsSocioPleno() ? "25000" : "15000";
        }

        private int ObtenerIdSocioActual()
        {
            if (!SessionManager.SesionIniciada)
            {
                throw new Exception("Sesión no iniciada.");
            }

            UsuarioBE usuario = SessionManager.ObtenerUsuarioActual();
            if (usuario == null || usuario.IdSocio <= 0)
            {
                throw new Exception("No se pudo identificar el socio conectado.");
            }

            return usuario.IdSocio;
        }

        private DataTable CrearTablaPerfil(int idSocio)
        {
            SocioBE socio = socioBLL.ObtenerSocio(idSocio);
            DataTable tabla = new DataTable();
            tabla.Columns.Add("Campo");
            tabla.Columns.Add("Valor");

            if (socio != null)
            {
                tabla.Rows.Add("Id", socio.IdSocio);
                tabla.Rows.Add("Documento", socio.TipoDocumento + " " + socio.NumeroDocumento);
                tabla.Rows.Add("Nombre", socio.Nombre);
                tabla.Rows.Add("Apellido", socio.Apellido);
                tabla.Rows.Add("Fecha nacimiento", socio.FechaNacimiento.ToString("yyyy-MM-dd"));
                tabla.Rows.Add("Nacionalidad", socio.Nacionalidad);
                tabla.Rows.Add("Mail", socio.Mail);
                tabla.Rows.Add("Teléfono", socio.Telefono);
                tabla.Rows.Add("Activo", socio.Activo);
            }

            return tabla;
        }

        private DataTable CrearTablaHistorial(int idSocio)
        {
            DataTable historial = new DataTable();
            historial.Columns.Add("Tipo");
            historial.Columns.Add("Fecha");
            historial.Columns.Add("Detalle");
            historial.Columns.Add("Importe");
            historial.Columns.Add("Estado");

            DataTable pagos = FiltrarPorEntero(moduloClubBLL.ConsultarPagos(), "IdSocio", idSocio);
            foreach (DataRow pago in pagos.Rows)
            {
                historial.Rows.Add("Pago", pago["FechaPago"], pago["Concepto"], pago["Importe"], pago["Estado"]);
            }

            DataTable asistencias = moduloClubBLL.ConsultarAsistenciasSocio(idSocio);
            foreach (DataRow asistencia in asistencias.Rows)
            {
                historial.Rows.Add("Evento", asistencia["FechaRegistro"], asistencia["TipoAsistencia"] + " - " + asistencia["Evento"], asistencia["Importe"], asistencia["Pagado"]);
            }

            DataTable insignias = FiltrarPorEntero(moduloClubBLL.ConsultarInsignias(), "IdSocio", idSocio);
            foreach (DataRow insignia in insignias.Rows)
            {
                historial.Rows.Add("Insignia", insignia["FechaOtorgamiento"], insignia["Nombre"] + " - " + insignia["Motivo"], string.Empty, string.Empty);
            }

            return historial;
        }

        private DataTable FiltrarConvocatoriasDelSocio(int idSocio)
        {
            DataTable jugadores = FiltrarPorEntero(moduloClubBLL.ConsultarJugadores(), "IdSocio", idSocio);
            DataTable convocatorias = moduloClubBLL.ConsultarConvocatorias();
            DataTable resultado = convocatorias.Clone();

            foreach (DataRow jugador in jugadores.Rows)
            {
                int idJugador;
                if (!int.TryParse(jugador["IdJugador"].ToString(), out idJugador))
                {
                    continue;
                }

                foreach (DataRow convocatoria in convocatorias.Rows)
                {
                    if (convocatorias.Columns.Contains("IdJugador") && Convert.ToInt32(convocatoria["IdJugador"]) == idJugador)
                    {
                        resultado.ImportRow(convocatoria);
                    }
                }
            }

            return resultado;
        }

        private DataTable FiltrarPorEntero(DataTable origen, string columna, int valor)
        {
            DataTable resultado = origen.Clone();

            if (!origen.Columns.Contains(columna))
            {
                return resultado;
            }

            foreach (DataRow fila in origen.Rows)
            {
                int valorFila;
                if (int.TryParse(fila[columna].ToString(), out valorFila) && valorFila == valor)
                {
                    resultado.ImportRow(fila);
                }
            }

            return resultado;
        }
    }
}
