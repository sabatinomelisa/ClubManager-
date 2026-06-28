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

        public DataTable ConsultarPagos() { return moduloClubDAL.Consultar("ConsultarPagos"); }
        public DataTable ConsultarJugadores() { return moduloClubDAL.Consultar("ConsultarJugadores"); }
        public DataTable ConsultarEventos() { return moduloClubDAL.Consultar("ConsultarEventosDeportivos"); }
        public DataTable ConsultarMovimientosFinancieros() { return moduloClubDAL.Consultar("ConsultarMovimientosFinancieros"); }
        public DataTable ConsultarPublicaciones() { return moduloClubDAL.Consultar("ConsultarPublicaciones"); }
        public DataTable ConsultarInsignias() { return moduloClubDAL.Consultar("ConsultarInsignias"); }
        public DataTable ConsultarInventario() { return moduloClubDAL.Consultar("ConsultarInventario"); }
        public DataTable ConsultarVentas() { return moduloClubDAL.Consultar("ConsultarVentas"); }
        public DataTable ConsultarConvocatorias() { return moduloClubDAL.Consultar("ConsultarConvocatoriasEvento"); }
        public DataTable ConsultarResultadosPartidos() { return moduloClubDAL.Consultar("ConsultarResultadosPartidos"); }
        public DataTable ConsultarReportes() { return moduloClubDAL.Consultar("ConsultarReportesClub"); }
        public DataTable ConsultarInsigniasCalculadasSocio(int idSocio) { return moduloClubDAL.ConsultarInsigniasCalculadasSocio(idSocio); }
        public DataTable ConsultarAsistenciasSocio(int idSocio) { return moduloClubDAL.ConsultarAsistenciasSocio(idSocio); }
        public DataTable ConsultarConfiguracionCuotas() { return moduloClubDAL.ConsultarConfiguracionCuotas(); }

        public DataTable ObtenerCuotaSocio(int idSocio)
        {
            return moduloClubDAL.ObtenerCuotaSocio(idSocio);
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

        public int RegistrarEvento(string nombre, string deporte, DateTime fechaEvento, string lugar, string estado, string usuario, int cupoEspectadores, decimal precioEntradaEspectador, decimal precioParticipacionJugador)
        {
            ValidarTexto(nombre, "nombre");
            ValidarTexto(deporte, "deporte");
            if (cupoEspectadores < 0) throw new Exception("El cupo de espectadores no puede ser negativo.");
            if (precioEntradaEspectador < 0) throw new Exception("El precio de entrada no puede ser negativo.");
            if (precioParticipacionJugador < 0) throw new Exception("El precio de participación no puede ser negativo.");
            int resultado = moduloClubDAL.RegistrarEvento(nombre, deporte, fechaEvento, lugar, estado, cupoEspectadores, precioEntradaEspectador, precioParticipacionJugador);
            bitacoraBLL.RegistrarAlta(usuario, "Eventos", "Registro de evento deportivo " + nombre + ".");
            return resultado;
        }

        public int RegistrarEvento(string nombre, string deporte, DateTime fechaEvento, string lugar, string estado, string usuario)
        {
            return RegistrarEvento(nombre, deporte, fechaEvento, lugar, estado, usuario, 100, 3000, 2000);
        }

        public int RegistrarMovimientoFinanciero(DateTime fechaMovimiento, string tipoMovimiento, string concepto, decimal importe, string usuario)
        {
            ValidarTexto(concepto, "concepto");
            ValidarTexto(tipoMovimiento, "tipo de movimiento");
            int resultado = moduloClubDAL.RegistrarMovimientoFinanciero(fechaMovimiento, tipoMovimiento.ToUpper(), concepto, importe);
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

        public int RegistrarInventario(string nombre, int cantidad, string ubicacion, string estado, string usuario)
        {
            ValidarTexto(nombre, "nombre de artículo");
            if (cantidad < 0) throw new Exception("La cantidad no puede ser negativa.");
            int resultado = moduloClubDAL.RegistrarInventario(nombre, cantidad, ubicacion, estado);
            bitacoraBLL.RegistrarAlta(usuario, "Inventario", "Registro de artículo de inventario " + nombre + ".");
            return resultado;
        }

        public int RegistrarVenta(DateTime fechaVenta, string tipoVenta, string descripcion, decimal importe, string usuario)
        {
            ValidarTexto(tipoVenta, "tipo de venta");
            ValidarTexto(descripcion, "descripción");
            int resultado = moduloClubDAL.RegistrarVenta(fechaVenta, tipoVenta.ToUpper(), descripcion, importe);
            bitacoraBLL.RegistrarAlta(usuario, "Ventas", "Registro de venta " + tipoVenta + ".");
            return resultado;
        }

        public int RegistrarConvocatoria(int idEvento, int idJugador, string estadoRespuesta, string usuario)
        {
            ValidarTexto(estadoRespuesta, "estado de respuesta");
            int resultado = moduloClubDAL.RegistrarConvocatoria(idEvento, idJugador, estadoRespuesta);
            bitacoraBLL.RegistrarAlta(usuario, "Convocatorias", "Convocatoria de jugador " + idJugador + " al evento " + idEvento + ".");
            return resultado;
        }

        public int RegistrarResultadoPartido(int idEvento, string equipoLocal, string equipoVisitante, string resultadoPartido, string usuario)
        {
            ValidarTexto(equipoLocal, "equipo local");
            ValidarTexto(equipoVisitante, "equipo visitante");
            ValidarTexto(resultadoPartido, "resultado");
            int resultado = moduloClubDAL.RegistrarResultadoPartido(idEvento, equipoLocal, equipoVisitante, resultadoPartido);
            bitacoraBLL.RegistrarAlta(usuario, "Resultados", "Registro de resultado del evento " + idEvento + ".");
            return resultado;
        }

        public int RegistrarAsistenciaEventoSocio(int idSocio, int idEvento, string tipoAsistencia, string usuario)
        {
            if (idSocio <= 0) throw new Exception("No se pudo identificar el socio.");
            if (idEvento <= 0) throw new Exception("Ingresar un evento válido.");
            ValidarTexto(tipoAsistencia, "tipo de asistencia");
            string tipo = tipoAsistencia.Trim().ToUpper();
            if (tipo != "ESPECTADOR" && tipo != "PARTICIPANTE")
            {
                throw new Exception("Tipo de asistencia inválido. Use ESPECTADOR o PARTICIPANTE.");
            }

            int resultado = moduloClubDAL.RegistrarAsistenciaEventoSocio(idSocio, idEvento, tipo);
            bitacoraBLL.RegistrarAlta(usuario, "Eventos", "Registro de asistencia " + tipo + " para socio " + idSocio + " en evento " + idEvento + ".");
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
