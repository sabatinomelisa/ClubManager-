using System;

namespace BE
{
    public class PagoBE
    {
        public int IdPago { get; set; }
        public int IdSocio { get; set; }
        public DateTime FechaPago { get; set; }
        public string Concepto { get; set; }
        public decimal Importe { get; set; }
        public string Estado { get; set; }
    }
}
