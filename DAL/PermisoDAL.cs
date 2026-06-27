<<<<<<< HEAD
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BE;
=======
﻿using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
>>>>>>> origin/main

namespace DAL
{
    public class PermisoDAL
    {
        public List<PermisoBE> ObtenerPermisos(RolBE rol, Acceso acceso)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();
<<<<<<< HEAD
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
=======
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
>>>>>>> origin/main
            }

            return permisos;
        }
    }
}
