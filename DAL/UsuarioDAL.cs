using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class UsuarioDAL
    {
        public int AltaUsuario(UsuarioBE usr)
        {
            Acceso acceso = new Acceso();

            acceso.Conectar();

            List<SqlParameter> parametros = new List<SqlParameter>();

            acceso.IniciarTx();

            try
            {

                //Busco el ID del socio
                string sql = "IdMaximo";
                parametros.Clear();
                int idSocio = acceso.DevolverEscalar(sql, parametros);


                //creo parámetros para el alta del socio
                parametros.Clear();
                parametros.Add(acceso.CrearParametro("@idSocio", idSocio));
                parametros.Add(acceso.CrearParametro("@tipDoc", usr.TipoDocumento));
                parametros.Add(acceso.CrearParametro("@nroDoc", usr.NumeroDocumento));
                parametros.Add(acceso.CrearParametro("@nombre", usr.Nombre));
                parametros.Add(acceso.CrearParametro("@apellido", usr.Apellido));
                parametros.Add(acceso.CrearParametro("@fecNac", usr.FechaNacimiento));
                parametros.Add(acceso.CrearParametro("@nacionalidad", usr.Nacionalidad));

                //Doy de alta al socio
                sql = "RegistrarSocio";
                int resultado = acceso.Escribir(sql, parametros);

                if(resultado != -1)
                {
                    //Doy de alta el usuario
                    sql = "RegistrarUsuario";
                    parametros.Clear();
                    parametros.Add(acceso.CrearParametro("@usuario", usr.Username));
                    parametros.Add(acceso.CrearParametro("@password", usr.Password));
                    parametros.Add(acceso.CrearParametro("@fechaCreacion", usr.FechaCreacion));
                    parametros.Add(acceso.CrearParametro("@id", idSocio));
                    parametros.Add(acceso.CrearParametro("@bloqueado", usr.Bloqueado));

                    resultado = acceso.Escribir(sql, parametros);
                    if (resultado != -1)
                    {
                        acceso.ConfirmarTx();
                    }
                    acceso.Desconectar();
                    return resultado;
                }else
                {
                    acceso.Desconectar();
                    return resultado;
                }

            }
            catch(Exception ex)
            {
                acceso.CancelarTx();
                acceso.Desconectar();
                throw new Exception("Error al registrar al usuario");
            }

        }

        public UsuarioBE DevolverUser(string usrIngresado,string password=null)
        {
            Acceso acceso = new Acceso();
            UsuarioBE usrAux = new UsuarioBE();
            string sql;

            acceso.Conectar();

            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Clear();
            parametros.Add(acceso.CrearParametro("@usu", usrIngresado));

            if(password==null)
            {
                //Consulto si el usuario ya existe
                sql = "ConsultaUsuario";
                string usrConsultado = acceso.DevolverEscalarString(sql, parametros);
                usrAux.Username = usrConsultado;
            }
            else
            {
                sql = "ConsultaUsrPass";
                parametros.Add(acceso.CrearParametro("@pass", password));
                DataTable tabla = new DataTable();
                tabla = acceso.Leer(sql,parametros);

                foreach (DataRow row in tabla.Rows)
                {
                    usrAux.Username = row["NombreUsuario"].ToString();
                    usrAux.Password = row["Contraseña"].ToString();

                }

            }

            acceso.Desconectar();

            return usrAux;
        }

    }
}
