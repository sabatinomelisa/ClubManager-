using System;

namespace BE
{
    public class MovimientoFinancieroBE
    {
        public int IdMovimiento { get; set; }
        public DateTime FechaMovimiento { get; set; }
        public string TipoMovimiento { get; set; }
        public string Concepto { get; set; }
        public decimal Importe { get; set; }
    }
}
