using System;
using System.Data;
using DAL;

namespace BLL
{
    public class ModuloClubBLL
    {
        private readonly ModuloClubDAL moduloClubDAL;
        private readonly BitacoraBLL bitacoraBLL;

        public ModuloClubBLL()
        {
            moduloClubDAL = new ModuloClubDAL();
            bitacoraBLL = new BitacoraBLL();
        }

        public DataTable ConsultarPagos()
        {
            return moduloClubDAL.Consultar("ConsultarPagos");
        }

        public DataTable ConsultarJugadores()
        {
            return moduloClubDAL.Consultar("ConsultarJugadores");
        }

        public DataTable ConsultarEventos()
        {
            return moduloClubDAL.Consultar("ConsultarEventosDeportivos");
        }

        public DataTable ConsultarMovimientosFinancieros()
        {
            return moduloClubDAL.Consultar("ConsultarMovimientosFinancieros");
        }

        public DataTable ConsultarPublicaciones()
        {
            return moduloClubDAL.Consultar("ConsultarPublicaciones");
        }

        public DataTable ConsultarInsignias()
        {
            return moduloClubDAL.Consultar("ConsultarInsignias");
        }

        public DataTable ConsultarReportes()
        {
            return moduloClubDAL.Consultar("ConsultarReportesClub");
        }

        public int RegistrarPago(int idSocio, DateTime fechaPago, string concepto, decimal importe, string estado, string usuario)
        {
            ValidarTexto(concepto, "concepto");
            int resultado = moduloClubDAL.RegistrarPago(idSocio, fechaPago, concepto, importe, estado);
            bitacoraBLL.RegistrarAlta(usuario, "Pagos", "Registro de pago/cuota para socio " + idSocio + ".");
            return resultado;
        }

        public int RegistrarJugador(int idSocio, string deporte, string posicion, string disponible, string usuario)
        {
            ValidarTexto(deporte, "deporte");
            int resultado = moduloClubDAL.RegistrarJugador(idSocio, deporte, posicion, disponible);
            bitacoraBLL.RegistrarAlta(usuario, "Jugadores", "Registro de jugador para socio " + idSocio + ".");
            return resultado;
        }

        public int RegistrarEvento(string nombre, string deporte, DateTime fechaEvento, string lugar, string estado, string usuario)
        {
            ValidarTexto(nombre, "nombre");
            int resultado = moduloClubDAL.RegistrarEvento(nombre, deporte, fechaEvento, lugar, estado);
            bitacoraBLL.RegistrarAlta(usuario, "Eventos", "Registro de evento deportivo " + nombre + ".");
            return resultado;
        }

        public int RegistrarMovimientoFinanciero(DateTime fechaMovimiento, string tipoMovimiento, string concepto, decimal importe, string usuario)
        {
            ValidarTexto(concepto, "concepto");
            int resultado = moduloClubDAL.RegistrarMovimientoFinanciero(fechaMovimiento, tipoMovimiento, concepto, importe);
            bitacoraBLL.RegistrarAlta(usuario, "Finanzas", "Registro de movimiento financiero.");
            return resultado;
        }

        public int RegistrarPublicacion(string titulo, string contenido, string tipoPublicacion, string usuarioAutor)
        {
            ValidarTexto(titulo, "título");
            int resultado = moduloClubDAL.RegistrarPublicacion(titulo, contenido, tipoPublicacion, usuarioAutor);
            bitacoraBLL.RegistrarAlta(usuarioAutor, "Comunicación", "Registro de publicación " + titulo + ".");
            return resultado;
        }

        public int RegistrarInsignia(int idSocio, string nombre, string motivo, string usuario)
        {
            ValidarTexto(nombre, "nombre");
            int resultado = moduloClubDAL.RegistrarInsignia(idSocio, nombre, motivo);
            bitacoraBLL.RegistrarAlta(usuario, "Insignias", "Otorgamiento de insignia a socio " + idSocio + ".");
            return resultado;
        }

        private void ValidarTexto(string valor, string campo)
        {
            if (string.IsNullOrWhiteSpace(valor))
            {
                throw new Exception("Ingresar " + campo + ".");
            }
        }
    }
}
