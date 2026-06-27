using BE;
using DAL;
using System;
using System.Collections.Generic;
<<<<<<< HEAD
=======
using System.Linq;
using System.Text;
using System.Threading.Tasks;
>>>>>>> origin/main

namespace BLL
{
    public class IdiomaBLL
    {
<<<<<<< HEAD
        private readonly IdiomaDAL idiomaDAL;
        private readonly BitacoraBLL bitacoraBLL;

        public IdiomaBLL()
        {
            idiomaDAL = new IdiomaDAL();
            bitacoraBLL = new BitacoraBLL();
        }

        public List<IdiomaBE> ListarIdiomas()
        {
            return idiomaDAL.Listar();
        }

        public int AltaIdioma(string nombreIdioma, string usuario)
        {
            if (string.IsNullOrWhiteSpace(nombreIdioma))
            {
                throw new Exception("Ingresar nombre del idioma.");
            }

            int resultado = idiomaDAL.AltaIdioma(nombreIdioma.Trim());
            bitacoraBLL.RegistrarAlta(usuario, "Idiomas", "Alta de idioma " + nombreIdioma + ".");
            return resultado;
        }
=======
        public List<IdiomaBE> ListarIdiomas()
        {
            IdiomaDAL idiomaDAL = new IdiomaDAL();

            return idiomaDAL.Listar();
        }
>>>>>>> origin/main
    }
}
