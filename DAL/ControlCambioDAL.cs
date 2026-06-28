using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BE;
using SERVICIOS;

namespace DAL
{
    public class ControlCambioDAL
    {
        private readonly string connectionString;

        public ControlCambioDAL()
        {
            connectionString = ConnectionStringProvider.Instancia.ObtenerConnectionString();
        }

        public void RegistrarMailHistorico(HistorialBE historial)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand("RegistrarHistorialMailSocio", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@idSocio", SqlDbType.Int).Value = historial.IdSocio;
                command.Parameters.Add("@mail", SqlDbType.VarChar, 100).Value = historial.Mail;

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public List<HistorialBE> ListarHistorialMailSocio(int? idSocio)
        {
            List<HistorialBE> historial = new List<HistorialBE>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand("ConsultarHistorialMailSocio", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@idSocio", SqlDbType.Int).Value = idSocio.HasValue ? (object)idSocio.Value : DBNull.Value;
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        historial.Add(MapearHistorial(reader));
                    }
                }
            }

            return historial;
        }

        public HistorialBE ObtenerHistorialMailSocio(int idSocio, int idHistorico)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand("ObtenerHistorialMailSocio", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@idSocio", SqlDbType.Int).Value = idSocio;
                command.Parameters.Add("@idHistorico", SqlDbType.Int).Value = idHistorico;
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapearHistorial(reader);
                    }
                }
            }

            return null;
        }

        private HistorialBE MapearHistorial(SqlDataReader reader)
        {
            HistorialBE historial = new HistorialBE();
            historial.IdHistorico = Convert.ToInt32(reader["IdHistorico"]);
            historial.IdSocio = Convert.ToInt32(reader["IdSocio"]);
            historial.Mail = reader["Email"].ToString();
            historial.FechaCreacion = Convert.ToDateTime(reader["FechaCreacion"]);
            return historial;
        }
    }
}
