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
            connectionString = ConnectionStringProvider.Instancia.ObtenerConnectionString();
        }

        public BitacoraDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void Registrar(BitacoraBE bitacora)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
                    INSERT INTO Bitacora (Fecha, Usuario, Accion, Modulo, Descripcion)
                    VALUES (@Fecha, @Usuario, @Accion, @Modulo, @Descripcion)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@Fecha", SqlDbType.DateTime).Value = DateTime.Now;
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
            return Buscar(null, null, null, null, null);
        }

        public List<BitacoraBE> ListarPorUsuario(string usuario)
        {
            return Buscar(usuario, null, null, null, null);
        }

        public List<BitacoraBE> Buscar(string usuario, string accion, string modulo, DateTime? fechaDesde, DateTime? fechaHasta)
        {
            List<BitacoraBE> bitacoras = new List<BitacoraBE>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT IdBitacora, Fecha, Usuario, Accion, Modulo, Descripcion
                    FROM Bitacora
                    WHERE (@Usuario IS NULL OR Usuario = @Usuario)
                      AND (@Accion IS NULL OR Accion = @Accion)
                      AND (@Modulo IS NULL OR Modulo = @Modulo)
                      AND (@FechaDesde IS NULL OR Fecha >= @FechaDesde)
                      AND (@FechaHasta IS NULL OR Fecha <= @FechaHasta)
                    ORDER BY Fecha DESC";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@Usuario", SqlDbType.NVarChar, 100).Value = ObtenerValorNullable(usuario);
                    command.Parameters.Add("@Accion", SqlDbType.NVarChar, 100).Value = ObtenerValorNullable(accion);
                    command.Parameters.Add("@Modulo", SqlDbType.NVarChar, 100).Value = ObtenerValorNullable(modulo);
                    command.Parameters.Add("@FechaDesde", SqlDbType.DateTime).Value = fechaDesde.HasValue ? (object)fechaDesde.Value : DBNull.Value;
                    command.Parameters.Add("@FechaHasta", SqlDbType.DateTime).Value = fechaHasta.HasValue ? (object)fechaHasta.Value : DBNull.Value;

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
