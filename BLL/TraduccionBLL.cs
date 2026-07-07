using BE;
using DAL;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class TraduccionBLL
    {
        private readonly TraduccionDAL traduccionDAL;
        private readonly BitacoraBLL bitacoraBLL;

        public TraduccionBLL()
        {
            traduccionDAL = new TraduccionDAL();
            bitacoraBLL = new BitacoraBLL();
        }

        public List<TraduccionBE> Listar(int idiomaSel)
        {
            return traduccionDAL.ListarTraducciones(idiomaSel);
        }

        public int GuardarTraduccion(int idIdioma, string nombreControl, string textoTraduccion, string usuario)
        {
            if (idIdioma <= 0)
            {
                throw new Exception("Seleccionar idioma.");
            }

            if (string.IsNullOrWhiteSpace(nombreControl))
            {
                throw new Exception("Ingresar nombre del control.");
            }

            if (string.IsNullOrWhiteSpace(textoTraduccion))
            {
                throw new Exception("Ingresar traducción.");
            }

            int resultado = traduccionDAL.GuardarTraduccion(idIdioma, nombreControl.Trim(), textoTraduccion.Trim());
            bitacoraBLL.RegistrarModificacion(usuario, "Idiomas", "Traducción actualizada para " + nombreControl + ".");
            return resultado;
        }
    }
}
