using System;
<<<<<<< HEAD
=======
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
>>>>>>> origin/main

namespace BE
{
    public class SocioBE
    {
<<<<<<< HEAD
        private int idSocio;
        public int IdSocio
        {
            get { return idSocio; }
            set { idSocio = value; }
        }

        private string tipoDocumento;
        public string TipoDocumento
        {
            get { return tipoDocumento; }
            set { tipoDocumento = value; }
        }

        private int numeroDocumento;
        public int NumeroDocumento
        {
            get { return numeroDocumento; }
            set { numeroDocumento = value; }
        }

        private string nombre;
        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        private string apellido;
        public string Apellido
        {
            get { return apellido; }
            set { apellido = value; }
        }

        private DateTime fechaNacimiento;
        public DateTime FechaNacimiento
        {
            get { return fechaNacimiento; }
            set { fechaNacimiento = value; }
        }

        private string nacionalidad;
        public string Nacionalidad
        {
            get { return nacionalidad; }
            set { nacionalidad = value; }
        }

        private string mail;
        public string Mail
        {
            get { return mail; }
            set { mail = value; }
        }

        private int telefono;
        public int Telefono
        {
            get { return telefono; }
            set { telefono = value; }
        }

        private string activo;
        public string Activo
        {
            get { return activo; }
            set { activo = value; }
        }

        private int digitoVerificadorHorizontal;
        public int DigitoVerificadorHorizontal
        {
            get { return digitoVerificadorHorizontal; }
            set { digitoVerificadorHorizontal = value; }
        }

        public string NombreCompleto
        {
            get { return Nombre + " " + Apellido; }
        }
    }
=======
		private int idSocio;

		public int IdSocio
		{
			get { return idSocio; }
			set { idSocio = value; }
		}

		private string tipoDocumento;

		public string TipoDocumento
		{
			get { return tipoDocumento; }
			set { tipoDocumento = value; }
		}

		private int numeroDocumento;

		public int NumeroDocumento
		{
			get { return numeroDocumento; }
			set { numeroDocumento = value; }
		}

		private string nombre;

		public string Nombre
		{
			get { return nombre; }
			set { nombre = value; }
		}

		private string apellido;

		public string Apellido
		{
			get { return apellido; }
			set { apellido = value; }
		}

		private DateTime fechaNacimiento;

		public DateTime FechaNacimiento
		{
			get { return fechaNacimiento; }
			set { fechaNacimiento = value; }
		}

		private string nacionalidad;

		public string Nacionalidad
		{
			get { return nacionalidad; }
			set { nacionalidad = value; }
		}

		private string mail;

		public string Mail
		{
			get { return mail; }
			set { mail = value; }
		}

		private int telefono;

		public int Telefono
		{
			get { return telefono; }
			set { telefono = value; }
		}



	}
>>>>>>> origin/main
}
