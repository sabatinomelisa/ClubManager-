using System;
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
        }

        public void IniciarTx()
        {
            tx = conexion.BeginTransaction();
        }

        public void ConfirmarTx()
        {
            tx.Commit();
        }

        public void CancelarTx()
        {
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

        public SqlParameter CrearParametro(string nombre, string valor)
        {
            SqlParameter parametro = new SqlParameter(nombre, valor);

            parametro.DbType = DbType.String;
            return parametro;

        }

        public SqlParameter CrearParametro(string nombre, int valor)
        {
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


    }
}
