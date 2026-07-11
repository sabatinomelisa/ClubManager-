using System;
using System.Collections.Generic;
using BE;
using DAL;

namespace BLL
{
    public class ControlCambioBLL
    {
        private readonly ControlCambioDAL controlCambioDAL;
        private readonly SocioDAL socioDAL;
        private readonly BitacoraBLL bitacoraBLL;
        private readonly IntegridadBLL integridadBLL;

        public ControlCambioBLL()
        {
            controlCambioDAL = new ControlCambioDAL();
            socioDAL = new SocioDAL();
            bitacoraBLL = new BitacoraBLL();
            integridadBLL = new IntegridadBLL();
        }

        public void RegistrarMailInicialSocio(SocioBE socio, string usuario)
        {
            if (socio == null || socio.IdSocio <= 0 || string.IsNullOrWhiteSpace(socio.Mail))
            {
                return;
            }

            RegistrarMailHistorico(socio.IdSocio, socio.Mail);
            bitacoraBLL.Registrar(usuario, "HISTORIAL_MAIL", "Socios", "Se registró el mail inicial del socio " + socio.IdSocio + ".");
        }

        public void RegistrarCambioMailSocio(SocioBE socioAnterior, SocioBE socioNuevo, string usuario)
        {
            if (socioAnterior == null || socioNuevo == null)
            {
                return;
            }

            string mailAnterior = socioAnterior.Mail == null ? string.Empty : socioAnterior.Mail.Trim();
            string mailNuevo = socioNuevo.Mail == null ? string.Empty : socioNuevo.Mail.Trim();

            if (string.Equals(mailAnterior, mailNuevo, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            RegistrarMailHistorico(socioAnterior.IdSocio, mailAnterior);
            bitacoraBLL.Registrar(usuario, "CAMBIO_MAIL", "Socios", "Se registró cambio de mail del socio " + socioAnterior.IdSocio + ". Mail anterior: " + mailAnterior + ".");
        }

        public List<HistorialBE> ListarHistorialMailSocio(int? idSocio)
        {
            return controlCambioDAL.ListarHistorialMailSocio(idSocio);
        }

        public void VolverAlMailHistorico(int idSocio, int idHistorico, string usuario)
        {
            HistorialBE historialMail = controlCambioDAL.ObtenerHistorialMailSocio(idSocio, idHistorico);

            if (historialMail == null)
            {
                throw new Exception("No se encontró el mail histórico seleccionado.");
            }

            SocioBE socioActual = socioDAL.ObtenerSocio(idSocio);

            if (socioActual == null)
            {
                throw new Exception("No se encontró el socio seleccionado.");
            }

            string mailActual = socioActual.Mail == null ? string.Empty : socioActual.Mail.Trim();
            string mailHistorico = historialMail.Mail == null ? string.Empty : historialMail.Mail.Trim();

            if (string.Equals(mailActual, mailHistorico, StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception("El socio ya tiene asignado ese mail.");
            }

            controlCambioDAL.RestaurarMailHistorico(idSocio, mailActual, mailHistorico);
            integridadBLL.RecalcularIntegridad();
            bitacoraBLL.Registrar(usuario, "RESTAURAR_MAIL", "Control de Cambios", "Se restauró el mail histórico del socio " + idSocio + ".");
        }

        private void RegistrarMailHistorico(int idSocio, string mail)
        {
            if (idSocio <= 0 || string.IsNullOrWhiteSpace(mail))
            {
                return;
            }

            string mailNormalizado = mail.Trim();

            List<HistorialBE> historialExistente = controlCambioDAL.ListarHistorialMailSocio(idSocio);
            if (historialExistente.Count > 0)
            {
                string ultimoMailHistorico = historialExistente[0].Mail == null ? string.Empty : historialExistente[0].Mail.Trim();

                if (string.Equals(ultimoMailHistorico, mailNormalizado, StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }
            }

            HistorialBE historial = new HistorialBE();
            historial.IdSocio = idSocio;
            historial.Mail = mailNormalizado;
            controlCambioDAL.RegistrarMailHistorico(historial);
        }
    }
}
