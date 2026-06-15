using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class TraduccionDAL
    {
        public List<TraduccionBE> ListarTraducciones(int idiomaSel)
        {
            Acceso acceso = new Acceso();

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
    }
}
