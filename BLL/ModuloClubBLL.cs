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
        public DataTable ConsultarCatalogoInsignias() { return moduloClubDAL.Consultar("ConsultarCatalogoInsignias"); }
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


        public int ResolverIdSocio(string socioOUsuario)
        {
            ValidarTexto(socioOUsuario, "número de socio o usuario");
            int idSocio = moduloClubDAL.ResolverIdSocio(socioOUsuario.Trim());
            if (idSocio <= 0)
            {
                throw new Exception("No se encontró un socio con ese número o usuario. Verifique el dato ingresado.");
            }
            return idSocio;
        }

        public string ObtenerRolSocio(int idSocio)
        {
            if (idSocio <= 0) throw new Exception("Ingresar un socio válido.");
            string rol = moduloClubDAL.ObtenerRolSocio(idSocio);
            return string.IsNullOrWhiteSpace(rol) ? "Sin usuario activo" : rol;
        }

        public void ValidarSocioPlenoParaInsignia(int idSocio)
        {
            string rol = ObtenerRolSocio(idSocio);
            if (!string.Equals(rol, "Socio Pleno", StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception("Solo se pueden asignar insignias a Socio Pleno. El Socio Simple no participa de insignias deportivas.");
            }
        }

        public int GuardarConfiguracionGeneral(int idConfiguracion, decimal importe, string activo, string usuario)
        {
            if (idConfiguracion <= 0) throw new Exception("Seleccione una configuración válida de la tabla.");
            if (importe < 0) throw new Exception("El importe no puede ser negativo.");
            string valorActivo = string.IsNullOrWhiteSpace(activo) ? "S" : activo.Trim().Substring(0, 1).ToUpper();
            if (valorActivo != "S" && valorActivo != "N") throw new Exception("Activo debe ser S o N.");
            int resultado = moduloClubDAL.GuardarConfiguracionGeneral(idConfiguracion, importe, valorActivo);
            bitacoraBLL.RegistrarModificacion(usuario, "Configuración", "Actualización de cuota/fee " + idConfiguracion + ".");
            return resultado;
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

        public int ActualizarEvento(int idEvento, string nombre, string deporte, DateTime fechaEvento, string lugar, string estado, int cupoEspectadores, decimal precioEntradaEspectador, decimal precioParticipacionJugador, string usuario)
        {
            if (idEvento <= 0) throw new Exception("Seleccione un evento válido para modificar.");
            ValidarTexto(nombre, "nombre");
            ValidarTexto(deporte, "deporte");
            if (cupoEspectadores < 0) throw new Exception("El cupo de espectadores no puede ser negativo.");
            if (precioEntradaEspectador < 0) throw new Exception("El precio de entrada no puede ser negativo.");
            if (precioParticipacionJugador < 0) throw new Exception("El precio de participación no puede ser negativo.");
            int resultado = moduloClubDAL.ActualizarEvento(idEvento, nombre, deporte, fechaEvento, lugar, estado, cupoEspectadores, precioEntradaEspectador, precioParticipacionJugador);
            bitacoraBLL.RegistrarModificacion(usuario, "Eventos", "Actualización de evento deportivo " + idEvento + " - " + nombre + ".");
            return resultado;
        }

        public int RegistrarMovimientoFinanciero(DateTime fechaMovimiento, string tipoMovimiento, string concepto, decimal importe, string usuario)
        {
            ValidarTexto(concepto, "concepto");
            ValidarTexto(tipoMovimiento, "tipo de movimiento");
            string tipo = tipoMovimiento.Trim().ToUpper();
            if (tipo != "INGRESO" && tipo != "EGRESO")
            {
                throw new Exception("Seleccione un tipo de movimiento válido: INGRESO o EGRESO.");
            }
            int resultado = moduloClubDAL.RegistrarMovimientoFinanciero(fechaMovimiento, tipo, concepto, Math.Abs(importe));
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

        public int ActualizarPublicacion(int idPublicacion, string titulo, string contenido, string tipoPublicacion, string usuario)
        {
            if (idPublicacion <= 0) throw new Exception("Seleccione una comunicación existente.");
            ValidarTexto(titulo, "título");
            int resultado = moduloClubDAL.ActualizarPublicacion(idPublicacion, titulo, contenido, tipoPublicacion);
            bitacoraBLL.RegistrarModificacion(usuario, "Comunicación", "Actualización de publicación " + titulo + ".");
            return resultado;
        }

        public int RegistrarInsignia(int idSocio, string nombre, string motivo, string usuario)
        {
            return AsignarInsigniaSocio(idSocio, nombre, 1, motivo, usuario);
        }

        public int AsignarInsigniaSocio(int idSocio, string nombre, int nivel, string motivo, string usuario)
        {
            if (idSocio <= 0) throw new Exception("Ingresar un socio válido.");
            ValidarSocioPlenoParaInsignia(idSocio);
            ValidarTexto(nombre, "insignia");
            if (nivel <= 0) throw new Exception("El nivel debe ser mayor a cero.");
            int resultado = moduloClubDAL.AsignarInsigniaSocio(idSocio, nombre, nivel, motivo);
            bitacoraBLL.RegistrarAlta(usuario, "Insignias", "Asignación/actualización de insignia " + nombre + " nivel " + nivel + " para socio " + idSocio + ".");
            return resultado;
        }

        public int GuardarInsigniaCatalogo(string nombre, string descripcion, string imagen, string tieneNiveles, string requisitoNiveles, string usuario)
        {
            ValidarTexto(nombre, "nombre de insignia");
            ValidarTexto(descripcion, "descripción de insignia");
            if (string.IsNullOrWhiteSpace(tieneNiveles)) tieneNiveles = "S";
            int resultado = moduloClubDAL.GuardarInsigniaCatalogo(nombre, descripcion, imagen, tieneNiveles, requisitoNiveles);
            bitacoraBLL.RegistrarAlta(usuario, "Insignias", "Alta/actualización de catálogo de insignia " + nombre + ".");
            return resultado;
        }

        public int ActualizarInsigniaCatalogo(int idInsigniaCatalogo, string nombre, string descripcion, string imagen, string tieneNiveles, string requisitoNiveles, string usuario)
        {
            if (idInsigniaCatalogo <= 0) throw new Exception("Seleccione una insignia existente.");
            ValidarTexto(nombre, "nombre de insignia");
            ValidarTexto(descripcion, "descripción de insignia");
            if (string.IsNullOrWhiteSpace(tieneNiveles)) tieneNiveles = "S";
            int resultado = moduloClubDAL.ActualizarInsigniaCatalogo(idInsigniaCatalogo, nombre, descripcion, imagen, tieneNiveles, requisitoNiveles);
            bitacoraBLL.RegistrarModificacion(usuario, "Insignias", "Actualización de catálogo de insignia " + nombre + ".");
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

        public int ActualizarStockInventario(int idInventario, int cantidad, bool sumar, string usuario)
        {
            if (idInventario <= 0) throw new Exception("Seleccione un artículo válido.");
            if (cantidad <= 0) throw new Exception("La cantidad debe ser mayor a cero.");
            int resultado = moduloClubDAL.ActualizarStockInventario(idInventario, cantidad, sumar ? "SUMAR" : "RESTAR");
            bitacoraBLL.RegistrarModificacion(usuario, "Inventario", (sumar ? "Suma" : "Resta") + " de stock del artículo " + idInventario + ".");
            return resultado;
        }

        public int RegistrarVenta(DateTime fechaVenta, string tipoVenta, string descripcion, decimal importe, string usuario)
        {
            ValidarTexto(tipoVenta, "tipo de venta");
            ValidarTexto(descripcion, "descripción");
            if (importe <= 0) throw new Exception("El importe de la venta debe ser mayor a cero.");
            int resultado = moduloClubDAL.RegistrarVenta(fechaVenta, tipoVenta.ToUpper(), descripcion, importe);
            moduloClubDAL.RegistrarMovimientoFinanciero(fechaVenta, "INGRESO", "Venta " + tipoVenta.ToUpper() + " - " + descripcion, importe);
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
