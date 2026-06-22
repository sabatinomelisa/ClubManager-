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
    public  class RolDAL
    {
        public RolBE ObtenerRol(int idRol)
        {
            RolBE rol = new RolBE();

            Acceso acceso = new Acceso();

            acceso.Conectar();

            List<SqlParameter> parametros = new List<SqlParameter>();
            string sql = "ConsultarRol";
            parametros.Add(acceso.CrearParametro("@id", idRol));

            DataTable respuesta = new DataTable();

            respuesta = acceso.Leer(sql,parametros);

            foreach (DataRow row in respuesta.Rows)
            {
                rol.Id = int.Parse(row["IdRol"].ToString());
                rol.Nombre = row["Nombre"].ToString();
            }

            PermisoDAL permisoDAL = new PermisoDAL();

            List<PermisoBE> permisos = permisoDAL.ObtenerPermisos(rol,acceso);

            foreach(PermisoBE p in permisos)
            {
                rol.Hijos.Add(p);
            }

            acceso.Desconectar();

            return rol;
        }
    }
}
