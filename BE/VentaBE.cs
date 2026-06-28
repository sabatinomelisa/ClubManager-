using System;

namespace BE
{
    public class VentaBE
    {
        public int IdVenta { get; set; }
        public DateTime FechaVenta { get; set; }
        public string TipoVenta { get; set; }
        public string Descripcion { get; set; }
        public decimal Importe { get; set; }
    }
}
