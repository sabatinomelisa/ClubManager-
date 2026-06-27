using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class HistorialBE
    {
		private int id;

		public int Id
		{
			get { return id; }
			set { id = value; }
		}

		private SocioBE socio;

		public SocioBE Socio
		{
			get { return socio; }
			set { socio = value; }
		}

		private DateTime fechaCreacion;

		public DateTime FechaCreacion
		{
			get { return fechaCreacion; }
			set { fechaCreacion = value; }
		}


	}
}
