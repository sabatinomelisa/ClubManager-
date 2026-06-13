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

        //Alta de un nuevo uauario
        public int AltaUsuario(UsuarioBE usr)
        {
            //Valido integridad de los datos ingresados
            ValidarDatos(usr);
            //Encriptado de Password
            usr.Password = Seguridad.GenerarHash(usr.Password);

            return usrDAL.AltaUsuario(usr);
        }

        private void ValidarDatos(UsuarioBE usr)
        {
            //Valido que se hayan ingresado los datos
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

            if (usr.Mail == string.Empty)
            {
                throw new Exception("Ingresar Mail");
            }else
            {
                bool mailOk = ValidarMail(usr.Mail);
                if (!mailOk)
                    throw new Exception("El mail tiene un formato incorrecto");
            }


            if (usr.Telefono == null)
                throw new Exception("Ingresar Telefono");

            if (usr.Username == string.Empty)
            {
                throw new Exception("Ingresar Nombre de Usuario");
            }
            else
            {
                //Vaido que el usuario no exista
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
                //Valido que la password cumpla con todos los requisitos
                bool passOk = ValidarPass(usr.Password);
                if(!passOk)
                {
                    throw new Exception("La contraseña debe tener al menos 8 caracteres, una letra y un número.");
                }
            }


        }

        private bool ValidarMail(string mail)
        {
            bool valido = Regex.IsMatch(mail, @"^.+@.+\.com$");
            return valido;
        }

        private bool ValidarPass(string password)
        {
            //Al menos 8 caracteres, una  letra y un número
            string patron = @"^(?=.*[A-Za-z])(?=.*\d).{8,}$";

            return Regex.IsMatch(password, patron);
        }


        public void Login(string username, string password)
        {
            //Consulto si el usuario es correcto
            UsuarioBE usuario = usrDAL.DevolverUser(username, password);

            if (usuario == null)
                throw new Exception("Usuario inexistente");

            //Si paso la validacion del usuario verifico que la password sea correcta
            bool passwordValida = Seguridad.VerificarPassword(password, usuario.Password);

            if (!passwordValida)
                throw new Exception("Contraseña incorrecta");

            SessionManager.Login(usuario);
        }

        public bool ValidarUsuario(string username, string password)
        {
            UsuarioBE usuario = usrDAL.DevolverUser(username, password);

            if (usuario == null)
                return false;

            bool passok = ValidarPass(password);

            return passok;
        }

        public int CambiarContraseña(UsuarioBE usr)
        {
            UsuarioDAL usrDAL = new UsuarioDAL();
            
            int filas = usrDAL.ActPass(usr);

            return filas;

        }
    }
}
