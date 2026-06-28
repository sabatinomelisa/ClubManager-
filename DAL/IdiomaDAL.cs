using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class IdiomaDAL
    {
        public List<IdiomaBE> Listar()
        {
            Acceso acceso = new Acceso();
            List<IdiomaBE> idiomas = new List<IdiomaBE>();

            acceso.Conectar();
            DataTable respuesta = acceso.Leer("ConsultaIdiomas");

            foreach (DataRow row in respuesta.Rows)
            {
                IdiomaBE idioma = new IdiomaBE();
                idioma.Id = int.Parse(row["Id"].ToString());
                idioma.Nombre = row["NombreIdioma"].ToString();
                idiomas.Add(idioma);
            }

            acceso.Desconectar();
            return idiomas;
        }

        public int AltaIdioma(string nombreIdioma)
        {
            Acceso acceso = new Acceso();
            acceso.Conectar();

            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>();
                parametros.Add(acceso.CrearParametro("@nombreIdioma", nombreIdioma));
                return acceso.Escribir("RegistrarIdioma", parametros);
            }
            finally
            {
                acceso.Desconectar();
            }
        }
    }
}
