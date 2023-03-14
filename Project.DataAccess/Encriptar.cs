using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Milano.BackEnd.DataAccess
{
    /// <summary>
    /// Clase para encritar passwords de la base de datos
    /// </summary>
    public static class Encriptar
    {

        private static byte[] key = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
        private static byte[] iv = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };

        /// <summary>
        /// Método que encripta una cadena
        /// </summary>
        /// <param name="cadena">Cadena a encriptar</param>
        /// <returns></returns>
        public static string EncriptarCadena(string cadena)
        {
            SymmetricAlgorithm algorithm = DES.Create();
            ICryptoTransform transform = algorithm.CreateEncryptor(key, iv);
            byte[] inputbuffer = Encoding.Unicode.GetBytes(cadena);
            byte[] outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
            return Convert.ToBase64String(outputBuffer);
        }

        /// <summary>
        /// Método que desencripta una cadena
        /// </summary>
        /// <param name="cadena">Cadena a desencriptar</param>
        /// <returns></returns>
        public static string DesencriptarCadena(string cadena)
        {
            SymmetricAlgorithm algorithm = DES.Create();
            ICryptoTransform transform = algorithm.CreateDecryptor(key, iv);
            byte[] inputbuffer = Convert.FromBase64String(cadena);
            byte[] outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
            return Encoding.Unicode.GetString(outputBuffer);
        }

    }
}