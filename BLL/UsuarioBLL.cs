<<<<<<< HEAD
﻿using System;
using System.Text.RegularExpressions;
using BE;
using DAL;
using SERVICIOS;
=======
﻿using BE;
using DAL;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
>>>>>>> origin/main

namespace BLL
{
    public class UsuarioBLL
    {
<<<<<<< HEAD
        private const int CantidadMaximaIntentosFallidos = 3;
        private readonly UsuarioDAL usuarioDAL;
        private readonly BitacoraBLL bitacoraBLL;
        private readonly IntegridadBLL integridadBLL;

        public UsuarioBLL()
        {
            usuarioDAL = new UsuarioDAL();
            bitacoraBLL = new BitacoraBLL();
            integridadBLL = new IntegridadBLL();
        }

        public int AltaUsuario(UsuarioBE usuario)
        {
            ValidarDatosRegistro(usuario);
            usuario.Password = Seguridad.GenerarHash(usuario.Password);
            usuario.Bloqueado = ObtenerValorSiNo(usuario.Bloqueado, "N");
            usuario.Activo = ObtenerValorSiNo(usuario.Activo, "S");

            int resultado = usuarioDAL.AltaUsuario(usuario);

            if (resultado > 0)
            {
                integridadBLL.RecalcularIntegridad();
                bitacoraBLL.RegistrarAlta(usuario.Username, "Usuarios", "Usuario registrado correctamente.");
            }

            return resultado;
        }

        public UsuarioBE Login(string username, string password)
        {
            ValidarCredencialesIngresadas(username, password);

            UsuarioBE usuario = usuarioDAL.ObtenerPorNombreUsuario(username);

            if (usuario == null)
            {
                bitacoraBLL.Registrar("SIN_SESION", "LOGIN_FALLIDO", "Seguridad", "Intento de login con usuario inexistente: " + username);
                throw new Exception("Usuario inexistente.");
            }

            if (usuario.Activo == "N")
            {
                bitacoraBLL.Registrar(username, "LOGIN_FALLIDO", "Seguridad", "El usuario está inactivo.");
                throw new Exception("El usuario está inactivo.");
            }

            if (usuario.Bloqueado == "S")
            {
                bitacoraBLL.Registrar(username, "LOGIN_FALLIDO", "Seguridad", "El usuario está bloqueado.");
                throw new Exception("El usuario está bloqueado.");
            }

            bool passwordValida = Seguridad.VerificarPassword(password, usuario.Password);

            if (!passwordValida)
            {
                usuarioDAL.IncrementarIntentosFallidos(username);
                UsuarioBE usuarioActualizado = usuarioDAL.ObtenerPorNombreUsuario(username);

                if (usuarioActualizado != null && usuarioActualizado.IntentosFallidos >= CantidadMaximaIntentosFallidos)
                {
                    usuarioDAL.BloquearUsuario(username);
                    bitacoraBLL.Registrar(username, "BLOQUEO", "Seguridad", "Usuario bloqueado por superar la cantidad máxima de intentos fallidos.");
                    throw new Exception("Contraseña incorrecta. La cuenta fue bloqueada por superar los 3 intentos fallidos.");
                }

                bitacoraBLL.Registrar(username, "LOGIN_FALLIDO", "Seguridad", "Contraseña incorrecta.");
                throw new Exception("Contraseña incorrecta.");
            }

            usuarioDAL.ReiniciarIntentosFallidos(username);
            SessionManager.Login(usuario);
            bitacoraBLL.RegistrarLogin(username);

            return usuario;
        }

        public void Logout()
        {
            string nombreUsuario = "SIN_SESION";

            if (SessionManager.SesionIniciada)
            {
                nombreUsuario = SessionManager.ObtenerUsuarioActual().Username;
                SessionManager.Logout();
            }

            bitacoraBLL.RegistrarLogout(nombreUsuario);
        }

        public bool ValidarUsuario(string username, string password)
        {
            try
            {
                UsuarioBE usuario = usuarioDAL.ObtenerPorNombreUsuario(username);
                return usuario != null && Seguridad.VerificarPassword(password, usuario.Password);
            }
            catch
            {
                return false;
            }
        }

        public int CambiarContraseña(UsuarioBE usuario)
        {
            if (usuario == null)
            {
                throw new Exception("Ingresar datos del usuario.");
            }

            if (string.IsNullOrWhiteSpace(usuario.Username))
            {
                throw new Exception("Ingresar nombre de usuario.");
            }

            if (!ValidarPass(usuario.Password))
            {
                throw new Exception("La contraseña debe tener al menos 8 caracteres, una letra y un número.");
            }

            usuario.Password = Seguridad.GenerarHash(usuario.Password);
            int filas = usuarioDAL.ActPass(usuario);

            if (filas > 0)
            {
                usuarioDAL.ReiniciarIntentosFallidos(usuario.Username);
                bitacoraBLL.RegistrarModificacion(usuario.Username, "Usuarios", "Contraseña actualizada correctamente.");
            }

            return filas;
        }

        public int CambiarContraseña(string username, string passwordActual, string passwordNueva)
        {
            UsuarioBE usuario = usuarioDAL.ObtenerPorNombreUsuario(username);

            if (usuario == null)
            {
                throw new Exception("Usuario inexistente.");
            }

            if (!Seguridad.VerificarPassword(passwordActual, usuario.Password))
            {
                bitacoraBLL.Registrar(username, "CAMBIO_PASSWORD_FALLIDO", "Seguridad", "Contraseña actual incorrecta.");
                throw new Exception("La contraseña actual es incorrecta.");
            }

            UsuarioBE usuarioCambioPassword = new UsuarioBE();
            usuarioCambioPassword.Username = username;
            usuarioCambioPassword.Password = passwordNueva;

            return CambiarContraseña(usuarioCambioPassword);
        }

        public int BloquearUsuario(string username)
        {
            ValidarNombreUsuario(username);
            int resultado = usuarioDAL.BloquearUsuario(username);
            bitacoraBLL.RegistrarModificacion(username, "Usuarios", "Usuario bloqueado manualmente.");
            return resultado;
        }

        public int DesbloquearUsuario(string username)
        {
            ValidarNombreUsuario(username);
            int resultado = usuarioDAL.DesbloquearUsuario(username);
            bitacoraBLL.RegistrarModificacion(username, "Usuarios", "Usuario desbloqueado manualmente.");
            return resultado;
        }

        public int ActivarUsuario(string username)
        {
            ValidarNombreUsuario(username);
            int resultado = usuarioDAL.ActivarUsuario(username);
            bitacoraBLL.RegistrarModificacion(username, "Usuarios", "Usuario activado.");
            return resultado;
        }

        public int DesactivarUsuario(string username)
        {
            ValidarNombreUsuario(username);
            int resultado = usuarioDAL.DesactivarUsuario(username);
            bitacoraBLL.RegistrarModificacion(username, "Usuarios", "Usuario desactivado.");
            return resultado;
        }

        private void ValidarDatosRegistro(UsuarioBE usuario)
        {
            if (usuario == null)
            {
                throw new Exception("Ingresar datos para el registro.");
            }

            if (string.IsNullOrWhiteSpace(usuario.Nombre))
            {
                throw new Exception("Ingresar nombre.");
            }

            if (string.IsNullOrWhiteSpace(usuario.Apellido))
            {
                throw new Exception("Ingresar apellido.");
            }

            if (string.IsNullOrWhiteSpace(usuario.TipoDocumento))
            {
                throw new Exception("Seleccionar tipo de documento.");
            }

            if (usuario.NumeroDocumento <= 0)
            {
                throw new Exception("Ingresar número de documento válido.");
            }

            if (usuario.FechaNacimiento == DateTime.MinValue)
            {
                throw new Exception("Ingresar fecha de nacimiento.");
            }

            if (string.IsNullOrWhiteSpace(usuario.Nacionalidad))
            {
                throw new Exception("Ingresar nacionalidad.");
            }

            if (string.IsNullOrWhiteSpace(usuario.Mail))
            {
                throw new Exception("Ingresar mail.");
            }

            if (!ValidarMail(usuario.Mail))
            {
                throw new Exception("El mail tiene un formato incorrecto.");
            }

            if (usuario.Telefono <= 0)
            {
                throw new Exception("Ingresar teléfono válido.");
            }

            ValidarNombreUsuario(usuario.Username);

            UsuarioBE usuarioExistente = usuarioDAL.ObtenerPorNombreUsuario(usuario.Username);
            if (usuarioExistente != null)
            {
                throw new Exception("Nombre de usuario ya utilizado.");
            }

            if (!ValidarPass(usuario.Password))
            {
                throw new Exception("La contraseña debe tener al menos 8 caracteres, una letra y un número.");
            }
        }

        private void ValidarCredencialesIngresadas(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new Exception("Ingresar nombre de usuario.");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new Exception("Ingresar contraseña.");
            }
        }

        private void ValidarNombreUsuario(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new Exception("Ingresar nombre de usuario.");
            }
=======
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


>>>>>>> origin/main
        }

        private bool ValidarMail(string mail)
        {
<<<<<<< HEAD
            return Regex.IsMatch(mail, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
=======
            bool valido = Regex.IsMatch(mail, @"^.+@.+\.com$");
            return valido;
>>>>>>> origin/main
        }

        private bool ValidarPass(string password)
        {
<<<<<<< HEAD
            if (string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            string patron = @"^(?=.*[A-Za-z])(?=.*\d).{8,}$";
            return Regex.IsMatch(password, patron);
        }

        private string ObtenerValorSiNo(string valor, string valorDefault)
        {
            if (string.IsNullOrWhiteSpace(valor))
            {
                return valorDefault;
            }

            return valor.Trim().ToUpper() == "S" ? "S" : "N";
=======
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

>>>>>>> origin/main
        }
    }
}
