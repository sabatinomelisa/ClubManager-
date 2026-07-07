using SERVICIOS.Composite;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class RolDAL
    {
        public List<Rol> ListarRoles()
        {
            Acceso acceso = new Acceso();
            acceso.Conectar();

            DataTable resultado = acceso.Leer("ConsultarRoles", new List<SqlParameter>());
            List<Rol> roles = new List<Rol>();

            foreach (DataRow filaRol in resultado.Rows)
            {
                Rol rol = new Rol();
                rol.Id = int.Parse(filaRol["IdRol"].ToString());
                rol.Nombre = filaRol["Nombre"].ToString();
                roles.Add(rol);
            }

            acceso.Desconectar();
            return roles;
        }

        public Rol ObtenerRol(int idRol)
        {
            Acceso acceso = new Acceso();
            acceso.Conectar();

            Rol rol = ObtenerRolPorId(idRol, acceso);

            if (rol.Id > 0)
            {
                CargarComponentesDeRol(rol, acceso, new List<int>());
            }

            acceso.Desconectar();
            return rol;
        }

        private Rol ObtenerRolPorId(int idRol, Acceso acceso)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@id", idRol));

            DataTable resultado = acceso.Leer("ConsultarRol", parametros);
            Rol rol = new Rol();

            foreach (DataRow filaRol in resultado.Rows)
            {
                rol.Id = int.Parse(filaRol["IdRol"].ToString());
                rol.Nombre = filaRol["Nombre"].ToString();
            }

            return rol;
        }

        private void CargarComponentesDeRol(Rol rolPadre, Acceso acceso, List<int> rolesVisitados)
        {
            if (rolesVisitados.Contains(rolPadre.Id))
            {
                return;
            }

            rolesVisitados.Add(rolPadre.Id);

            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@idRol", rolPadre.Id));

            DataTable componentes = acceso.Leer("ConsultarComponentesRol", parametros);

            foreach (DataRow filaComponente in componentes.Rows)
            {
                string tipoComponente = filaComponente["TipoComponente"].ToString();

                if (tipoComponente == "ROL")
                {
                    Rol rolHijo = new Rol();
                    rolHijo.Id = int.Parse(filaComponente["IdComponente"].ToString());
                    rolHijo.Nombre = filaComponente["Nombre"].ToString();

                    CargarComponentesDeRol(rolHijo, acceso, rolesVisitados);
                    rolPadre.AgregarComponente(rolHijo);
                }
                else
                {
                    Permiso permiso = new Permiso();
                    permiso.Id = int.Parse(filaComponente["IdComponente"].ToString());
                    permiso.Nombre = filaComponente["Nombre"].ToString();
                    rolPadre.AgregarComponente(permiso);
                }
            }
        }
    }
}
