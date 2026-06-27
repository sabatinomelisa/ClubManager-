using System;

namespace BE
{
    public class CambioSocioBE
    {
        public int IdCambioSocio { get; set; }
        public int IdSocio { get; set; }
        public string Usuario { get; set; }
        public DateTime FechaCambio { get; set; }
        public string Accion { get; set; }
        public string EstadoAnterior { get; set; }
        public string EstadoNuevo { get; set; }
        public string Descripcion { get; set; }
    }
}
