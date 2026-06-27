using System;

namespace BE
{
    public class PublicacionBE
    {
        public int IdPublicacion { get; set; }
        public string Titulo { get; set; }
        public string Contenido { get; set; }
        public string TipoPublicacion { get; set; }
        public DateTime FechaPublicacion { get; set; }
        public string UsuarioAutor { get; set; }
    }
}
