using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
<<<<<<< HEAD
=======
using System.Linq;
using System.Text;
using System.Threading.Tasks;
>>>>>>> origin/main

namespace DAL
{
    public class TraduccionDAL
    {
        public List<TraduccionBE> ListarTraducciones(int idiomaSel)
        {
            Acceso acceso = new Acceso();
<<<<<<< HEAD
            List<TraduccionBE> traducciones = new List<TraduccionBE>();

            acceso.Conectar();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@id", idiomaSel));
            DataTable tabla = acceso.Leer("ConsultaTraducciones", parametros);

            foreach (DataRow row in tabla.Rows)
            {
                TraduccionBE traduccionAuxiliar = new TraduccionBE();
                traduccionAuxiliar.Id = int.Parse(row["Id"].ToString());
                traduccionAuxiliar.NombreControl = row["NombreControl"].ToString();
                traduccionAuxiliar.Traduccion = row["Traduccion"].ToString();
                traducciones.Add(traduccionAuxiliar);
            }

            acceso.Desconectar();
            return traducciones;
        }

        public int GuardarTraduccion(int idIdioma, string nombreControl, string textoTraduccion)
        {
            Acceso acceso = new Acceso();
            acceso.Conectar();

            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>();
                parametros.Add(acceso.CrearParametro("@idIdioma", idIdioma));
                parametros.Add(acceso.CrearParametro("@nombreControl", nombreControl));
                parametros.Add(acceso.CrearParametro("@traduccion", textoTraduccion));
                return acceso.Escribir("GuardarTraduccion", parametros);
            }
            finally
            {
                acceso.Desconectar();
            }
        }
=======

            List<TraduccionBE> traducciones = new List<TraduccionBE>();

            acceso.Conectar();
            string sql = "ConsultaTraducciones";
            List<SqlParameter> parametros = new List<SqlParameter>();
            DataTable tabla = new DataTable();

            parametros.Add(acceso.CrearParametro("@id", idiomaSel));
            tabla= acceso.Leer(sql, parametros);

            foreach (DataRow row in tabla.Rows)
            {
                TraduccionBE tradAux = new TraduccionBE();
                tradAux.Id=int.Parse(row["Id"].ToString());
                tradAux.NombreControl = row["NombreControl"].ToString();
                tradAux.Traduccion = row["Traduccion"].ToString();
                traducciones.Add(tradAux);
            }

            acceso.Desconectar();

            return traducciones;
        }
>>>>>>> origin/main
    }
}
