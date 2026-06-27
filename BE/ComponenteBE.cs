<<<<<<< HEAD
using System;
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
    public abstract class ComponenteBE
    {
        private int id;
<<<<<<< HEAD
        private string nombre;
=======
>>>>>>> origin/main

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

<<<<<<< HEAD
=======
        private string nombre;

>>>>>>> origin/main
        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

<<<<<<< HEAD
        public abstract bool EsRol { get; }

        public abstract void AgregarComponente(ComponenteBE componente);

        public abstract void QuitarComponente(ComponenteBE componente);

        public abstract IList<ComponenteBE> ObtenerComponentes();
=======
>>>>>>> origin/main
    }
}
