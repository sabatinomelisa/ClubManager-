using System;
using System.Data;
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
        private Button btnGuardar;
        private Button btnActualizar;

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
            Text = tipoModulo;
            StartPosition = FormStartPosition.CenterScreen;
            Width = 940;
            Height = 560;

            lblCampoUno = CrearLabel(20, 20);
            txtCampoUno = CrearTextBox(20, 45);
            lblCampoDos = CrearLabel(240, 20);
            txtCampoDos = CrearTextBox(240, 45);
            lblCampoTres = CrearLabel(460, 20);
            txtCampoTres = CrearTextBox(460, 45);
            lblCampoCuatro = CrearLabel(680, 20);
            txtCampoCuatro = CrearTextBox(680, 45);

            btnGuardar = new Button();
            btnGuardar.Text = "Guardar";
            btnGuardar.Left = 20;
            btnGuardar.Top = 95;
            btnGuardar.Width = 100;
            btnGuardar.Click += btnGuardar_Click;

            btnActualizar = new Button();
            btnActualizar.Text = "Actualizar";
            btnActualizar.Left = 130;
            btnActualizar.Top = 95;
            btnActualizar.Width = 100;
            btnActualizar.Click += btnActualizar_Click;

            dgvDatos = new DataGridView();
            dgvDatos.Left = 20;
            dgvDatos.Top = 140;
            dgvDatos.Width = 880;
            dgvDatos.Height = 340;
            dgvDatos.ReadOnly = true;
            dgvDatos.AllowUserToAddRows = false;
            dgvDatos.AllowUserToDeleteRows = false;
            dgvDatos.AutoGenerateColumns = true;

            Controls.Add(btnGuardar);
            Controls.Add(btnActualizar);
            Controls.Add(dgvDatos);

            VisualStyleHelper.AplicarEstiloBase(this);
        }

        private Label CrearLabel(int left, int top)
        {
            Label label = new Label();
            label.Left = left;
            label.Top = top;
            label.Width = 200;
            Controls.Add(label);
            return label;
        }

        private TextBox CrearTextBox(int left, int top)
        {
            TextBox textBox = new TextBox();
            textBox.Left = left;
            textBox.Top = top;
            textBox.Width = 190;
            Controls.Add(textBox);
            return textBox;
        }

        private void ConfigurarModulo()
        {
            txtCampoUno.Enabled = true;
            txtCampoDos.Enabled = true;
            txtCampoTres.Enabled = true;
            txtCampoCuatro.Enabled = true;
            btnGuardar.Enabled = true;

            if (tipoModulo == "Pagos")
            {
                lblCampoUno.Text = "Id socio";
                lblCampoDos.Text = "Concepto";
                lblCampoTres.Text = "Importe";
                lblCampoCuatro.Text = "Estado";
            }
            else if (tipoModulo == "Jugadores")
            {
                lblCampoUno.Text = "Id socio";
                lblCampoDos.Text = "Deporte";
                lblCampoTres.Text = "Posición";
                lblCampoCuatro.Text = "Disponible S/N";
            }
            else if (tipoModulo == "Eventos")
            {
                lblCampoUno.Text = "Nombre";
                lblCampoDos.Text = "Deporte";
                lblCampoTres.Text = "Lugar";
                lblCampoCuatro.Text = "Estado";
            }
            else if (tipoModulo == "Finanzas")
            {
                lblCampoUno.Text = "Tipo INGRESO/EGRESO";
                lblCampoDos.Text = "Concepto";
                lblCampoTres.Text = "Importe";
                lblCampoCuatro.Text = "No usado";
                txtCampoCuatro.Enabled = false;
            }
            else if (tipoModulo == "Comunicación")
            {
                lblCampoUno.Text = "Título";
                lblCampoDos.Text = "Contenido";
                lblCampoTres.Text = "Tipo";
                lblCampoCuatro.Text = "No usado";
                txtCampoCuatro.Enabled = false;
            }
            else if (tipoModulo == "Insignias")
            {
                lblCampoUno.Text = "Id socio";
                lblCampoDos.Text = "Nombre";
                lblCampoTres.Text = "Motivo";
                lblCampoCuatro.Text = "No usado";
                txtCampoCuatro.Enabled = false;
            }
            else if (tipoModulo == "Inventario")
            {
                lblCampoUno.Text = "Nombre artículo";
                lblCampoDos.Text = "Cantidad";
                lblCampoTres.Text = "Ubicación";
                lblCampoCuatro.Text = "Estado";
            }
            else if (tipoModulo == "Ventas")
            {
                lblCampoUno.Text = "Tipo ENTRADA/BUFFET";
                lblCampoDos.Text = "Descripción";
                lblCampoTres.Text = "Importe";
                lblCampoCuatro.Text = "No usado";
                txtCampoCuatro.Enabled = false;
            }
            else if (tipoModulo == "Convocatorias")
            {
                lblCampoUno.Text = "Id evento";
                lblCampoDos.Text = "Id jugador";
                lblCampoTres.Text = "Estado respuesta";
                lblCampoCuatro.Text = "No usado";
                txtCampoCuatro.Enabled = false;
            }
            else if (tipoModulo == "Resultados")
            {
                lblCampoUno.Text = "Id evento";
                lblCampoDos.Text = "Equipo local";
                lblCampoTres.Text = "Equipo visitante";
                lblCampoCuatro.Text = "Resultado";
            }
            else if (tipoModulo == "Cuotas y fees")
            {
                lblCampoUno.Text = "Solo consulta";
                lblCampoDos.Text = "Solo consulta";
                lblCampoTres.Text = "Solo consulta";
                lblCampoCuatro.Text = "Solo consulta";
                txtCampoUno.Enabled = false;
                txtCampoDos.Enabled = false;
                txtCampoTres.Enabled = false;
                txtCampoCuatro.Enabled = false;
                btnGuardar.Enabled = false;
            }
            else if (tipoModulo == "Reportes")
            {
                lblCampoUno.Text = "Solo consulta";
                lblCampoDos.Text = "Solo consulta";
                lblCampoTres.Text = "Solo consulta";
                lblCampoCuatro.Text = "Solo consulta";
                txtCampoUno.Enabled = false;
                txtCampoDos.Enabled = false;
                txtCampoTres.Enabled = false;
                txtCampoCuatro.Enabled = false;
                btnGuardar.Enabled = false;
            }
            else
            {
                lblCampoUno.Text = "Campo 1";
                lblCampoDos.Text = "Campo 2";
                lblCampoTres.Text = "Campo 3";
                lblCampoCuatro.Text = "Campo 4";
            }
        }

        private void CargarDatos()
        {
            try
            {
                DataTable tabla;

                if (tipoModulo == "Pagos") tabla = moduloClubBLL.ConsultarPagos();
                else if (tipoModulo == "Jugadores") tabla = moduloClubBLL.ConsultarJugadores();
                else if (tipoModulo == "Eventos") tabla = moduloClubBLL.ConsultarEventos();
                else if (tipoModulo == "Finanzas") tabla = moduloClubBLL.ConsultarMovimientosFinancieros();
                else if (tipoModulo == "Comunicación") tabla = moduloClubBLL.ConsultarPublicaciones();
                else if (tipoModulo == "Insignias") tabla = moduloClubBLL.ConsultarInsignias();
                else if (tipoModulo == "Inventario") tabla = moduloClubBLL.ConsultarInventario();
                else if (tipoModulo == "Ventas") tabla = moduloClubBLL.ConsultarVentas();
                else if (tipoModulo == "Convocatorias") tabla = moduloClubBLL.ConsultarConvocatorias();
                else if (tipoModulo == "Resultados") tabla = moduloClubBLL.ConsultarResultadosPartidos();
                else if (tipoModulo == "Cuotas y fees") tabla = moduloClubBLL.ConsultarConfiguracionCuotas();
                else tabla = moduloClubBLL.ConsultarReportes();

                dgvDatos.DataSource = tabla;
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
                string usuario = SessionManager.SesionIniciada ? SessionManager.ObtenerUsuarioActual().Username : "SIN_SESION";

                if (tipoModulo == "Pagos")
                {
                    moduloClubBLL.RegistrarPago(Convert.ToInt32(txtCampoUno.Text), DateTime.Now, txtCampoDos.Text, Convert.ToDecimal(txtCampoTres.Text), txtCampoCuatro.Text, usuario);
                }
                else if (tipoModulo == "Jugadores")
                {
                    moduloClubBLL.RegistrarJugador(Convert.ToInt32(txtCampoUno.Text), txtCampoDos.Text, txtCampoTres.Text, txtCampoCuatro.Text, usuario);
                }
                else if (tipoModulo == "Eventos")
                {
                    moduloClubBLL.RegistrarEvento(txtCampoUno.Text, txtCampoDos.Text, DateTime.Now, txtCampoTres.Text, txtCampoCuatro.Text, usuario);
                }
                else if (tipoModulo == "Finanzas")
                {
                    moduloClubBLL.RegistrarMovimientoFinanciero(DateTime.Now, txtCampoUno.Text, txtCampoDos.Text, Convert.ToDecimal(txtCampoTres.Text), usuario);
                }
                else if (tipoModulo == "Comunicación")
                {
                    moduloClubBLL.RegistrarPublicacion(txtCampoUno.Text, txtCampoDos.Text, txtCampoTres.Text, usuario);
                }
                else if (tipoModulo == "Insignias")
                {
                    moduloClubBLL.RegistrarInsignia(Convert.ToInt32(txtCampoUno.Text), txtCampoDos.Text, txtCampoTres.Text, usuario);
                }
                else if (tipoModulo == "Inventario")
                {
                    moduloClubBLL.RegistrarInventario(txtCampoUno.Text, Convert.ToInt32(txtCampoDos.Text), txtCampoTres.Text, txtCampoCuatro.Text, usuario);
                }
                else if (tipoModulo == "Ventas")
                {
                    moduloClubBLL.RegistrarVenta(DateTime.Now, txtCampoUno.Text, txtCampoDos.Text, Convert.ToDecimal(txtCampoTres.Text), usuario);
                }
                else if (tipoModulo == "Convocatorias")
                {
                    moduloClubBLL.RegistrarConvocatoria(Convert.ToInt32(txtCampoUno.Text), Convert.ToInt32(txtCampoDos.Text), txtCampoTres.Text, usuario);
                }
                else if (tipoModulo == "Resultados")
                {
                    moduloClubBLL.RegistrarResultadoPartido(Convert.ToInt32(txtCampoUno.Text), txtCampoDos.Text, txtCampoTres.Text, txtCampoCuatro.Text, usuario);
                }

                LimpiarCampos();
                CargarDatos();
                MessageBox.Show("Registro guardado correctamente.");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarCampos()
        {
            txtCampoUno.Clear();
            txtCampoDos.Clear();
            txtCampoTres.Clear();
            txtCampoCuatro.Clear();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            CargarDatos();
        }
    }
}
