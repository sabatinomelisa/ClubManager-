using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICIOS.Observer
{
    public class TratamientoIdioma
    {
        //Utilizamos  un Singleton para gestionar el Observer para que solamente exista un idioma activo a la vez
        private static TratamientoIdioma instancia;

        private List<IOberverIdioma> observadores;

        private IdiomaBE idiomaActual;

        public IdiomaBE IdiomaActual
        {
            get { return idiomaActual; }
            set { idiomaActual = value; }
        }

        private TratamientoIdioma()
        {
            observadores=new List<IOberverIdioma>();
        }
        public static TratamientoIdioma Instancia
        {
            get
            {
                if(instancia==null)
                    instancia=new TratamientoIdioma();
                return instancia;
            }
        }
        public void Suscribir(IOberverIdioma observer)
        {
            observadores.Add(observer);   
        }

        public void Desuscribir(IOberverIdioma observer)
        {
            observadores.Remove(observer);
        }

        public void Notificar()
        {
            foreach(var o in observadores)
            {
                o.ActualizarIdioma();
            }
        }

    }
}
