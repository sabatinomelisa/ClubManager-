using System;

namespace BE
{
    public class UsuarioBE : SocioBE
    {
        private string username;

        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        private string password;

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        private DateTime fechaCreacion;

        public DateTime FechaCreacion
        {
            get { return fechaCreacion; }
            set { fechaCreacion = value; }
        }

        private string bloqueado;

        public string Bloqueado
        {
            get { return bloqueado; }
            set { bloqueado = value; }
        }


        private int intentosFallidos;

        public int IntentosFallidos
        {
            get { return intentosFallidos; }
            set { intentosFallidos = value; }
        }

        private int idRol;

        public int IdRol
        {
            get { return idRol; }
            set { idRol = value; }
        }

        private string nombreRol;

        public string NombreRol
        {
            get { return nombreRol; }
            set { nombreRol = value; }
        }
    }
}
