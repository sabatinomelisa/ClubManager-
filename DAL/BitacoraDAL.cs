using BE;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class BitacoraDAL
    {
        private readonly string connectionString;

        public BitacoraDAL()
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["ClubManagerDB"];

            if (settings == null || string.IsNullOrWhiteSpace(settings.ConnectionString))
            {
                throw new ConfigurationErrorsException("No se encontró la cadena de conexión 'ClubManagerDB' en App.config.");
            }

            connectionString = settings.ConnectionString;
        }

        public void Registrar(BitacoraBE bitacora)
        {
            if (bitacora == null)
            {
                throw new ArgumentNullException("bitacora");
            }

            CrearTablaSiNoExiste();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                const string query = @"
                    INSERT INTO dbo.Bitacora (Usuario, Accion, Modulo, Descripcion)
                    VALUES (@Usuario, @Accion, @Modulo, @Descripcion);";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@Usuario", SqlDbType.NVarChar, 100).Value =
                        string.IsNullOrWhiteSpace(bitacora.Usuario) ? (object)DBNull.Value : bitacora.Usuario;
                    command.Parameters.Add("@Accion", SqlDbType.NVarChar, 100).Value = bitacora.Accion;
                    command.Parameters.Add("@Modulo", SqlDbType.NVarChar, 100).Value = bitacora.Modulo;
                    command.Parameters.Add("@Descripcion", SqlDbType.NVarChar, 500).Value =
                        string.IsNullOrWhiteSpace(bitacora.Descripcion) ? (object)DBNull.Value : bitacora.Descripcion;

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<BitacoraBE> Listar()
        {
            CrearTablaSiNoExiste();

            List<BitacoraBE> lista = new List<BitacoraBE>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                const string query = @"
                    SELECT IdBitacora, Fecha, Usuario, Accion, Modulo, Descripcion
                    FROM dbo.Bitacora
                    ORDER BY Fecha DESC;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new BitacoraBE
                            {
                                IdBitacora = Convert.ToInt32(reader["IdBitacora"]),
                                Fecha = Convert.ToDateTime(reader["Fecha"]),
                                Usuario = reader["Usuario"] == DBNull.Value ? null : reader["Usuario"].ToString(),
                                Accion = reader["Accion"].ToString(),
                                Modulo = reader["Modulo"].ToString(),
                                Descripcion = reader["Descripcion"] == DBNull.Value ? null : reader["Descripcion"].ToString()
                            });
                        }
                    }
                }
            }

            return lista;
        }

        private void CrearTablaSiNoExiste()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                const string query = @"
                    IF OBJECT_ID('dbo.Bitacora', 'U') IS NULL
                    BEGIN
                        CREATE TABLE dbo.Bitacora
                        (
                            IdBitacora INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
                            Fecha DATETIME NOT NULL DEFAULT GETDATE(),
                            Usuario NVARCHAR(100) NULL,
                            Accion NVARCHAR(100) NOT NULL,
                            Modulo NVARCHAR(100) NOT NULL,
                            Descripcion NVARCHAR(500) NULL
                        );
                    END;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
