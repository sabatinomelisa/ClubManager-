using System;
using System.Data;
using System.Data.SqlClient;
using SERVICIOS;

namespace DAL
{
    public class ModuloClubDAL
    {
        private readonly string connectionString;

        public ModuloClubDAL()
        {
            connectionString = ConnectionStringProvider.Instancia.ObtenerConnectionString();
        }

        public DataTable Consultar(string procedimiento)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(procedimiento, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                command.CommandType = CommandType.StoredProcedure;
                DataTable tabla = new DataTable();
                adapter.Fill(tabla);
                return tabla;
            }
        }

        public DataTable ConsultarInsigniasCalculadasSocio(int idSocio)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand("ConsultarInsigniasCalculadasSocio", connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@idSocio", SqlDbType.Int).Value = idSocio;
                DataTable tabla = new DataTable();
                adapter.Fill(tabla);
                return tabla;
            }
        }


        public int ResolverIdSocio(string socioOUsuario)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand("ResolverIdSocio", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@socioOUsuario", SqlDbType.NVarChar, 100).Value = socioOUsuario;
                connection.Open();
                object resultado = command.ExecuteScalar();
                if (resultado == null || resultado == DBNull.Value)
                {
                    return 0;
                }
                return Convert.ToInt32(resultado);
            }
        }

        public string ObtenerRolSocio(int idSocio)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(@"
                SELECT TOP 1 r.Nombre
                FROM dbo.Usuario u
                INNER JOIN dbo.Rol r ON r.IdRol = u.IdRol
                WHERE u.IdSocio = @idSocio AND u.Activo = 'S'", connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.Add("@idSocio", SqlDbType.Int).Value = idSocio;
                connection.Open();
                object resultado = command.ExecuteScalar();
                return resultado == null || resultado == DBNull.Value ? string.Empty : Convert.ToString(resultado);
            }
        }

        public int GuardarConfiguracionGeneral(int idConfiguracion, decimal importe, string activo)
        {
            return EjecutarRegistro("GuardarConfiguracionGeneral", command =>
            {
                command.Parameters.Add("@idConfiguracion", SqlDbType.Int).Value = idConfiguracion;
                AgregarDecimal(command, "@importe", importe);
                command.Parameters.Add("@activo", SqlDbType.Char, 1).Value = string.IsNullOrWhiteSpace(activo) ? "S" : activo.Substring(0, 1).ToUpper();
            });
        }

        public int RegistrarPago(int idSocio, DateTime fechaPago, string concepto, decimal importe, string estado)
        {
            return EjecutarRegistro("RegistrarPago", command =>
            {
                command.Parameters.Add("@idSocio", SqlDbType.Int).Value = idSocio;
                command.Parameters.Add("@fechaPago", SqlDbType.DateTime).Value = fechaPago;
                command.Parameters.Add("@concepto", SqlDbType.NVarChar, 100).Value = concepto;
                AgregarDecimal(command, "@importe", importe);
                command.Parameters.Add("@estado", SqlDbType.NVarChar, 30).Value = estado;
            });
        }

        public int RegistrarJugador(int idSocio, string deporte, string posicion, string disponible)
        {
            return EjecutarRegistro("RegistrarJugador", command =>
            {
                command.Parameters.Add("@idSocio", SqlDbType.Int).Value = idSocio;
                command.Parameters.Add("@deporte", SqlDbType.NVarChar, 80).Value = deporte;
                command.Parameters.Add("@posicion", SqlDbType.NVarChar, 80).Value = posicion;
                command.Parameters.Add("@disponible", SqlDbType.Char, 1).Value = disponible;
            });
        }

        public int RegistrarEvento(string nombre, string deporte, DateTime fechaEvento, string lugar, string estado, int cupoEspectadores, decimal precioEntradaEspectador, decimal precioParticipacionJugador)
        {
            return EjecutarRegistro("RegistrarEventoDeportivo", command =>
            {
                command.Parameters.Add("@nombre", SqlDbType.NVarChar, 120).Value = nombre;
                command.Parameters.Add("@deporte", SqlDbType.NVarChar, 80).Value = deporte;
                command.Parameters.Add("@fechaEvento", SqlDbType.DateTime).Value = fechaEvento;
                command.Parameters.Add("@lugar", SqlDbType.NVarChar, 120).Value = lugar;
                command.Parameters.Add("@estado", SqlDbType.NVarChar, 30).Value = estado;
                command.Parameters.Add("@cupoEspectadores", SqlDbType.Int).Value = cupoEspectadores;
                AgregarDecimal(command, "@precioEntradaEspectador", precioEntradaEspectador);
                AgregarDecimal(command, "@precioParticipacionJugador", precioParticipacionJugador);
            });
        }

        public int ActualizarEvento(int idEvento, string nombre, string deporte, DateTime fechaEvento, string lugar, string estado, int cupoEspectadores, decimal precioEntradaEspectador, decimal precioParticipacionJugador)
        {
            return EjecutarRegistro("ActualizarEventoDeportivo", command =>
            {
                command.Parameters.Add("@idEvento", SqlDbType.Int).Value = idEvento;
                command.Parameters.Add("@nombre", SqlDbType.NVarChar, 120).Value = nombre;
                command.Parameters.Add("@deporte", SqlDbType.NVarChar, 80).Value = deporte;
                command.Parameters.Add("@fechaEvento", SqlDbType.DateTime).Value = fechaEvento;
                command.Parameters.Add("@lugar", SqlDbType.NVarChar, 120).Value = lugar;
                command.Parameters.Add("@estado", SqlDbType.NVarChar, 30).Value = estado;
                command.Parameters.Add("@cupoEspectadores", SqlDbType.Int).Value = cupoEspectadores;
                AgregarDecimal(command, "@precioEntradaEspectador", precioEntradaEspectador);
                AgregarDecimal(command, "@precioParticipacionJugador", precioParticipacionJugador);
            });
        }

        public int RegistrarMovimientoFinanciero(DateTime fechaMovimiento, string tipoMovimiento, string concepto, decimal importe)
        {
            return EjecutarRegistro("RegistrarMovimientoFinanciero", command =>
            {
                command.Parameters.Add("@fechaMovimiento", SqlDbType.DateTime).Value = fechaMovimiento;
                command.Parameters.Add("@tipoMovimiento", SqlDbType.NVarChar, 20).Value = tipoMovimiento;
                command.Parameters.Add("@concepto", SqlDbType.NVarChar, 120).Value = concepto;
                AgregarDecimal(command, "@importe", importe);
            });
        }

        public int RegistrarPublicacion(string titulo, string contenido, string tipoPublicacion, string usuarioAutor)
        {
            return EjecutarRegistro("RegistrarPublicacion", command =>
            {
                command.Parameters.Add("@titulo", SqlDbType.NVarChar, 120).Value = titulo;
                command.Parameters.Add("@contenido", SqlDbType.NVarChar, -1).Value = contenido;
                command.Parameters.Add("@tipoPublicacion", SqlDbType.NVarChar, 50).Value = tipoPublicacion;
                command.Parameters.Add("@usuarioAutor", SqlDbType.NVarChar, 100).Value = usuarioAutor;
            });
        }

        public int ActualizarPublicacion(int idPublicacion, string titulo, string contenido, string tipoPublicacion)
        {
            return EjecutarRegistro("ActualizarPublicacion", command =>
            {
                command.Parameters.Add("@idPublicacion", SqlDbType.Int).Value = idPublicacion;
                command.Parameters.Add("@titulo", SqlDbType.NVarChar, 120).Value = titulo;
                command.Parameters.Add("@contenido", SqlDbType.NVarChar, -1).Value = contenido;
                command.Parameters.Add("@tipoPublicacion", SqlDbType.NVarChar, 50).Value = tipoPublicacion;
            });
        }

        public int RegistrarInsignia(int idSocio, string nombre, string motivo)
        {
            return AsignarInsigniaSocio(idSocio, nombre, 1, motivo);
        }

        public int AsignarInsigniaSocio(int idSocio, string nombre, int nivel, string motivo)
        {
            return EjecutarRegistro("AsignarInsigniaSocio", command =>
            {
                command.Parameters.Add("@idSocio", SqlDbType.Int).Value = idSocio;
                command.Parameters.Add("@nombre", SqlDbType.NVarChar, 100).Value = nombre;
                command.Parameters.Add("@nivel", SqlDbType.Int).Value = nivel;
                command.Parameters.Add("@motivo", SqlDbType.NVarChar, 300).Value = string.IsNullOrWhiteSpace(motivo) ? (object)DBNull.Value : motivo;
            });
        }

        public int GuardarInsigniaCatalogo(string nombre, string descripcion, string imagen, string tieneNiveles, string requisitoNiveles)
        {
            return EjecutarRegistro("GuardarInsigniaCatalogo", command =>
            {
                command.Parameters.Add("@nombre", SqlDbType.NVarChar, 100).Value = nombre;
                command.Parameters.Add("@descripcion", SqlDbType.NVarChar, 500).Value = descripcion;
                command.Parameters.Add("@imagen", SqlDbType.NVarChar, 200).Value = string.IsNullOrWhiteSpace(imagen) ? (object)DBNull.Value : imagen;
                command.Parameters.Add("@tieneNiveles", SqlDbType.Char, 1).Value = string.IsNullOrWhiteSpace(tieneNiveles) ? "S" : tieneNiveles.Substring(0, 1).ToUpper();
                command.Parameters.Add("@requisitoNiveles", SqlDbType.NVarChar, 500).Value = string.IsNullOrWhiteSpace(requisitoNiveles) ? (object)DBNull.Value : requisitoNiveles;
            });
        }

        public int ActualizarInsigniaCatalogo(int idInsigniaCatalogo, string nombre, string descripcion, string imagen, string tieneNiveles, string requisitoNiveles)
        {
            return EjecutarRegistro("ActualizarInsigniaCatalogo", command =>
            {
                command.Parameters.Add("@idInsigniaCatalogo", SqlDbType.Int).Value = idInsigniaCatalogo;
                command.Parameters.Add("@nombre", SqlDbType.NVarChar, 100).Value = nombre;
                command.Parameters.Add("@descripcion", SqlDbType.NVarChar, 500).Value = descripcion;
                command.Parameters.Add("@imagen", SqlDbType.NVarChar, 200).Value = string.IsNullOrWhiteSpace(imagen) ? (object)DBNull.Value : imagen;
                command.Parameters.Add("@tieneNiveles", SqlDbType.Char, 1).Value = string.IsNullOrWhiteSpace(tieneNiveles) ? "S" : tieneNiveles.Substring(0, 1).ToUpper();
                command.Parameters.Add("@requisitoNiveles", SqlDbType.NVarChar, 500).Value = string.IsNullOrWhiteSpace(requisitoNiveles) ? (object)DBNull.Value : requisitoNiveles;
            });
        }

        public int RegistrarInventario(string nombre, int cantidad, string ubicacion, string estado)
        {
            return EjecutarRegistro("RegistrarInventario", command =>
            {
                command.Parameters.Add("@nombre", SqlDbType.NVarChar, 120).Value = nombre;
                command.Parameters.Add("@cantidad", SqlDbType.Int).Value = cantidad;
                command.Parameters.Add("@ubicacion", SqlDbType.NVarChar, 120).Value = ubicacion;
                command.Parameters.Add("@estado", SqlDbType.NVarChar, 50).Value = estado;
            });
        }

        public int ActualizarStockInventario(int idInventario, int cantidad, string operacion)
        {
            return EjecutarRegistro("ActualizarStockInventario", command =>
            {
                command.Parameters.Add("@idInventario", SqlDbType.Int).Value = idInventario;
                command.Parameters.Add("@cantidad", SqlDbType.Int).Value = cantidad;
                command.Parameters.Add("@operacion", SqlDbType.NVarChar, 10).Value = operacion;
            });
        }

        public int RegistrarVenta(DateTime fechaVenta, string tipoVenta, string descripcion, decimal importe)
        {
            return EjecutarRegistro("RegistrarVenta", command =>
            {
                command.Parameters.Add("@fechaVenta", SqlDbType.DateTime).Value = fechaVenta;
                command.Parameters.Add("@tipoVenta", SqlDbType.NVarChar, 30).Value = tipoVenta;
                command.Parameters.Add("@descripcion", SqlDbType.NVarChar, 160).Value = descripcion;
                AgregarDecimal(command, "@importe", importe);
            });
        }

        public int RegistrarConvocatoria(int idEvento, int idJugador, string estadoRespuesta)
        {
            return EjecutarRegistro("RegistrarConvocatoriaEvento", command =>
            {
                command.Parameters.Add("@idEvento", SqlDbType.Int).Value = idEvento;
                command.Parameters.Add("@idJugador", SqlDbType.Int).Value = idJugador;
                command.Parameters.Add("@estadoRespuesta", SqlDbType.NVarChar, 30).Value = estadoRespuesta;
            });
        }

        public int RegistrarResultadoPartido(int idEvento, string equipoLocal, string equipoVisitante, string resultadoPartido)
        {
            return EjecutarRegistro("RegistrarResultadoPartido", command =>
            {
                command.Parameters.Add("@idEvento", SqlDbType.Int).Value = idEvento;
                command.Parameters.Add("@equipoLocal", SqlDbType.NVarChar, 120).Value = equipoLocal;
                command.Parameters.Add("@equipoVisitante", SqlDbType.NVarChar, 120).Value = equipoVisitante;
                command.Parameters.Add("@resultado", SqlDbType.NVarChar, 30).Value = resultadoPartido;
            });
        }

        public DataTable ConsultarAsistenciasSocio(int idSocio)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand("ConsultarAsistenciasSocio", connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@idSocio", SqlDbType.Int).Value = idSocio;
                DataTable tabla = new DataTable();
                adapter.Fill(tabla);
                return tabla;
            }
        }

        public DataTable ConsultarConfiguracionCuotas()
        {
            return Consultar("ConsultarConfiguracionCuotas");
        }

        public DataTable ObtenerCuotaSocio(int idSocio)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand("ObtenerCuotaSocio", connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@idSocio", SqlDbType.Int).Value = idSocio;
                DataTable tabla = new DataTable();
                adapter.Fill(tabla);
                return tabla;
            }
        }

        public int RegistrarAsistenciaEventoSocio(int idSocio, int idEvento, string tipoAsistencia)
        {
            return EjecutarRegistro("RegistrarAsistenciaEventoSocio", command =>
            {
                command.Parameters.Add("@idSocio", SqlDbType.Int).Value = idSocio;
                command.Parameters.Add("@idEvento", SqlDbType.Int).Value = idEvento;
                command.Parameters.Add("@tipoAsistencia", SqlDbType.NVarChar, 20).Value = tipoAsistencia;
            });
        }

        private void AgregarDecimal(SqlCommand command, string nombreParametro, decimal valor)
        {
            SqlParameter parametro = command.Parameters.Add(nombreParametro, SqlDbType.Decimal);
            parametro.Precision = 18;
            parametro.Scale = 2;
            parametro.Value = valor;
        }

        private int EjecutarRegistro(string procedimiento, Action<SqlCommand> cargarParametros)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(procedimiento, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                cargarParametros(command);
                connection.Open();
                return command.ExecuteNonQuery();
            }
        }
    }
}
