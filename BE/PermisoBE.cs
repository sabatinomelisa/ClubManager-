using System;
using System.Collections.Generic;

namespace BE
{
    public class PermisoBE : ComponenteBE
    {
        public override bool EsRol
        {
            get { return false; }
        }

        public override void AgregarComponente(ComponenteBE componente)
        {
            throw new InvalidOperationException("Un permiso no puede contener otros componentes.");
        }

        public override void QuitarComponente(ComponenteBE componente)
        {
            throw new InvalidOperationException("Un permiso no puede contener otros componentes.");
        }

        public override IList<ComponenteBE> ObtenerComponentes()
        {
            return new List<ComponenteBE>().AsReadOnly();
        }
    }
}
