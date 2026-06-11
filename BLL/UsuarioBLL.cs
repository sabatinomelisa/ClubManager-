using BE;
using DAL;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BLL
{
    public class UsuarioBLL
    {
        UsuarioDAL usrDAL = new UsuarioDAL();

        public int AltaUsuario(UsuarioBE usr)
        {

            ValidarDatos(usr);

            usr.Password = Seguridad.GenerarHash(usr.Password);

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
            {
                throw new Exception("Ingresar Nombre de Usuario");
            }
            else
            {
                UsuarioBE usuario = usrDAL.DevolverUser(usr.Username);

                if (usuario.Username==usr.Username)
                {
                    throw new Exception("Nombre de Usuario ya utilizado");
                }
            }

            if (usr.Password == string.Empty)
            {
                throw new Exception("Ingresar Contraseña");

            }
            else
            {
                bool passOk = ValidarPass(usr.Password);
                if(!passOk)
                {
                    throw new Exception("La contraseña debe tener al menos 8 caracteres, una letra y un número.");
                }
            }


        }

        private bool ValidarPass(string password)
        {
            //Al menos 8 caracteres, una  letra y un número
            string patron = @"^(?=.*[A-Za-z])(?=.*\d).{8,}$";

            return Regex.IsMatch(password, patron);
        }


        public void Login(string username, string password)
        {
            UsuarioBE usuario = usrDAL.DevolverUser(username,password);

            if (usuario == null)
                throw new Exception("Usuario inexistente");

            bool passwordValida = Seguridad.VerificarPassword(password, usuario.Password);

            if (!passwordValida)
                throw new Exception("Contraseña incorrecta");

            SessionManager.Login(usuario);
        }
    }
}
