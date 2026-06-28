using DAL;
using SERVICIOS;
using SERVICIOS.Composite;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class RolBLL
    {
        private readonly RolDAL rolDAL;
        private readonly BitacoraBLL bitacoraBLL;

        public RolBLL()
        {
            rolDAL = new RolDAL();
            bitacoraBLL = new BitacoraBLL();
        }

        public List<Rol> ListarRoles()
        {
            return rolDAL.ListarRoles();
        }

        public Rol ObtenerRolConComponentes(int idRol)
        {
            Rol rol = rolDAL.ObtenerRol(idRol);
            string nombreUsuario = "Sistema";

            if (SessionManager.SesionIniciada)
            {
                nombreUsuario = SessionManager.ObtenerUsuarioActual().Username;
            }

            bitacoraBLL.Registrar(nombreUsuario, "Consulta", "Perfiles", "Se consultó el árbol de permisos del rol " + rol.Nombre);
            return rol;
        }

        public bool TienePermiso(Rol rol, string nombrePermiso)
        {
            if (rol == null || string.IsNullOrWhiteSpace(nombrePermiso))
            {
                return false;
            }

            return BuscarPermisoEnComponente(rol, nombrePermiso);
        }

        public List<Componente> ObtenerComponentesPlanos(Rol rol)
        {
            List<Componente> componentesPlanos = new List<Componente>();

            if (rol != null)
            {
                AgregarComponentesRecursivos(rol, componentesPlanos);
            }

            return componentesPlanos;
        }

        private bool BuscarPermisoEnComponente(Componente componente, string nombrePermiso)
        {
            if (!componente.EsRol)
            {
                return string.Equals(componente.Nombre, nombrePermiso, StringComparison.OrdinalIgnoreCase);
            }

            foreach (Componente componenteHijo in componente.ObtenerComponentes())
            {
                if (BuscarPermisoEnComponente(componenteHijo, nombrePermiso))
                {
                    return true;
                }
            }

            return false;
        }

        private void AgregarComponentesRecursivos(Componente componente, List<Componente> componentesPlanos)
        {
            foreach (Componente componenteHijo in componente.ObtenerComponentes())
            {
                componentesPlanos.Add(componenteHijo);
                AgregarComponentesRecursivos(componenteHijo, componentesPlanos);
            }
        }
    }
}
