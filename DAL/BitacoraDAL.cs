using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BE;

namespace DAL
{
    public class BitacoraDAL
    {
        private readonly string connectionString;

        public BitacoraDAL()
        {
            // No se modifica App.config para no tocar archivos existentes.
            // Ajustar este valor si tu base no usa LocalDB.
            connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ClubManagerDB;Integrated Security=True";
        }

        public BitacoraDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void Registrar(BitacoraBE bitacora)
        {
            CrearTablaSiNoExiste();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
                    INSERT INTO Bitacora (Usuario, Accion, Modulo, Descripcion)
                    VALUES (@Usuario, @Accion, @Modulo, @Descripcion)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@Usuario", SqlDbType.NVarChar, 100).Value = ObtenerValorNullable(bitacora.Usuario);
                    command.Parameters.Add("@Accion", SqlDbType.NVarChar, 100).Value = bitacora.Accion;
                    command.Parameters.Add("@Modulo", SqlDbType.NVarChar, 100).Value = bitacora.Modulo;
                    command.Parameters.Add("@Descripcion", SqlDbType.NVarChar, 500).Value = ObtenerValorNullable(bitacora.Descripcion);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<BitacoraBE> Listar()
        {
            CrearTablaSiNoExiste();

            List<BitacoraBE> bitacoras = new List<BitacoraBE>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT IdBitacora, Fecha, Usuario, Accion, Modulo, Descripcion
                    FROM Bitacora
                    ORDER BY Fecha DESC";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            BitacoraBE bitacora = new BitacoraBE();
                            bitacora.IdBitacora = Convert.ToInt32(reader["IdBitacora"]);
                            bitacora.Fecha = Convert.ToDateTime(reader["Fecha"]);
                            bitacora.Usuario = ObtenerTexto(reader["Usuario"]);
                            bitacora.Accion = ObtenerTexto(reader["Accion"]);
                            bitacora.Modulo = ObtenerTexto(reader["Modulo"]);
                            bitacora.Descripcion = ObtenerTexto(reader["Descripcion"]);

                            bitacoras.Add(bitacora);
                        }
                    }
                }
            }

            return bitacoras;
        }

        public List<BitacoraBE> ListarPorUsuario(string usuario)
        {
            CrearTablaSiNoExiste();

            List<BitacoraBE> bitacoras = new List<BitacoraBE>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT IdBitacora, Fecha, Usuario, Accion, Modulo, Descripcion
                    FROM Bitacora
                    WHERE Usuario = @Usuario
                    ORDER BY Fecha DESC";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@Usuario", SqlDbType.NVarChar, 100).Value = usuario;
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            BitacoraBE bitacora = new BitacoraBE();
                            bitacora.IdBitacora = Convert.ToInt32(reader["IdBitacora"]);
                            bitacora.Fecha = Convert.ToDateTime(reader["Fecha"]);
                            bitacora.Usuario = ObtenerTexto(reader["Usuario"]);
                            bitacora.Accion = ObtenerTexto(reader["Accion"]);
                            bitacora.Modulo = ObtenerTexto(reader["Modulo"]);
                            bitacora.Descripcion = ObtenerTexto(reader["Descripcion"]);

                            bitacoras.Add(bitacora);
                        }
                    }
                }
            }

            return bitacoras;
        }

        private void CrearTablaSiNoExiste()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
                    IF NOT EXISTS (
                        SELECT 1
                        FROM sys.tables
                        WHERE name = 'Bitacora'
                    )
                    BEGIN
                        CREATE TABLE Bitacora (
                            IdBitacora INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
                            Fecha DATETIME NOT NULL DEFAULT GETDATE(),
                            Usuario NVARCHAR(100) NULL,
                            Accion NVARCHAR(100) NOT NULL,
                            Modulo NVARCHAR(100) NOT NULL,
                            Descripcion NVARCHAR(500) NULL
                        )
                    END";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        private object ObtenerValorNullable(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
            {
                return DBNull.Value;
            }

            return valor;
        }

        private string ObtenerTexto(object valor)
        {
            if (valor == DBNull.Value || valor == null)
            {
                return string.Empty;
            }

            return valor.ToString();
        }
    }
}
