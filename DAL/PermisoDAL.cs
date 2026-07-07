using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SERVICIOS.Composite;

namespace DAL
{
    public class PermisoDAL
    {
        public List<Permiso> ObtenerPermisos(Rol rol, Acceso acceso)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@idRol", rol.Id));

            DataTable resultado = acceso.Leer("ConsultarPermisos", parametros);
            List<Permiso> permisos = new List<Permiso>();

            foreach (DataRow filaPermiso in resultado.Rows)
            {
                Permiso permiso = new Permiso();
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
