using BE;
using DAL;
using SERVICIOS;
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

        public List<RolBE> ListarRoles()
        {
            return rolDAL.ListarRoles();
        }

        public RolBE ObtenerRolConComponentes(int idRol)
        {
            RolBE rol = rolDAL.ObtenerRol(idRol);
            string nombreUsuario = "Sistema";

            if (SessionManager.SesionIniciada)
            {
                nombreUsuario = SessionManager.ObtenerUsuarioActual().Username;
            }

            bitacoraBLL.Registrar(nombreUsuario, "Consulta", "Perfiles", "Se consultó el árbol de permisos del rol " + rol.Nombre);
            return rol;
        }

        public bool TienePermiso(RolBE rol, string nombrePermiso)
        {
            if (rol == null || string.IsNullOrWhiteSpace(nombrePermiso))
            {
                return false;
            }

            return BuscarPermisoEnComponente(rol, nombrePermiso);
        }

        public List<ComponenteBE> ObtenerComponentesPlanos(RolBE rol)
        {
            List<ComponenteBE> componentesPlanos = new List<ComponenteBE>();

            if (rol != null)
            {
                AgregarComponentesRecursivos(rol, componentesPlanos);
            }

            return componentesPlanos;
        }

        private bool BuscarPermisoEnComponente(ComponenteBE componente, string nombrePermiso)
        {
            if (!componente.EsRol)
            {
                return string.Equals(componente.Nombre, nombrePermiso, StringComparison.OrdinalIgnoreCase);
            }

            foreach (ComponenteBE componenteHijo in componente.ObtenerComponentes())
            {
                if (BuscarPermisoEnComponente(componenteHijo, nombrePermiso))
                {
                    return true;
                }
            }

            return false;
        }

        private void AgregarComponentesRecursivos(ComponenteBE componente, List<ComponenteBE> componentesPlanos)
        {
            foreach (ComponenteBE componenteHijo in componente.ObtenerComponentes())
            {
                componentesPlanos.Add(componenteHijo);
                AgregarComponentesRecursivos(componenteHijo, componentesPlanos);
            }
        }
    }
}
