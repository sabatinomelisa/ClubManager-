using System;

namespace BE
{
    public class EventoDeportivoBE
    {
        public int IdEvento { get; set; }
        public string Nombre { get; set; }
        public string Deporte { get; set; }
        public DateTime FechaEvento { get; set; }
        public string Lugar { get; set; }
        public string Estado { get; set; }
    }
}
