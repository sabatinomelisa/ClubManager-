using System;
using System.Collections.Generic;
using System.Globalization;
using BE;
using DAL;
using SERVICIOS;

namespace BLL
{
    public class IntegridadBLL
    {
        private const string EntidadSocio = "Socio";
        private readonly IntegridadDAL integridadDAL;
        private readonly CalculadorDigitoVerificador calculadorDigitoVerificador;

        public IntegridadBLL()
        {
            integridadDAL = new IntegridadDAL();
            calculadorDigitoVerificador = new CalculadorDigitoVerificador();
        }

        public void InicializarSiCorresponde()
        {
            if (!integridadDAL.ExistenDigitosVerticales(EntidadSocio))
            {
                RecalcularIntegridad();
            }
        }


        public void ValidarIntegridadParaLogin()
        {
            InicializarSiCorresponde();
            ValidarIntegridadObligatoria("login");
        }

        public void ActualizarIntegridadParaLogout()
        {
            RecalcularIntegridad();
            ValidarIntegridadObligatoria("logout");
        }

        private void ValidarIntegridadObligatoria(string operacion)
        {
            List<ResultadoIntegridadBE> resultados = VerificarIntegridad();

            foreach (ResultadoIntegridadBE resultado in resultados)
            {
                if (!resultado.Correcto)
                {
                    throw new Exception("Error de integridad en " + operacion + ": " + resultado.Mensaje);
                }
            }
        }

        public List<ResultadoIntegridadBE> VerificarIntegridad()
        {
            List<ResultadoIntegridadBE> resultados = new List<ResultadoIntegridadBE>();
            List<SocioBE> socios = integridadDAL.ListarSociosParaIntegridad();

            foreach (SocioBE socio in socios)
            {
                int digitoEsperado = CalcularDigitoHorizontalSocio(socio);
                bool correcto = digitoEsperado == socio.DigitoVerificadorHorizontal;

                if (!correcto)
                {
                    resultados.Add(new ResultadoIntegridadBE
                    {
                        Entidad = EntidadSocio,
                        Identificador = socio.IdSocio.ToString(CultureInfo.InvariantCulture),
                        TipoDigito = "DVH",
                        ValorEsperado = digitoEsperado,
                        ValorActual = socio.DigitoVerificadorHorizontal,
                        Correcto = false,
                        Mensaje = "El registro fue modificado o alterado por fuera del sistema."
                    });
                }
            }

            Dictionary<string, int> verticalesCalculados = CalcularDigitosVerticales(socios);
            Dictionary<string, int> verticalesGuardados = integridadDAL.ObtenerDigitosVerticales(EntidadSocio);

            foreach (KeyValuePair<string, int> verticalCalculado in verticalesCalculados)
            {
                int valorGuardado = verticalesGuardados.ContainsKey(verticalCalculado.Key) ? verticalesGuardados[verticalCalculado.Key] : -1;

                if (valorGuardado != verticalCalculado.Value)
                {
                    resultados.Add(new ResultadoIntegridadBE
                    {
                        Entidad = EntidadSocio,
                        Identificador = verticalCalculado.Key,
                        TipoDigito = "DVV",
                        ValorEsperado = verticalCalculado.Value,
                        ValorActual = valorGuardado,
                        Correcto = false,
                        Mensaje = "La columna presenta altas, bajas o intercambios no registrados por la aplicación."
                    });
                }
            }

            if (resultados.Count == 0)
            {
                resultados.Add(new ResultadoIntegridadBE
                {
                    Entidad = EntidadSocio,
                    Identificador = "GENERAL",
                    TipoDigito = "DVH/DVV",
                    ValorEsperado = 0,
                    ValorActual = 0,
                    Correcto = true,
                    Mensaje = "La integridad de la entidad Socio es correcta."
                });
            }

            return resultados;
        }

        public void RecalcularIntegridad()
        {
            List<SocioBE> socios = integridadDAL.ListarSociosParaIntegridad();

            foreach (SocioBE socio in socios)
            {
                int digitoHorizontal = CalcularDigitoHorizontalSocio(socio);
                integridadDAL.ActualizarDigitoHorizontalSocio(socio.IdSocio, digitoHorizontal);
                socio.DigitoVerificadorHorizontal = digitoHorizontal;
            }

            Dictionary<string, int> digitosVerticales = CalcularDigitosVerticales(socios);
            integridadDAL.GuardarDigitosVerticales(EntidadSocio, digitosVerticales);
        }

        public int CalcularDigitoHorizontalSocio(SocioBE socio)
        {
            return calculadorDigitoVerificador.CalcularHorizontal(
                socio.IdSocio.ToString(CultureInfo.InvariantCulture),
                socio.TipoDocumento,
                socio.NumeroDocumento.ToString(CultureInfo.InvariantCulture),
                socio.Nombre,
                socio.Apellido,
                socio.FechaNacimiento.ToString("yyyyMMdd", CultureInfo.InvariantCulture),
                socio.Nacionalidad,
                socio.Mail,
                socio.Telefono.ToString(CultureInfo.InvariantCulture),
                string.IsNullOrWhiteSpace(socio.Activo) ? "S" : socio.Activo
            );
        }

        private Dictionary<string, int> CalcularDigitosVerticales(List<SocioBE> socios)
        {
            List<Dictionary<string, string>> filas = new List<Dictionary<string, string>>();

            foreach (SocioBE socio in socios)
            {
                Dictionary<string, string> fila = new Dictionary<string, string>();
                fila.Add("IdSocio", socio.IdSocio.ToString(CultureInfo.InvariantCulture));
                fila.Add("TipoDocumento", socio.TipoDocumento);
                fila.Add("NumeroDocumento", socio.NumeroDocumento.ToString(CultureInfo.InvariantCulture));
                fila.Add("Nombre", socio.Nombre);
                fila.Add("Apellido", socio.Apellido);
                fila.Add("FechaNacimiento", socio.FechaNacimiento.ToString("yyyyMMdd", CultureInfo.InvariantCulture));
                fila.Add("Nacionalidad", socio.Nacionalidad);
                fila.Add("Email", socio.Mail);
                fila.Add("Telefono", socio.Telefono.ToString(CultureInfo.InvariantCulture));
                fila.Add("Activo", string.IsNullOrWhiteSpace(socio.Activo) ? "S" : socio.Activo);
                fila.Add("DVH", socio.DigitoVerificadorHorizontal.ToString(CultureInfo.InvariantCulture));
                filas.Add(fila);
            }

            return calculadorDigitoVerificador.CalcularVertical(filas);
        }
    }
}
