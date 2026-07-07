using System.Collections.Generic;

namespace SERVICIOS.Composite
{
    public class Rol : Componente
    {
        private readonly List<Componente> componentes;

        public Rol()
        {
            componentes = new List<Componente>();
        }

        public override bool EsRol
        {
            get { return true; }
        }

        public IList<Componente> Hijos
        {
            get { return componentes.AsReadOnly(); }
        }

        public override void AgregarComponente(Componente componente)
        {
            if (componente != null && !componentes.Contains(componente))
            {
                componentes.Add(componente);
            }
        }

        public override void QuitarComponente(Componente componente)
        {
            if (componente != null)
            {
                componentes.Remove(componente);
            }
        }

        public override IList<Componente> ObtenerComponentes()
        {
            return componentes.AsReadOnly();
        }
    }
}
