using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class TraduccionBLL
    {
        public List<TraduccionBE> Listar(int idiomaSel)
        {
            TraduccionDAL tradDAL = new TraduccionDAL();

            List<TraduccionBE> traducciones= new List<TraduccionBE>();

            traducciones = tradDAL.ListarTraducciones(idiomaSel);

            return traducciones;
        }
    }
}
