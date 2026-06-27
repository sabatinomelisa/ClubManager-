using System;
using System.Collections.Generic;
using BE;
using DAL;

namespace BLL
{
    public class BitacoraBLL
    {
        private readonly BitacoraDAL bitacoraDAL;

        public BitacoraBLL()
        {
            bitacoraDAL = new BitacoraDAL();
        }

        public BitacoraBLL(string connectionString)
        {
            bitacoraDAL = new BitacoraDAL(connectionString);
        }

        public void Registrar(string usuario, string accion, string modulo, string descripcion)
        {
            ValidarEvento(accion, modulo);

            BitacoraBE bitacora = new BitacoraBE();
            bitacora.Usuario = usuario;
            bitacora.Accion = accion;
            bitacora.Modulo = modulo;
            bitacora.Descripcion = descripcion;

            bitacoraDAL.Registrar(bitacora);
        }

        public void RegistrarLogin(string usuario)
        {
            Registrar(usuario, "LOGIN", "Seguridad", "Usuario inició sesión correctamente.");
        }

        public void RegistrarLogout(string usuario)
        {
            Registrar(usuario, "LOGOUT", "Seguridad", "Usuario cerró sesión.");
        }

        public void RegistrarAlta(string usuario, string modulo, string descripcion)
        {
            Registrar(usuario, "ALTA", modulo, descripcion);
        }

        public void RegistrarModificacion(string usuario, string modulo, string descripcion)
        {
            Registrar(usuario, "MODIFICACION", modulo, descripcion);
        }

        public void RegistrarBaja(string usuario, string modulo, string descripcion)
        {
            Registrar(usuario, "BAJA", modulo, descripcion);
        }

        public void RegistrarError(string usuario, string modulo, Exception exception)
        {
            string descripcion = "Error no especificado.";

            if (exception != null)
            {
                descripcion = exception.Message;
            }

            Registrar(usuario, "ERROR", modulo, descripcion);
        }

        public List<BitacoraBE> Listar()
        {
            return bitacoraDAL.Listar();
        }

        public List<BitacoraBE> ListarPorUsuario(string usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario))
            {
                throw new ArgumentException("El usuario es obligatorio.");
            }

            return bitacoraDAL.ListarPorUsuario(usuario);
        }

        public List<BitacoraBE> Buscar(string usuario, string accion, string modulo, DateTime? fechaDesde, DateTime? fechaHasta)
        {
            return bitacoraDAL.Buscar(usuario, accion, modulo, fechaDesde, fechaHasta);
        }

        private void ValidarEvento(string accion, string modulo)
        {
            if (string.IsNullOrWhiteSpace(accion))
            {
                throw new ArgumentException("La acción es obligatoria.");
            }

            if (string.IsNullOrWhiteSpace(modulo))
            {
                throw new ArgumentException("El módulo es obligatorio.");
            }
        }
    }
}
