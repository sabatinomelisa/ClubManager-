<<<<<<< HEAD
using System.Collections.Generic;
=======
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
>>>>>>> origin/main

namespace BE
{
    public class RolBE : ComponenteBE
    {
<<<<<<< HEAD
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
=======
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private string nombre;

        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        public List<ComponenteBE> Hijos { get; set; }

        public RolBE()
        {
            Hijos = new List<ComponenteBE>();
        }

    }

>>>>>>> origin/main
}
