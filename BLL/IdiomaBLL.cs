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

            string nombreNormalizado = nombreIdioma.Trim();
            foreach (IdiomaBE idioma in idiomaDAL.Listar())
            {
                if (string.Equals(idioma.Nombre, nombreNormalizado, StringComparison.OrdinalIgnoreCase))
                {
                    throw new Exception("Ya existe un idioma con ese nombre.");
                }
            }

            int idIdioma = idiomaDAL.AltaIdioma(nombreNormalizado);

            if (idIdioma <= 0)
            {
                foreach (IdiomaBE idioma in idiomaDAL.Listar())
                {
                    if (string.Equals(idioma.Nombre, nombreNormalizado, StringComparison.OrdinalIgnoreCase))
                    {
                        idIdioma = idioma.Id;
                        break;
                    }
                }
            }

            bitacoraBLL.RegistrarAlta(usuario, "Idiomas", "Alta de idioma " + nombreNormalizado + ".");
            return idIdioma;
        }

        public void BajaIdioma(int idIdioma, string usuario)
        {
            if (idIdioma <= 0)
            {
                throw new Exception("Seleccionar idioma.");
            }

            if (idIdioma == 1 || idIdioma == 2)
            {
                throw new Exception("No se pueden eliminar los idiomas base del sistema.");
            }

            IdiomaBE idiomaAEliminar = null;
            foreach (IdiomaBE idioma in idiomaDAL.Listar())
            {
                if (idioma.Id == idIdioma)
                {
                    idiomaAEliminar = idioma;
                    break;
                }
            }

            if (idiomaAEliminar == null)
            {
                throw new Exception("El idioma seleccionado no existe.");
            }

            idiomaDAL.BajaIdioma(idIdioma);
            bitacoraBLL.RegistrarBaja(usuario, "Idiomas", "Baja de idioma " + idiomaAEliminar.Nombre + ".");
        }
    }
}
