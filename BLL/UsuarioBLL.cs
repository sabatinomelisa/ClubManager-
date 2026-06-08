using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class UsuarioBLL
    {
        public int AltaUsuario(UsuarioBE usr)
        {
            UsuarioDAL usrDAL = new UsuarioDAL();

            ValidarDatos(usr);

            return usrDAL.AltaUsuario(usr);
        }

        private void ValidarDatos(UsuarioBE usr)
        {
            if (usr == null)
                throw new Exception("Ingresar datos para  el registro");

            if (usr.Nombre == string.Empty)
                throw new Exception("Ingresar Nombre");

            if (usr.Apellido == string.Empty)
                throw new Exception("Ingresar Apellido");

            if (usr.TipoDocumento == string.Empty)
                throw new Exception("Seleccionar Tipo de Documento");

            if (usr.NumeroDocumento == null)
                throw new Exception("Ingresar Numero de Documento");

            if (usr.FechaNacimiento == null)
                throw new Exception("Ingresar Fecha de Nacimiento");

            if (usr.Nacionalidad == string.Empty)
                throw new Exception("Ingresar Nacionalidad");

            if (usr.Username == string.Empty)
                throw new Exception("Ingresar Nombre de Usuario");

            if (usr.Password == string.Empty)
                throw new Exception("Ingresar Contraseña");
        }
    }
}
