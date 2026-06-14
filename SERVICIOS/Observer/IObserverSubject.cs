using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICIOS.Observer
{
    public interface IObserverSubject
    {
        void Suscribir(IOberverIdioma observer);

        void Desuscribir(IOberverIdioma observer);
        void Notificar();

    }
}
