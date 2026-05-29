using BE;
using DAL;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class BitacoraBLL
    {
        private readonly BitacoraDAL bitacoraDAL;

        public BitacoraBLL()
        {
            bitacoraDAL = new BitacoraDAL();
        }

        public void Registrar(string usuario, string accion, string modulo, string descripcion)
        {
            if (string.IsNullOrWhiteSpace(accion))
            {
                throw new ArgumentException("La acción es obligatoria.", "accion");
            }

            if (string.IsNullOrWhiteSpace(modulo))
            {
                throw new ArgumentException("El módulo es obligatorio.", "modulo");
            }

            BitacoraBE bitacora = new BitacoraBE
            {
                Fecha = DateTime.Now,
                Usuario = usuario,
                Accion = accion.Trim().ToUpper(),
                Modulo = modulo.Trim(),
                Descripcion = descripcion
            };

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

        public void RegistrarError(string usuario, string modulo, Exception exception)
        {
            string mensaje = exception == null ? "Error no especificado." : exception.Message;
            Registrar(usuario, "ERROR", modulo, mensaje);
        }

        public List<BitacoraBE> Listar()
        {
            return bitacoraDAL.Listar();
        }
    }
}
