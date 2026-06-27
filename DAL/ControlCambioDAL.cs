using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BE;

namespace DAL
{
    public class ControlCambioDAL
    {
        private readonly string connectionString;

        public ControlCambioDAL()
        {
            connectionString = ConnectionStringProvider.Instancia.ObtenerConnectionString();
        }

        public void RegistrarCambioSocio(CambioSocioBE cambioSocio)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand("RegistrarCambioSocio", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@idSocio", SqlDbType.Int).Value = cambioSocio.IdSocio;
                command.Parameters.Add("@usuario", SqlDbType.NVarChar, 100).Value = ObtenerValor(cambioSocio.Usuario);
                command.Parameters.Add("@accion", SqlDbType.NVarChar, 50).Value = cambioSocio.Accion;
                command.Parameters.Add("@estadoAnterior", SqlDbType.NVarChar, -1).Value = ObtenerValor(cambioSocio.EstadoAnterior);
                command.Parameters.Add("@estadoNuevo", SqlDbType.NVarChar, -1).Value = ObtenerValor(cambioSocio.EstadoNuevo);
                command.Parameters.Add("@descripcion", SqlDbType.NVarChar, 500).Value = ObtenerValor(cambioSocio.Descripcion);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public List<CambioSocioBE> ListarCambiosSocio(int? idSocio)
        {
            List<CambioSocioBE> cambios = new List<CambioSocioBE>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand("ConsultarCambiosSocio", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@idSocio", SqlDbType.Int).Value = idSocio.HasValue ? (object)idSocio.Value : DBNull.Value;
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cambios.Add(MapearCambio(reader));
                    }
                }
            }

            return cambios;
        }

        public CambioSocioBE ObtenerCambioSocio(int idCambioSocio)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand("ObtenerCambioSocio", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@idCambioSocio", SqlDbType.Int).Value = idCambioSocio;
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapearCambio(reader);
                    }
                }
            }

            return null;
        }

        private CambioSocioBE MapearCambio(SqlDataReader reader)
        {
            CambioSocioBE cambioSocio = new CambioSocioBE();
            cambioSocio.IdCambioSocio = Convert.ToInt32(reader["IdCambioSocio"]);
            cambioSocio.IdSocio = Convert.ToInt32(reader["IdSocio"]);
            cambioSocio.Usuario = reader["Usuario"].ToString();
            cambioSocio.FechaCambio = Convert.ToDateTime(reader["FechaCambio"]);
            cambioSocio.Accion = reader["Accion"].ToString();
            cambioSocio.EstadoAnterior = reader["EstadoAnterior"].ToString();
            cambioSocio.EstadoNuevo = reader["EstadoNuevo"].ToString();
            cambioSocio.Descripcion = reader["Descripcion"].ToString();
            return cambioSocio;
        }

        private object ObtenerValor(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
            {
                return DBNull.Value;
            }

            return valor;
        }
    }
}
