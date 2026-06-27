<<<<<<< HEAD
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
=======
﻿using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class SessionManager
    {
        private static object _lock = new object();

        private static SessionManager session;

        UsuarioBE Usuario { get; set; }
        public DateTime FechaInicio { get; set; }
        public static SessionManager GetInstance
        {
            get
            {
                if (session == null) throw new Exception("Sesión no iniciada");
                return session;
            }
>>>>>>> origin/main
        }

        public static void Login(UsuarioBE usuario)
        {
<<<<<<< HEAD
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
=======
            lock (_lock)
            {
                if (session == null)
                {
                    session = new SessionManager();
                    session.Usuario = usuario;
                    session.FechaInicio = DateTime.Now;

                }
                else
                {
                    throw new Exception("Sesión ya iniciada");
                }
            }

>>>>>>> origin/main
        }

        public static void Logout()
        {
<<<<<<< HEAD
            lock (bloqueadorSesion)
            {
                if (sesionActual == null)
                {
                    throw new InvalidOperationException("Sesión no iniciada.");
                }

                sesionActual = null;
            }
        }
=======
            lock (_lock)
            {
                if (session != null)
                {
                    session = null;
                }
                else
                {
                    throw new Exception("Sesion no iniciada");
                }
            }

        }
        private SessionManager()
        {

        }

>>>>>>> origin/main
    }
}
