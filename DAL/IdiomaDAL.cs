using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class IdiomaDAL
    {
        public List<IdiomaBE> Listar()
        {
            Acceso acceso = new Acceso();
            List<IdiomaBE> idiomas = new List<IdiomaBE>();

            acceso.Conectar();

            string sql = "ConsultaIdiomas";
            
            DataTable respuesta = new DataTable();

            respuesta = acceso.Leer(sql);

            foreach (DataRow row in respuesta.Rows)
            {
                IdiomaBE idioma = new IdiomaBE();
                idioma.Id =int.Parse( row["Id"].ToString());
                idioma.Nombre = row["NombreIdioma"].ToString();
                idiomas.Add(idioma);

            }

            acceso.Desconectar();

            return idiomas;
        }
    }
}
