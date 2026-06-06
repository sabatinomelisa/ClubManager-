using BE;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
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
            string sql = "Registrar";
            List<SqlParameter> parametros = new List<SqlParameter>();

            parametros.Add(acceso.CrearParametro("@usuario", usr.Username));
            parametros.Add(acceso.CrearParametro("@password", usr.Password));
            parametros.Add(acceso.CrearParametro("@fecha", (DateTime.Now)));

            int resultado = acceso.Escribir(sql, parametros);
            acceso.Desconectar();
            return resultado;

        }

    }
}
