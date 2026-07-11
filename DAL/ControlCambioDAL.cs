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

        public void RestaurarMailHistorico(int idSocio, string mailActual, string mailHistorico)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand actualizarMail = new SqlCommand("ActualizarMailSocio", connection, transaction))
                        {
                            actualizarMail.CommandType = CommandType.StoredProcedure;
                            actualizarMail.Parameters.Add("@idSocio", SqlDbType.Int).Value = idSocio;
                            actualizarMail.Parameters.Add("@mail", SqlDbType.VarChar, 100).Value = mailHistorico.Trim();

                            int filasAfectadas = actualizarMail.ExecuteNonQuery();
                            if (filasAfectadas <= 0)
                            {
                                throw new Exception("No se pudo restaurar el mail histórico seleccionado.");
                            }
                        }

                        string ultimoMailHistorico = null;
                        using (SqlCommand obtenerUltimoMail = new SqlCommand(@"
                            SELECT TOP 1 Email
                            FROM dbo.HistorialSocio
                            WHERE IdSocio = @idSocio
                            ORDER BY FechaCreacion DESC, IdHistorico DESC", connection, transaction))
                        {
                            obtenerUltimoMail.Parameters.Add("@idSocio", SqlDbType.Int).Value = idSocio;
                            object valor = obtenerUltimoMail.ExecuteScalar();
                            ultimoMailHistorico = valor == null || valor == DBNull.Value ? null : valor.ToString();
                        }

                        bool debeGuardarMailActual = string.IsNullOrWhiteSpace(ultimoMailHistorico)
                            || !string.Equals(ultimoMailHistorico.Trim(), mailActual.Trim(), StringComparison.OrdinalIgnoreCase);

                        if (debeGuardarMailActual)
                        {
                            int idHistorico;
                            using (SqlCommand obtenerSiguienteId = new SqlCommand(@"
                                SELECT ISNULL(MAX(IdHistorico), 0) + 1
                                FROM dbo.HistorialSocio
                                WHERE IdSocio = @idSocio", connection, transaction))
                            {
                                obtenerSiguienteId.Parameters.Add("@idSocio", SqlDbType.Int).Value = idSocio;
                                idHistorico = Convert.ToInt32(obtenerSiguienteId.ExecuteScalar());
                            }

                            using (SqlCommand insertarHistorial = new SqlCommand(@"
                                INSERT INTO dbo.HistorialSocio (IdHistorico, IdSocio, Email, FechaCreacion)
                                VALUES (@idHistorico, @idSocio, @mail, GETDATE())", connection, transaction))
                            {
                                insertarHistorial.Parameters.Add("@idHistorico", SqlDbType.Int).Value = idHistorico;
                                insertarHistorial.Parameters.Add("@idSocio", SqlDbType.Int).Value = idSocio;
                                insertarHistorial.Parameters.Add("@mail", SqlDbType.VarChar, 100).Value = mailActual.Trim();
                                insertarHistorial.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
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
