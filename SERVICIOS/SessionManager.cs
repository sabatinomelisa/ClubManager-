using System;
using BE;

namespace SERVICIOS
{
    public sealed class SessionManager
    {
        private static readonly object bloqueadorSesion = new object();
        private static SessionManager sesionActual;

        private SessionManager(UsuarioBE usuario)
        {
            Usuario = usuario;
            FechaInicio = DateTime.Now;
        }

        public UsuarioBE Usuario { get; private set; }
        public DateTime FechaInicio { get; private set; }

        public static bool SesionIniciada
        {
            get { return sesionActual != null; }
        }

        public static SessionManager ObtenerInstancia()
        {
            if (sesionActual == null)
            {
                throw new InvalidOperationException("Sesión no iniciada.");
            }

            return sesionActual;
        }

        public static UsuarioBE ObtenerUsuarioActual()
        {
            return ObtenerInstancia().Usuario;
        }

        public static void Login(UsuarioBE usuario)
        {
            if (usuario == null)
            {
                throw new ArgumentException("El usuario de sesión es obligatorio.");
            }

            lock (bloqueadorSesion)
            {
                if (sesionActual != null)
                {
                    throw new InvalidOperationException("Sesión ya iniciada.");
                }

                sesionActual = new SessionManager(usuario);
            }
        }

        public static void Logout()
        {
            lock (bloqueadorSesion)
            {
                if (sesionActual == null)
                {
                    throw new InvalidOperationException("Sesión no iniciada.");
                }

                sesionActual = null;
            }
        }
    }
}
