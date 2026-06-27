using System.Collections.Generic;

namespace BE
{
    public class RolBE : ComponenteBE
    {
        private readonly List<ComponenteBE> componentes;

        public RolBE()
        {
            componentes = new List<ComponenteBE>();
        }

        public override bool EsRol
        {
            get { return true; }
        }

        public IList<ComponenteBE> Hijos
        {
            get { return componentes; }
        }

        public override void AgregarComponente(ComponenteBE componente)
        {
            if (componente != null && !componentes.Contains(componente))
            {
                componentes.Add(componente);
            }
        }

        public override void QuitarComponente(ComponenteBE componente)
        {
            if (componente != null)
            {
                componentes.Remove(componente);
            }
        }

        public override IList<ComponenteBE> ObtenerComponentes()
        {
            return componentes.AsReadOnly();
        }
    }
}
