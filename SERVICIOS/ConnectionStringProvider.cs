using System.Configuration;

namespace SERVICIOS
{
    public sealed class ConnectionStringProvider
    {
        private const string NombreConnectionString = "ClubManagerDatabase";
        private const string ConnectionStringDefault = "Data Source=.\\SQLEXPRESS;Initial Catalog=Club Manager;Integrated Security=True;TrustServerCertificate=True;";
        private static readonly ConnectionStringProvider instancia = new ConnectionStringProvider();

        private ConnectionStringProvider()
        {
        }

        public static ConnectionStringProvider Instancia
        {
            get { return instancia; }
        }

        public string ObtenerConnectionString()
        {
            ConnectionStringSettings configuracion = ConfigurationManager.ConnectionStrings[NombreConnectionString];

            if (configuracion != null && !string.IsNullOrWhiteSpace(configuracion.ConnectionString))
            {
                return configuracion.ConnectionString;
            }

            return ConnectionStringDefault;
        }
    }
}
