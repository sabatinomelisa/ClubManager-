using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BE;
using SERVICIOS;

namespace DAL
{
    public class IntegridadDAL
    {
        private readonly string connectionString;
        private readonly SocioDAL socioDAL;

        public IntegridadDAL()
        {
            connectionString = ConnectionStringProvider.Instancia.ObtenerConnectionString();
            socioDAL = new SocioDAL();
        }

        public List<SocioBE> ListarSociosParaIntegridad()
        {
            return socioDAL.ListarSocios(true);
        }

        public void ActualizarDigitoHorizontalSocio(int idSocio, int digitoVerificadorHorizontal)
        {
            socioDAL.ActualizarDigitoVerificadorHorizontal(idSocio, digitoVerificadorHorizontal);
        }

        public Dictionary<string, int> ObtenerDigitosVerticales(string entidad)
        {
            Dictionary<string, int> digitos = new Dictionary<string, int>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand("ConsultarDigitosVerticales", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@entidad", SqlDbType.NVarChar, 100).Value = entidad;
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        digitos[reader["Campo"].ToString()] = Convert.ToInt32(reader["Valor"]);
                    }
                }
            }

            return digitos;
        }

        public void GuardarDigitosVerticales(string entidad, Dictionary<string, int> digitos)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                foreach (KeyValuePair<string, int> digito in digitos)
                {
                    using (SqlCommand command = new SqlCommand("GuardarDigitoVertical", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@entidad", SqlDbType.NVarChar, 100).Value = entidad;
                        command.Parameters.Add("@campo", SqlDbType.NVarChar, 100).Value = digito.Key;
                        command.Parameters.Add("@valor", SqlDbType.Int).Value = digito.Value;
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public bool ExistenDigitosVerticales(string entidad)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand("ExisteDigitoVertical", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@entidad", SqlDbType.NVarChar, 100).Value = entidad;
                connection.Open();
                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
        }
    }
}
