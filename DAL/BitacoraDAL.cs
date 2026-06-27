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
<<<<<<< HEAD
            connectionString = ConnectionStringProvider.Instancia.ObtenerConnectionString();
=======
            // No se modifica App.config para no tocar archivos existentes.
            // Ajustar este valor si tu base no usa LocalDB.
            connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ClubManagerDB;Integrated Security=True";
>>>>>>> origin/main
        }

        public BitacoraDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void Registrar(BitacoraBE bitacora)
        {
<<<<<<< HEAD
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
                    INSERT INTO Bitacora (Fecha, Usuario, Accion, Modulo, Descripcion)
                    VALUES (@Fecha, @Usuario, @Accion, @Modulo, @Descripcion)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@Fecha", SqlDbType.DateTime).Value = DateTime.Now;
=======
            CrearTablaSiNoExiste();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
                    INSERT INTO Bitacora (Usuario, Accion, Modulo, Descripcion)
                    VALUES (@Usuario, @Accion, @Modulo, @Descripcion)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
>>>>>>> origin/main
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
<<<<<<< HEAD
            return Buscar(null, null, null, null, null);
        }

        public List<BitacoraBE> ListarPorUsuario(string usuario)
        {
            return Buscar(usuario, null, null, null, null);
        }

        public List<BitacoraBE> Buscar(string usuario, string accion, string modulo, DateTime? fechaDesde, DateTime? fechaHasta)
        {
=======
            CrearTablaSiNoExiste();

>>>>>>> origin/main
            List<BitacoraBE> bitacoras = new List<BitacoraBE>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT IdBitacora, Fecha, Usuario, Accion, Modulo, Descripcion
                    FROM Bitacora
<<<<<<< HEAD
                    WHERE (@Usuario IS NULL OR Usuario = @Usuario)
                      AND (@Accion IS NULL OR Accion = @Accion)
                      AND (@Modulo IS NULL OR Modulo = @Modulo)
                      AND (@FechaDesde IS NULL OR Fecha >= @FechaDesde)
                      AND (@FechaHasta IS NULL OR Fecha <= @FechaHasta)
=======
>>>>>>> origin/main
                    ORDER BY Fecha DESC";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
<<<<<<< HEAD
                    command.Parameters.Add("@Usuario", SqlDbType.NVarChar, 100).Value = ObtenerValorNullable(usuario);
                    command.Parameters.Add("@Accion", SqlDbType.NVarChar, 100).Value = ObtenerValorNullable(accion);
                    command.Parameters.Add("@Modulo", SqlDbType.NVarChar, 100).Value = ObtenerValorNullable(modulo);
                    command.Parameters.Add("@FechaDesde", SqlDbType.DateTime).Value = fechaDesde.HasValue ? (object)fechaDesde.Value : DBNull.Value;
                    command.Parameters.Add("@FechaHasta", SqlDbType.DateTime).Value = fechaHasta.HasValue ? (object)fechaHasta.Value : DBNull.Value;

=======
>>>>>>> origin/main
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

<<<<<<< HEAD
=======
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

>>>>>>> origin/main
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
