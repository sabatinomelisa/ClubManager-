using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class IdiomaBLL
    {
        public List<IdiomaBE> ListarIdiomas()
        {
            IdiomaDAL idiomaDAL = new IdiomaDAL();

            return idiomaDAL.Listar();
        }
    }
}
