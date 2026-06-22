using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class RolBE : ComponenteBE
    {
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private string nombre;

        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        public List<ComponenteBE> Hijos { get; set; }

        public RolBE()
        {
            Hijos = new List<ComponenteBE>();
        }

    }

}
