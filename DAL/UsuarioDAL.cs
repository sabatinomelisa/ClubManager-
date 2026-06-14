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
        public int ActPass(UsuarioBE usr)
        {
            Acceso acceso = new Acceso();

            acceso.Conectar();

            List<SqlParameter> parametros = new List<SqlParameter>();

            acceso.IniciarTx();

            try
            {
                string sql = "ActualizaPass";
                parametros.Clear();
                parametros.Add(acceso.CrearParametro("@usu", usr.Username));
                parametros.Add(acceso.CrearParametro("@nuevapass",usr.Password));
                int resultado = acceso.Escribir(sql, parametros);
                acceso.ConfirmarTx();
                acceso.Desconectar();
                return resultado;

            }
            catch(Exception ex)
            {
                acceso.CancelarTx();
                acceso.Desconectar();
                throw new Exception("Error al actualizar contraseña");
            }
        }

        public int AltaUsuario(UsuarioBE usr)
        {
            Acceso acceso = new Acceso();

            acceso.Conectar();

            List<SqlParameter> parametros = new List<SqlParameter>();

            acceso.IniciarTx();

            try
            {
                SocioDAL socio = new SocioDAL();

                int resultado = socio.AltaSocio(usr, acceso);

                if(resultado != -1)
                {
                    //Doy de alta el usuario
                    string sql = "RegistrarUsuario";
                    parametros.Clear();
                    parametros.Add(acceso.CrearParametro("@usuario", usr.Username));
                    parametros.Add(acceso.CrearParametro("@password", usr.Password));
                    parametros.Add(acceso.CrearParametro("@fechaCreacion", usr.FechaCreacion));
                    parametros.Add(acceso.CrearParametro("@id", usr.IdSocio));
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
