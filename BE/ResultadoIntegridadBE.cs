namespace BE
{
    public class ResultadoIntegridadBE
    {
        public string Entidad { get; set; }
        public string Identificador { get; set; }
        public string TipoDigito { get; set; }
        public int ValorEsperado { get; set; }
        public int ValorActual { get; set; }
        public bool Correcto { get; set; }
        public string Mensaje { get; set; }
    }
}
