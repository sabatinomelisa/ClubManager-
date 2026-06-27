<<<<<<< HEAD
using BE;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class RolDAL
    {
        public List<RolBE> ListarRoles()
        {
            Acceso acceso = new Acceso();
            acceso.Conectar();

            DataTable resultado = acceso.Leer("ConsultarRoles", new List<SqlParameter>());
            List<RolBE> roles = new List<RolBE>();

            foreach (DataRow filaRol in resultado.Rows)
            {
                RolBE rol = new RolBE();
                rol.Id = int.Parse(filaRol["IdRol"].ToString());
                rol.Nombre = filaRol["Nombre"].ToString();
                roles.Add(rol);
            }

            acceso.Desconectar();
            return roles;
        }

        public RolBE ObtenerRol(int idRol)
        {
            Acceso acceso = new Acceso();
            acceso.Conectar();

            RolBE rol = ObtenerRolPorId(idRol, acceso);

            if (rol.Id > 0)
            {
                CargarComponentesDeRol(rol, acceso, new List<int>());
            }

            acceso.Desconectar();
            return rol;
        }

        private RolBE ObtenerRolPorId(int idRol, Acceso acceso)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@id", idRol));

            DataTable resultado = acceso.Leer("ConsultarRol", parametros);
            RolBE rol = new RolBE();

            foreach (DataRow filaRol in resultado.Rows)
            {
                rol.Id = int.Parse(filaRol["IdRol"].ToString());
                rol.Nombre = filaRol["Nombre"].ToString();
            }

            return rol;
        }

        private void CargarComponentesDeRol(RolBE rolPadre, Acceso acceso, List<int> rolesVisitados)
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
                    RolBE rolHijo = new RolBE();
                    rolHijo.Id = int.Parse(filaComponente["IdComponente"].ToString());
                    rolHijo.Nombre = filaComponente["Nombre"].ToString();

                    CargarComponentesDeRol(rolHijo, acceso, rolesVisitados);
                    rolPadre.AgregarComponente(rolHijo);
                }
                else
                {
                    PermisoBE permiso = new PermisoBE();
                    permiso.Id = int.Parse(filaComponente["IdComponente"].ToString());
                    permiso.Nombre = filaComponente["Nombre"].ToString();
                    rolPadre.AgregarComponente(permiso);
                }
            }
        }
=======
﻿using BE;
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
>>>>>>> origin/main
    }
}
