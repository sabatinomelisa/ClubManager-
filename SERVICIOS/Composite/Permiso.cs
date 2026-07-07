using System;
using System.Collections.Generic;

namespace SERVICIOS.Composite
{
    public class Permiso : Componente
    {
        public override bool EsRol
        {
            get { return false; }
        }

        public override void AgregarComponente(Componente componente)
        {
            throw new InvalidOperationException("Un permiso no puede contener otros componentes.");
        }

        public override void QuitarComponente(Componente componente)
        {
            throw new InvalidOperationException("Un permiso no puede contener otros componentes.");
        }

        public override IList<Componente> ObtenerComponentes()
        {
            return new List<Componente>().AsReadOnly();
        }
    }
}
