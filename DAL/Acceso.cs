<<<<<<< HEAD
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
=======
﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public  class Acceso
    {
        SqlConnection conexion;
        SqlTransaction tx;
        public void Conectar()
        {
            conexion = new SqlConnection();
            conexion.ConnectionString = "Data Source=DESKTOP-BJDMH9N\\SQLEXPRESS;" + "Initial Catalog=Club Manager;" + "Integrated Security=True;" + "TrustServerCertificate=True;";
            conexion.Open();
        }


        public void Desconectar()
        {
            conexion.Close();
            conexion = null;
            GC.Collect();
>>>>>>> origin/main
        }

        public void IniciarTx()
        {
<<<<<<< HEAD
            ValidarConexionAbierta();
            transaccionActual = conexion.BeginTransaction();
=======
            tx = conexion.BeginTransaction();
>>>>>>> origin/main
        }

        public void ConfirmarTx()
        {
<<<<<<< HEAD
            if (transaccionActual != null)
            {
                transaccionActual.Commit();
                transaccionActual.Dispose();
                transaccionActual = null;
            }
=======
            tx.Commit();
>>>>>>> origin/main
        }

        public void CancelarTx()
        {
<<<<<<< HEAD
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
=======
            tx.Rollback();
        }

        private SqlCommand CrearComando(string sql, List<SqlParameter> parameters = null)
        {
            SqlCommand cmd = new SqlCommand(sql, conexion);
            cmd.CommandType = CommandType.StoredProcedure;

            if (tx != null)
            {
                cmd.Transaction = tx;
            }

            if (parameters != null)
            {
                cmd.Parameters.AddRange(parameters.ToArray());
            }

            return cmd;
        }

        public DataTable Leer(string sql, List<SqlParameter> parameters = null)
        {
            DataTable tabla = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();

            da.SelectCommand = CrearComando(sql, parameters);
            da.Fill(tabla);
            da.Dispose();

            return tabla;

        }

        public int Escribir(string sql, List<SqlParameter> parameters = null)
        {
            SqlCommand comando = CrearComando(sql, parameters);

            int registrosAfectados = 0;

            try
            {
                registrosAfectados = comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                registrosAfectados = -1;
            }

            comando.Parameters.Clear();
            comando.Dispose();
            return registrosAfectados;
        }

        public int DevolverEscalar(string sql, List<SqlParameter> parameters = null)
        {
            SqlCommand comando = CrearComando(sql, parameters);

            return int.Parse(comando.ExecuteScalar().ToString());
        }

        public string DevolverEscalarString(string sql, List<SqlParameter> parameters = null)
        {
            SqlCommand comando = CrearComando(sql, parameters);

            object resultado = comando.ExecuteScalar();

            if (resultado == null)
            {
                return string.Empty;
            }

            return resultado.ToString();

>>>>>>> origin/main
        }

        public SqlParameter CrearParametro(string nombre, string valor)
        {
<<<<<<< HEAD
            SqlParameter parametro = new SqlParameter(nombre, SqlDbType.NVarChar);
            parametro.Value = ObtenerValorParametro(valor);
            return parametro;
=======
            SqlParameter parametro = new SqlParameter(nombre, valor);

            parametro.DbType = DbType.String;
            return parametro;

>>>>>>> origin/main
        }

        public SqlParameter CrearParametro(string nombre, int valor)
        {
<<<<<<< HEAD
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
=======
            SqlParameter parametro = new SqlParameter(nombre, valor);

            parametro.DbType = DbType.Int32;
            return parametro;
        }
        public SqlParameter CrearParametro(string nombre, DateTime valor)
        {
            SqlParameter parametro = new SqlParameter(nombre, valor);

            parametro.DbType = DbType.DateTime;
            return parametro;
        }


>>>>>>> origin/main
    }
}
