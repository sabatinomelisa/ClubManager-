using BE;
using System;
using System.Collections.Generic;
using System.Data;
<<<<<<< HEAD
using System.Data.SqlClient;
=======
using System.Linq;
using System.Text;
using System.Threading.Tasks;
>>>>>>> origin/main

namespace DAL
{
    public class IdiomaDAL
    {
        public List<IdiomaBE> Listar()
        {
            Acceso acceso = new Acceso();
            List<IdiomaBE> idiomas = new List<IdiomaBE>();

            acceso.Conectar();
<<<<<<< HEAD
            DataTable respuesta = acceso.Leer("ConsultaIdiomas");
=======

            string sql = "ConsultaIdiomas";
            
            DataTable respuesta = new DataTable();

            respuesta = acceso.Leer(sql);
>>>>>>> origin/main

            foreach (DataRow row in respuesta.Rows)
            {
                IdiomaBE idioma = new IdiomaBE();
<<<<<<< HEAD
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
=======
                idioma.Id =int.Parse( row["Id"].ToString());
                idioma.Nombre = row["NombreIdioma"].ToString();
                idiomas.Add(idioma);

            }

            acceso.Desconectar();

            return idiomas;
        }
>>>>>>> origin/main
    }
}
