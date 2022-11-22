using System.Security.Cryptography;
namespace Sistema_De_Gestion.Data
{
    public class Encrypt
    {
        public static byte[] GenerarToken() //genereamos un token con un numero de salto y un numero random
        {
            const int numerodeSalto = 24;

            using (var numeroRandomGenerador = new RNGCryptoServiceProvider())
            {
                var numeroRandom = new byte[numerodeSalto];
                numeroRandomGenerador.GetBytes(numeroRandom);

                return numeroRandom;
            }
        }

        private static byte[] Combine(byte[] primer, byte[] second) //usamos para combinar salto con la contrase
        {
            var combinado = new byte[primer.Length + second.Length];

            Buffer.BlockCopy(primer, 0, combinado, 0, primer.Length);
            Buffer.BlockCopy(second, 0, combinado, primer.Length, second.Length);

            return combinado;
        }

        public static byte[] HashPassword(byte[] toBeHashed, byte[] salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var combinedHash = Combine(toBeHashed, salt);

                return sha256.ComputeHash(combinedHash);
            }
        }
    }
}