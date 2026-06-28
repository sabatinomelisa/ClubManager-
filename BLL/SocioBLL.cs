using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using BE;
using DAL;

namespace BLL
{
    public class SocioBLL
    {
        private readonly SocioDAL socioDAL;
        private readonly ControlCambioBLL controlCambioBLL;
        private readonly BitacoraBLL bitacoraBLL;
        private readonly IntegridadBLL integridadBLL;

        public SocioBLL()
        {
            socioDAL = new SocioDAL();
            controlCambioBLL = new ControlCambioBLL();
            bitacoraBLL = new BitacoraBLL();
            integridadBLL = new IntegridadBLL();
        }

        public List<SocioBE> ListarSocios(bool incluirInactivos)
        {
            return socioDAL.ListarSocios(incluirInactivos);
        }

        public SocioBE ObtenerSocio(int idSocio)
        {
            return socioDAL.ObtenerSocio(idSocio);
        }

        public int AltaSocio(SocioBE socio, string usuario)
        {
            ValidarSocio(socio);
            socio.Activo = "S";
            int resultado = socioDAL.AltaSocio(socio);
            if (resultado > 0)
            {
                integridadBLL.RecalcularIntegridad();
            }
            controlCambioBLL.RegistrarMailInicialSocio(socio, usuario);
            bitacoraBLL.RegistrarAlta(usuario, "Socios", "Alta de socio " + socio.NombreCompleto + ".");
            return resultado;
        }

        public int ModificarSocio(SocioBE socio, string usuario)
        {
            ValidarSocio(socio);
            SocioBE socioAnterior = socioDAL.ObtenerSocio(socio.IdSocio);

            if (socioAnterior == null)
            {
                throw new Exception("No se encontró el socio a modificar.");
            }

            controlCambioBLL.RegistrarCambioMailSocio(socioAnterior, socio, usuario);
            int resultado = socioDAL.ModificarSocio(socio);
            if (resultado > 0)
            {
                integridadBLL.RecalcularIntegridad();
            }
            bitacoraBLL.RegistrarModificacion(usuario, "Socios", "Modificación de socio " + socio.IdSocio + ".");
            return resultado;
        }

        public int BajaSocio(int idSocio, string usuario)
        {
            SocioBE socioAnterior = socioDAL.ObtenerSocio(idSocio);

            if (socioAnterior == null)
            {
                throw new Exception("No se encontró el socio a dar de baja.");
            }

            int resultado = socioDAL.CambiarEstadoSocio(idSocio, "N");
            if (resultado > 0)
            {
                integridadBLL.RecalcularIntegridad();
            }
            bitacoraBLL.RegistrarBaja(usuario, "Socios", "Baja lógica de socio " + idSocio + ".");
            return resultado;
        }

        private void ValidarSocio(SocioBE socio)
        {
            if (socio == null)
            {
                throw new Exception("Ingresar datos del socio.");
            }

            if (string.IsNullOrWhiteSpace(socio.TipoDocumento))
            {
                throw new Exception("Ingresar tipo de documento.");
            }

            if (socio.NumeroDocumento <= 0)
            {
                throw new Exception("Ingresar número de documento válido.");
            }

            if (string.IsNullOrWhiteSpace(socio.Nombre))
            {
                throw new Exception("Ingresar nombre.");
            }

            if (string.IsNullOrWhiteSpace(socio.Apellido))
            {
                throw new Exception("Ingresar apellido.");
            }

            if (socio.FechaNacimiento == DateTime.MinValue)
            {
                throw new Exception("Ingresar fecha de nacimiento.");
            }

            if (string.IsNullOrWhiteSpace(socio.Nacionalidad))
            {
                throw new Exception("Ingresar nacionalidad.");
            }

            if (string.IsNullOrWhiteSpace(socio.Mail) || !Regex.IsMatch(socio.Mail, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                throw new Exception("Ingresar un mail válido.");
            }

            if (socio.Telefono <= 0)
            {
                throw new Exception("Ingresar teléfono válido.");
            }

            if (string.IsNullOrWhiteSpace(socio.Activo))
            {
                socio.Activo = "S";
            }
        }
    }
}
