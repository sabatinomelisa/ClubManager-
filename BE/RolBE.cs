using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class RolBE : ComponenteBE
    {
        public List<ComponenteBE> Hijos { get; set; }

        public RolBE()
        {
            Hijos = new List<ComponenteBE>();
        }

    }

}
