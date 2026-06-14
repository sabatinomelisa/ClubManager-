using BE;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class SocioDAL
    {

        public int AltaSocio(UsuarioBE usr, Acceso acceso)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();
            //Busco el proximo ID del socio para insertar
            string sql = "IdMaximo";
            parametros.Clear();
            int idSocio = acceso.DevolverEscalar(sql, parametros);
            usr.IdSocio = idSocio;

            //creo parámetros para el alta del socio

            parametros.Clear();
            parametros.Add(acceso.CrearParametro("@idSocio", idSocio));
            parametros.Add(acceso.CrearParametro("@tipDoc", usr.TipoDocumento));
            parametros.Add(acceso.CrearParametro("@nroDoc", usr.NumeroDocumento));
            parametros.Add(acceso.CrearParametro("@nombre", usr.Nombre));
            parametros.Add(acceso.CrearParametro("@apellido", usr.Apellido));
            parametros.Add(acceso.CrearParametro("@fecNac", usr.FechaNacimiento));
            parametros.Add(acceso.CrearParametro("@nacionalidad", usr.Nacionalidad));
            parametros.Add(acceso.CrearParametro("@mail", usr.Mail));
            parametros.Add(acceso.CrearParametro("@telefono", usr.Telefono));

            //Doy de alta al socio
            sql = "RegistrarSocio";
            int resultado = acceso.Escribir(sql, parametros);
           
            return resultado;

        }
    }
}
