using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BE;

namespace DAL
{
    public class PermisoDAL
    {
        public List<PermisoBE> ObtenerPermisos(RolBE rol, Acceso acceso)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@idRol", rol.Id));

            DataTable resultado = acceso.Leer("ConsultarPermisos", parametros);
            List<PermisoBE> permisos = new List<PermisoBE>();

            foreach (DataRow filaPermiso in resultado.Rows)
            {
                PermisoBE permiso = new PermisoBE();
                permiso.Id = int.Parse(filaPermiso["IdPermiso"].ToString());

                if (resultado.Columns.Contains("Nombre"))
                {
                    permiso.Nombre = filaPermiso["Nombre"].ToString();
                }

                permisos.Add(permiso);
            }

            return permisos;
        }
    }
}
