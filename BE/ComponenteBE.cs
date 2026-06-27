using System;
using System.Collections.Generic;

namespace BE
{
    public abstract class ComponenteBE
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

        public abstract void AgregarComponente(ComponenteBE componente);

        public abstract void QuitarComponente(ComponenteBE componente);

        public abstract IList<ComponenteBE> ObtenerComponentes();
    }
}
