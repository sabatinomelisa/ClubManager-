using BE;
using DAL;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class IdiomaBLL
    {
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
    }
}
