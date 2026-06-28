using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BE;
using SERVICIOS;

namespace DAL
{
    public class SocioDAL
    {
        private readonly string connectionString;

        public SocioDAL()
        {
            connectionString = ConnectionStringProvider.Instancia.ObtenerConnectionString();
        }

        public int AltaSocio(UsuarioBE usuario, Acceso acceso)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();

            int idSocio = acceso.DevolverEscalar("IdMaximo", parametros);
            usuario.IdSocio = idSocio;

            parametros.Clear();
            parametros.Add(acceso.CrearParametro("@idSocio", idSocio));
            parametros.Add(acceso.CrearParametro("@tipDoc", usuario.TipoDocumento));
            parametros.Add(acceso.CrearParametro("@nroDoc", usuario.NumeroDocumento));
            parametros.Add(acceso.CrearParametro("@nombre", usuario.Nombre));
            parametros.Add(acceso.CrearParametro("@apellido", usuario.Apellido));
            parametros.Add(acceso.CrearParametro("@fecNac", usuario.FechaNacimiento));
            parametros.Add(acceso.CrearParametro("@nacionalidad", usuario.Nacionalidad));
            parametros.Add(acceso.CrearParametro("@mail", usuario.Mail));
            parametros.Add(acceso.CrearParametro("@telefono", usuario.Telefono));

            return acceso.Escribir("RegistrarSocio", parametros);
        }

        public int AltaSocio(SocioBE socio)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand("RegistrarSocio", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@idSocio", SqlDbType.Int).Value = ObtenerSiguienteIdSocio();
                command.Parameters.Add("@tipDoc", SqlDbType.VarChar, 10).Value = socio.TipoDocumento;
                command.Parameters.Add("@nroDoc", SqlDbType.Int).Value = socio.NumeroDocumento;
                command.Parameters.Add("@nombre", SqlDbType.VarChar, 50).Value = socio.Nombre;
                command.Parameters.Add("@apellido", SqlDbType.VarChar, 50).Value = socio.Apellido;
                command.Parameters.Add("@fecNac", SqlDbType.DateTime).Value = socio.FechaNacimiento;
                command.Parameters.Add("@nacionalidad", SqlDbType.VarChar, 50).Value = socio.Nacionalidad;
                command.Parameters.Add("@mail", SqlDbType.VarChar, 100).Value = socio.Mail;
                command.Parameters.Add("@telefono", SqlDbType.Int).Value = socio.Telefono;

                connection.Open();
                int filasAfectadas = command.ExecuteNonQuery();
                socio.IdSocio = Convert.ToInt32(command.Parameters["@idSocio"].Value);
                return filasAfectadas;
            }
        }

        public List<SocioBE> ListarSocios(bool incluirInactivos)
        {
            List<SocioBE> socios = new List<SocioBE>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand("ConsultarSocios", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@incluirInactivos", SqlDbType.Bit).Value = incluirInactivos;
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        socios.Add(MapearSocio(reader));
                    }
                }
            }

            return socios;
        }

        public SocioBE ObtenerSocio(int idSocio)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand("ObtenerSocio", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@idSocio", SqlDbType.Int).Value = idSocio;
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapearSocio(reader);
                    }
                }
            }

            return null;
        }

        public int ModificarSocio(SocioBE socio)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand("ModificarSocio", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@idSocio", SqlDbType.Int).Value = socio.IdSocio;
                command.Parameters.Add("@tipDoc", SqlDbType.VarChar, 10).Value = socio.TipoDocumento;
                command.Parameters.Add("@nroDoc", SqlDbType.Int).Value = socio.NumeroDocumento;
                command.Parameters.Add("@nombre", SqlDbType.VarChar, 50).Value = socio.Nombre;
                command.Parameters.Add("@apellido", SqlDbType.VarChar, 50).Value = socio.Apellido;
                command.Parameters.Add("@fecNac", SqlDbType.DateTime).Value = socio.FechaNacimiento;
                command.Parameters.Add("@nacionalidad", SqlDbType.VarChar, 50).Value = socio.Nacionalidad;
                command.Parameters.Add("@mail", SqlDbType.VarChar, 100).Value = socio.Mail;
                command.Parameters.Add("@telefono", SqlDbType.Int).Value = socio.Telefono;
                command.Parameters.Add("@activo", SqlDbType.Char, 1).Value = string.IsNullOrWhiteSpace(socio.Activo) ? "S" : socio.Activo;

                connection.Open();
                return command.ExecuteNonQuery();
            }
        }

        public int CambiarEstadoSocio(int idSocio, string activo)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand("CambiarEstadoSocio", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@idSocio", SqlDbType.Int).Value = idSocio;
                command.Parameters.Add("@activo", SqlDbType.Char, 1).Value = activo;

                connection.Open();
                return command.ExecuteNonQuery();
            }
        }


        public int ActualizarMailSocio(int idSocio, string mail)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand("ActualizarMailSocio", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@idSocio", SqlDbType.Int).Value = idSocio;
                command.Parameters.Add("@mail", SqlDbType.VarChar, 100).Value = mail;

                connection.Open();
                return command.ExecuteNonQuery();
            }
        }

        public void ActualizarDigitoVerificadorHorizontal(int idSocio, int digitoVerificadorHorizontal)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand("ActualizarDVHSocio", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@idSocio", SqlDbType.Int).Value = idSocio;
                command.Parameters.Add("@dvh", SqlDbType.Int).Value = digitoVerificadorHorizontal;

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        private int ObtenerSiguienteIdSocio()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand("IdMaximo", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        private SocioBE MapearSocio(SqlDataReader reader)
        {
            SocioBE socio = new SocioBE();
            socio.IdSocio = Convert.ToInt32(reader["IdSocio"]);
            socio.TipoDocumento = reader["TipoDocumento"].ToString();
            socio.NumeroDocumento = Convert.ToInt32(reader["NumeroDocumento"]);
            socio.Nombre = reader["Nombre"].ToString();
            socio.Apellido = reader["Apellido"].ToString();
            socio.FechaNacimiento = Convert.ToDateTime(reader["FechaNacimiento"]);
            socio.Nacionalidad = reader["Nacionalidad"].ToString();
            socio.Mail = reader["Email"].ToString();
            socio.Telefono = Convert.ToInt32(reader["Telefono"]);
            socio.Activo = reader["Activo"].ToString();
            socio.DigitoVerificadorHorizontal = Convert.ToInt32(reader["DigitoVerificadorHorizontal"]);
            return socio;
        }
    }
}
