using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class Acceso
    {
        private SqlConnection conexion;
        private SqlTransaction transaccionActual;

        public void Conectar()
        {
            conexion = new SqlConnection(ConnectionStringProvider.Instancia.ObtenerConnectionString());
            conexion.Open();
        }

        public void Desconectar()
        {
            if (conexion != null)
            {
                if (conexion.State != ConnectionState.Closed)
                {
                    conexion.Close();
                }

                conexion.Dispose();
                conexion = null;
            }
        }

        public void IniciarTx()
        {
            ValidarConexionAbierta();
            transaccionActual = conexion.BeginTransaction();
        }

        public void ConfirmarTx()
        {
            if (transaccionActual != null)
            {
                transaccionActual.Commit();
                transaccionActual.Dispose();
                transaccionActual = null;
            }
        }

        public void CancelarTx()
        {
            if (transaccionActual != null)
            {
                transaccionActual.Rollback();
                transaccionActual.Dispose();
                transaccionActual = null;
            }
        }

        private SqlCommand CrearComando(string procedimiento, List<SqlParameter> parametros = null)
        {
            ValidarConexionAbierta();

            SqlCommand comando = new SqlCommand(procedimiento, conexion);
            comando.CommandType = CommandType.StoredProcedure;

            if (transaccionActual != null)
            {
                comando.Transaction = transaccionActual;
            }

            if (parametros != null)
            {
                comando.Parameters.AddRange(parametros.ToArray());
            }

            return comando;
        }

        public DataTable Leer(string procedimiento, List<SqlParameter> parametros = null)
        {
            DataTable tabla = new DataTable();

            using (SqlCommand comando = CrearComando(procedimiento, parametros))
            using (SqlDataAdapter adaptador = new SqlDataAdapter(comando))
            {
                adaptador.Fill(tabla);
            }

            return tabla;
        }

        public int Escribir(string procedimiento, List<SqlParameter> parametros = null)
        {
            using (SqlCommand comando = CrearComando(procedimiento, parametros))
            {
                return comando.ExecuteNonQuery();
            }
        }

        public int DevolverEscalar(string procedimiento, List<SqlParameter> parametros = null)
        {
            using (SqlCommand comando = CrearComando(procedimiento, parametros))
            {
                object resultado = comando.ExecuteScalar();

                if (resultado == null || resultado == DBNull.Value)
                {
                    return 0;
                }

                return Convert.ToInt32(resultado);
            }
        }

        public string DevolverEscalarString(string procedimiento, List<SqlParameter> parametros = null)
        {
            using (SqlCommand comando = CrearComando(procedimiento, parametros))
            {
                object resultado = comando.ExecuteScalar();

                if (resultado == null || resultado == DBNull.Value)
                {
                    return string.Empty;
                }

                return resultado.ToString();
            }
        }

        public SqlParameter CrearParametro(string nombre, string valor)
        {
            SqlParameter parametro = new SqlParameter(nombre, SqlDbType.NVarChar);
            parametro.Value = ObtenerValorParametro(valor);
            return parametro;
        }

        public SqlParameter CrearParametro(string nombre, int valor)
        {
            SqlParameter parametro = new SqlParameter(nombre, SqlDbType.Int);
            parametro.Value = valor;
            return parametro;
        }

        public SqlParameter CrearParametro(string nombre, DateTime valor)
        {
            SqlParameter parametro = new SqlParameter(nombre, SqlDbType.DateTime);
            parametro.Value = valor;
            return parametro;
        }

        private object ObtenerValorParametro(string valor)
        {
            if (valor == null)
            {
                return DBNull.Value;
            }

            return valor;
        }

        private void ValidarConexionAbierta()
        {
            if (conexion == null || conexion.State != ConnectionState.Open)
            {
                throw new InvalidOperationException("La conexión a base de datos no está abierta.");
            }
        }
    }
}
