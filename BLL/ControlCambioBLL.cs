using System;
using System.Collections.Generic;
using BE;
using DAL;
using SERVICIOS;

namespace BLL
{
    public class ControlCambioBLL
    {
        private readonly ControlCambioDAL controlCambioDAL;
        private readonly SocioDAL socioDAL;
        private readonly SerializadorSimple serializadorSimple;
        private readonly BitacoraBLL bitacoraBLL;
        private readonly IntegridadBLL integridadBLL;

        public ControlCambioBLL()
        {
            controlCambioDAL = new ControlCambioDAL();
            socioDAL = new SocioDAL();
            serializadorSimple = new SerializadorSimple();
            bitacoraBLL = new BitacoraBLL();
            integridadBLL = new IntegridadBLL();
        }

        public void RegistrarCambioSocio(SocioBE estadoAnterior, SocioBE estadoNuevo, string usuario, string accion, string descripcion)
        {
            CambioSocioBE cambioSocio = new CambioSocioBE();
            cambioSocio.IdSocio = estadoNuevo != null ? estadoNuevo.IdSocio : estadoAnterior.IdSocio;
            cambioSocio.Usuario = string.IsNullOrWhiteSpace(usuario) ? "SIN_SESION" : usuario;
            cambioSocio.Accion = accion;
            cambioSocio.EstadoAnterior = serializadorSimple.SerializarSocio(estadoAnterior);
            cambioSocio.EstadoNuevo = serializadorSimple.SerializarSocio(estadoNuevo);
            cambioSocio.Descripcion = descripcion;

            controlCambioDAL.RegistrarCambioSocio(cambioSocio);
        }

        public List<CambioSocioBE> ListarCambiosSocio(int? idSocio)
        {
            return controlCambioDAL.ListarCambiosSocio(idSocio);
        }

        public void RecomponerEstadoAnterior(int idCambioSocio, string usuario)
        {
            CambioSocioBE cambioSocio = controlCambioDAL.ObtenerCambioSocio(idCambioSocio);

            if (cambioSocio == null)
            {
                throw new Exception("No se encontró el cambio seleccionado.");
            }

            SocioBE estadoAnterior = serializadorSimple.DeserializarSocio(cambioSocio.EstadoAnterior);
            SocioBE estadoActual = socioDAL.ObtenerSocio(estadoAnterior.IdSocio);

            socioDAL.ModificarSocio(estadoAnterior);
            integridadBLL.RecalcularIntegridad();
            RegistrarCambioSocio(estadoActual, estadoAnterior, usuario, "RECOMPOSICION", "Recomposición desde historial de cambios.");
            bitacoraBLL.Registrar(usuario, "RECOMPOSICION", "Control de Cambios", "Se recompuso el socio " + estadoAnterior.IdSocio + " desde el cambio " + idCambioSocio + ".");
        }
    }
}
