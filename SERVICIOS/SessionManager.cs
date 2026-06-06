using BE;
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
        }

        public static void Login(UsuarioBE usuario)
        {
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

        }

        public static void Logout()
        {
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

    }
}
