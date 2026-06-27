using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BE;

namespace DAL
{
    public class UsuarioDAL
    {
        public int ActPass(UsuarioBE usuario)
        {
            Acceso acceso = new Acceso();
            acceso.Conectar();
            acceso.IniciarTx();

            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>();
                parametros.Add(acceso.CrearParametro("@usu", usuario.Username));
                parametros.Add(acceso.CrearParametro("@nuevapass", usuario.Password));

                int resultado = acceso.Escribir("ActualizaPass", parametros);
                acceso.ConfirmarTx();
                return resultado;
            }
            catch (Exception exception)
            {
                acceso.CancelarTx();
                throw new Exception("Error al actualizar contraseña: " + exception.Message);
            }
            finally
            {
                acceso.Desconectar();
            }
        }

        public int AltaUsuario(UsuarioBE usuario)
        {
            Acceso acceso = new Acceso();
            acceso.Conectar();
            acceso.IniciarTx();

            try
            {
                SocioDAL socioDAL = new SocioDAL();
                int resultadoSocio = socioDAL.AltaSocio(usuario, acceso);

                if (resultadoSocio <= 0)
                {
                    acceso.CancelarTx();
                    return resultadoSocio;
                }

                List<SqlParameter> parametros = new List<SqlParameter>();
                parametros.Add(acceso.CrearParametro("@usuario", usuario.Username));
                parametros.Add(acceso.CrearParametro("@password", usuario.Password));
                parametros.Add(acceso.CrearParametro("@fechaCreacion", usuario.FechaCreacion));
                parametros.Add(acceso.CrearParametro("@id", usuario.IdSocio));
                parametros.Add(acceso.CrearParametro("@bloqueado", ObtenerValorSiNo(usuario.Bloqueado, "N")));
                parametros.Add(acceso.CrearParametro("@activo", ObtenerValorSiNo(usuario.Activo, "S")));
                parametros.Add(acceso.CrearParametro("@idRol", ObtenerIdRol(usuario)));

                int resultadoUsuario = acceso.Escribir("RegistrarUsuario", parametros);
                acceso.ConfirmarTx();
                return resultadoUsuario;
            }
            catch (Exception exception)
            {
                acceso.CancelarTx();
                throw new Exception("Error al registrar al usuario: " + exception.Message);
            }
            finally
            {
                acceso.Desconectar();
            }
        }

        public UsuarioBE DevolverUser(string usuarioIngresado, string password = null)
        {
            return ObtenerPorNombreUsuario(usuarioIngresado);
        }

        public UsuarioBE ObtenerPorNombreUsuario(string usuarioIngresado)
        {
            if (string.IsNullOrWhiteSpace(usuarioIngresado))
            {
                return null;
            }

            Acceso acceso = new Acceso();
            acceso.Conectar();

            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>();
                parametros.Add(acceso.CrearParametro("@usu", usuarioIngresado));

                DataTable tabla = acceso.Leer("ObtenerUsuarioPorNombre", parametros);

                if (tabla.Rows.Count == 0)
                {
                    return null;
                }

                return MapearUsuario(tabla.Rows[0]);
            }
            finally
            {
                acceso.Desconectar();
            }
        }

        public int IncrementarIntentosFallidos(string nombreUsuario)
        {
            return EjecutarOperacionUsuario("IncrementarIntentosFallidos", nombreUsuario);
        }

        public int ReiniciarIntentosFallidos(string nombreUsuario)
        {
            return EjecutarOperacionUsuario("ReiniciarIntentosFallidos", nombreUsuario);
        }

        public int BloquearUsuario(string nombreUsuario)
        {
            return EjecutarOperacionUsuario("BloquearUsuario", nombreUsuario);
        }

        public int DesbloquearUsuario(string nombreUsuario)
        {
            return EjecutarOperacionUsuario("DesbloquearUsuario", nombreUsuario);
        }

        public int ActivarUsuario(string nombreUsuario)
        {
            return EjecutarOperacionUsuario("ActivarUsuario", nombreUsuario);
        }

        public int DesactivarUsuario(string nombreUsuario)
        {
            return EjecutarOperacionUsuario("DesactivarUsuario", nombreUsuario);
        }

        private int EjecutarOperacionUsuario(string procedimiento, string nombreUsuario)
        {
            Acceso acceso = new Acceso();
            acceso.Conectar();
            acceso.IniciarTx();

            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>();
                parametros.Add(acceso.CrearParametro("@usu", nombreUsuario));

                int resultado = acceso.Escribir(procedimiento, parametros);
                acceso.ConfirmarTx();
                return resultado;
            }
            catch
            {
                acceso.CancelarTx();
                throw;
            }
            finally
            {
                acceso.Desconectar();
            }
        }

        private UsuarioBE MapearUsuario(DataRow fila)
        {
            UsuarioBE usuario = new UsuarioBE();
            usuario.IdSocio = Convert.ToInt32(fila["IdSocio"]);
            usuario.Username = fila["NombreUsuario"].ToString();
            usuario.Password = fila["Contraseña"].ToString();
            usuario.FechaCreacion = Convert.ToDateTime(fila["FechaCreacion"]);
            usuario.Bloqueado = fila["Bloqueado"].ToString();
            usuario.Activo = fila["Activo"].ToString();
            usuario.IntentosFallidos = Convert.ToInt32(fila["IntentosFallidos"]);

            if (fila.Table.Columns.Contains("TipoDocumento") && fila["TipoDocumento"] != DBNull.Value)
            {
                usuario.TipoDocumento = fila["TipoDocumento"].ToString();
            }

            if (fila.Table.Columns.Contains("NumeroDocumento") && fila["NumeroDocumento"] != DBNull.Value)
            {
                usuario.NumeroDocumento = Convert.ToInt32(fila["NumeroDocumento"]);
            }

            if (fila.Table.Columns.Contains("Nombre") && fila["Nombre"] != DBNull.Value)
            {
                usuario.Nombre = fila["Nombre"].ToString();
            }

            if (fila.Table.Columns.Contains("Apellido") && fila["Apellido"] != DBNull.Value)
            {
                usuario.Apellido = fila["Apellido"].ToString();
            }

            if (fila.Table.Columns.Contains("Email") && fila["Email"] != DBNull.Value)
            {
                usuario.Mail = fila["Email"].ToString();
            }

            if (fila.Table.Columns.Contains("IdRol") && fila["IdRol"] != DBNull.Value)
            {
                usuario.Rol = new RolBE();
                usuario.Rol.Id = Convert.ToInt32(fila["IdRol"]);

                if (fila.Table.Columns.Contains("NombreRol") && fila["NombreRol"] != DBNull.Value)
                {
                    usuario.Rol.Nombre = fila["NombreRol"].ToString();
                }
            }

            return usuario;
        }

        private string ObtenerValorSiNo(string valor, string valorDefault)
        {
            if (string.IsNullOrWhiteSpace(valor))
            {
                return valorDefault;
            }

            return valor.Trim().ToUpper() == "S" ? "S" : "N";
        }

        private int ObtenerIdRol(UsuarioBE usuario)
        {
            if (usuario.Rol != null && usuario.Rol.Id > 0)
            {
                return usuario.Rol.Id;
            }

            return 2;
        }
    }
}
