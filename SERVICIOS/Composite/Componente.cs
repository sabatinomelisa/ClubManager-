using System.Collections.Generic;

namespace SERVICIOS.Composite
{
    public abstract class Componente
    {
        private int id;
        private string nombre;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        public abstract bool EsRol { get; }

        public abstract void AgregarComponente(Componente componente);

        public abstract void QuitarComponente(Componente componente);

        public abstract IList<Componente> ObtenerComponentes();
    }
}
