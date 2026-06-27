using System;
using System.Globalization;
using BE;

namespace SERVICIOS
{
    public class SerializadorSimple
    {
        private const char Separador = '|';

        public string SerializarSocio(SocioBE socio)
        {
            if (socio == null)
            {
                return string.Empty;
            }

            string[] valores = new string[]
            {
                socio.IdSocio.ToString(CultureInfo.InvariantCulture),
                Limpiar(socio.TipoDocumento),
                socio.NumeroDocumento.ToString(CultureInfo.InvariantCulture),
                Limpiar(socio.Nombre),
                Limpiar(socio.Apellido),
                socio.FechaNacimiento.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                Limpiar(socio.Nacionalidad),
                Limpiar(socio.Mail),
                socio.Telefono.ToString(CultureInfo.InvariantCulture),
                Limpiar(string.IsNullOrWhiteSpace(socio.Activo) ? "S" : socio.Activo),
                socio.DigitoVerificadorHorizontal.ToString(CultureInfo.InvariantCulture)
            };

            return string.Join(Separador.ToString(), valores);
        }

        public SocioBE DeserializarSocio(string estadoSerializado)
        {
            if (string.IsNullOrWhiteSpace(estadoSerializado))
            {
                throw new ArgumentException("El estado anterior no puede estar vacío.");
            }

            string[] valores = estadoSerializado.Split(Separador);

            if (valores.Length < 10)
            {
                throw new ArgumentException("El estado anterior del socio tiene un formato inválido.");
            }

            SocioBE socio = new SocioBE();
            socio.IdSocio = Convert.ToInt32(valores[0], CultureInfo.InvariantCulture);
            socio.TipoDocumento = valores[1];
            socio.NumeroDocumento = Convert.ToInt32(valores[2], CultureInfo.InvariantCulture);
            socio.Nombre = valores[3];
            socio.Apellido = valores[4];
            socio.FechaNacimiento = DateTime.ParseExact(valores[5], "yyyy-MM-dd", CultureInfo.InvariantCulture);
            socio.Nacionalidad = valores[6];
            socio.Mail = valores[7];
            socio.Telefono = Convert.ToInt32(valores[8], CultureInfo.InvariantCulture);
            socio.Activo = valores[9];

            if (valores.Length > 10)
            {
                socio.DigitoVerificadorHorizontal = Convert.ToInt32(valores[10], CultureInfo.InvariantCulture);
            }

            return socio;
        }

        private string Limpiar(string valor)
        {
            if (valor == null)
            {
                return string.Empty;
            }

            return valor.Replace(Separador.ToString(), "/").Trim();
        }
    }
}
