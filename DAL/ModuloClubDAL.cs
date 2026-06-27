using System;
using System.Data;
using System.Data.SqlClient;

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

        public int RegistrarPago(int idSocio, DateTime fechaPago, string concepto, decimal importe, string estado)
        {
            return EjecutarRegistro("RegistrarPago", command =>
            {
                command.Parameters.Add("@idSocio", SqlDbType.Int).Value = idSocio;
                command.Parameters.Add("@fechaPago", SqlDbType.DateTime).Value = fechaPago;
                command.Parameters.Add("@concepto", SqlDbType.NVarChar, 100).Value = concepto;
                SqlParameter parametroImporte = command.Parameters.Add("@importe", SqlDbType.Decimal);
                parametroImporte.Precision = 18;
                parametroImporte.Scale = 2;
                parametroImporte.Value = importe;
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

        public int RegistrarEvento(string nombre, string deporte, DateTime fechaEvento, string lugar, string estado)
        {
            return EjecutarRegistro("RegistrarEventoDeportivo", command =>
            {
                command.Parameters.Add("@nombre", SqlDbType.NVarChar, 120).Value = nombre;
                command.Parameters.Add("@deporte", SqlDbType.NVarChar, 80).Value = deporte;
                command.Parameters.Add("@fechaEvento", SqlDbType.DateTime).Value = fechaEvento;
                command.Parameters.Add("@lugar", SqlDbType.NVarChar, 120).Value = lugar;
                command.Parameters.Add("@estado", SqlDbType.NVarChar, 30).Value = estado;
            });
        }

        public int RegistrarMovimientoFinanciero(DateTime fechaMovimiento, string tipoMovimiento, string concepto, decimal importe)
        {
            return EjecutarRegistro("RegistrarMovimientoFinanciero", command =>
            {
                command.Parameters.Add("@fechaMovimiento", SqlDbType.DateTime).Value = fechaMovimiento;
                command.Parameters.Add("@tipoMovimiento", SqlDbType.NVarChar, 20).Value = tipoMovimiento;
                command.Parameters.Add("@concepto", SqlDbType.NVarChar, 120).Value = concepto;
                SqlParameter parametroImporte = command.Parameters.Add("@importe", SqlDbType.Decimal);
                parametroImporte.Precision = 18;
                parametroImporte.Scale = 2;
                parametroImporte.Value = importe;
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

        public int RegistrarInsignia(int idSocio, string nombre, string motivo)
        {
            return EjecutarRegistro("RegistrarInsignia", command =>
            {
                command.Parameters.Add("@idSocio", SqlDbType.Int).Value = idSocio;
                command.Parameters.Add("@nombre", SqlDbType.NVarChar, 100).Value = nombre;
                command.Parameters.Add("@motivo", SqlDbType.NVarChar, 300).Value = motivo;
            });
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
