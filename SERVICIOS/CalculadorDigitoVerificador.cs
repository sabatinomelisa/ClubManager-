using System;
using System.Collections.Generic;
using System.Linq;

namespace SERVICIOS
{
    public class CalculadorDigitoVerificador
    {
        private const int ModuloCalculo = 1000000007;

        public int CalcularHorizontal(params string[] valoresAtributos)
        {
            if (valoresAtributos == null)
            {
                return 0;
            }

            long acumulador = 0;

            for (int indiceAtributo = 0; indiceAtributo < valoresAtributos.Length; indiceAtributo++)
            {
                string valorAtributo = valoresAtributos[indiceAtributo] ?? string.Empty;

                for (int indiceCaracter = 0; indiceCaracter < valorAtributo.Length; indiceCaracter++)
                {
                    int posicionAtributo = indiceAtributo + 1;
                    int posicionCaracter = indiceCaracter + 1;
                    int valorCaracter = Convert.ToInt32(valorAtributo[indiceCaracter]);

                    acumulador += posicionAtributo * posicionCaracter * valorCaracter;
                }
            }

            return Convert.ToInt32(acumulador % ModuloCalculo);
        }

        public Dictionary<string, int> CalcularVertical(List<Dictionary<string, string>> filas)
        {
            Dictionary<string, int> resultados = new Dictionary<string, int>();

            if (filas == null)
            {
                return resultados;
            }

            for (int indiceFila = 0; indiceFila < filas.Count; indiceFila++)
            {
                Dictionary<string, string> fila = filas[indiceFila];

                foreach (KeyValuePair<string, string> atributo in fila.OrderBy(item => item.Key))
                {
                    int valorCalculado = CalcularHorizontal(indiceFila.ToString(), atributo.Key, atributo.Value);

                    if (!resultados.ContainsKey(atributo.Key))
                    {
                        resultados.Add(atributo.Key, 0);
                    }

                    resultados[atributo.Key] = Convert.ToInt32((resultados[atributo.Key] + valorCalculado) % ModuloCalculo);
                }
            }

            return resultados;
        }
    }
}
