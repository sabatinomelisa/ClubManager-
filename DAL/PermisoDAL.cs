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
    public class PermisoDAL
    {
        public List<PermisoBE> ObtenerPermisos(RolBE rol, Acceso acceso)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();
            //Busco el proximo ID del socio para insertar
            string sql = "ConsultarPermisos";
            parametros.Clear();

            parametros.Clear();
            parametros.Add(acceso.CrearParametro("@idRol", rol.Id));

            List<PermisoBE> permisos = new List<PermisoBE>();

            DataTable resultado = new DataTable();

            resultado = acceso.Leer(sql, parametros);

            foreach (PermisoBE row in resultado.Rows)
            {
                permisos.Add(row);
            }

            return permisos;
        }
    }
}
