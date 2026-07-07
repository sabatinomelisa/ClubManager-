using System;
using System.Text.RegularExpressions;
using BE;
using DAL;
using SERVICIOS;

namespace BLL
{
    public class UsuarioBLL
    {
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
            integridadBLL.ValidarIntegridadParaLogin();

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
                integridadBLL.ActualizarIntegridadParaLogout();
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


        public UsuarioBE ObtenerPorIdSocio(int idSocio)
        {
            if (idSocio <= 0)
            {
                throw new Exception("Seleccionar un socio válido.");
            }

            return usuarioDAL.ObtenerPorIdSocio(idSocio);
        }

        public int CambiarRolSocio(int idSocio, int idRol, string usuarioAdministrador)
        {
            if (idSocio <= 0)
            {
                throw new Exception("Seleccionar un socio válido.");
            }

            if (idRol != 1 && idRol != 2 && idRol != 3)
            {
                throw new Exception("Solo se puede asignar Administrador, Socio Simple o Socio Pleno desde esta pantalla.");
            }

            UsuarioBE usuario = usuarioDAL.ObtenerPorIdSocio(idSocio);

            if (usuario == null)
            {
                throw new Exception("El socio seleccionado no tiene usuario asociado.");
            }

            int resultado = usuarioDAL.CambiarRolPorIdSocio(idSocio, idRol);

            if (resultado > 0)
            {
                string nombreAdmin = string.IsNullOrWhiteSpace(usuarioAdministrador) ? "SIN_SESION" : usuarioAdministrador;
                string tipoSocio = idRol == 1 ? "Administrador" : (idRol == 3 ? "Socio Pleno" : "Socio Simple");
                bitacoraBLL.RegistrarModificacion(nombreAdmin, "Usuarios", "Cambio de tipo de socio para " + usuario.Username + " a " + tipoSocio + ".");
            }

            return resultado;
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
        }

        private bool ValidarMail(string mail)
        {
            return Regex.IsMatch(mail, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        private bool ValidarPass(string password)
        {
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
        }
    }
}
