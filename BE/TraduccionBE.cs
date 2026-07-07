using System;

namespace BE
{
    public class TraduccionBE
    {
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private string nombreControl;

        public string NombreControl
        {
            get { return nombreControl; }
            set { nombreControl = value; }
        }

        private IdiomaBE idioma;

        public IdiomaBE Idioma
        {
            get { return idioma; }
            set { idioma = value; }
        }

        private string traduccion;

        public string Traduccion
        {
            get { return traduccion; }
            set { traduccion = value; }
        }
    }
}
