using System;
using System.Security.Cryptography;
using System.Text;

namespace SERVICIOS
{
    public class Seguridad
    {
        private const string SaltAplicacion = "ClubManagerPlus2026";

        public static string GenerarHash(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("La contraseña es obligatoria.");
            }

            using (SHA256 algoritmoHash = SHA256.Create())
            {
                string textoParaHashear = SaltAplicacion + password;
                byte[] bytesEntrada = Encoding.UTF8.GetBytes(textoParaHashear);
                byte[] bytesHash = algoritmoHash.ComputeHash(bytesEntrada);

                StringBuilder hashResultado = new StringBuilder();
                foreach (byte byteHash in bytesHash)
                {
                    hashResultado.Append(byteHash.ToString("x2"));
                }

                return hashResultado.ToString();
            }
        }

        public static bool VerificarPassword(string password, string hashGuardado)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hashGuardado))
            {
                return false;
            }

            string hashCalculado = GenerarHash(password);
            return string.Equals(hashCalculado, hashGuardado, StringComparison.OrdinalIgnoreCase);
        }
    }
}
