using Jose;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
namespace Milano.BackEnd.Utils
{

    /// <summary>
    /// Clase para encritar
    /// </summary>
    public class Encrypted
    {

        /// <summary>
        /// Método que decodifica una cadena codificada en formato base64
        /// </summary>
        /// <param name="argumento">cadena a decodificar</param>
        /// <returns></returns>
        public string Base64DecodeToString(string argumento)
        {
            return Encoding.UTF8.GetString(Base64UrlDecode(argumento));
        }

        /// <summary>
        /// Generala la llave de encryptación
        /// </summary>
        /// <param name="argumento">cadena a encriptar</param>
        /// <returns></returns>
        public byte[] Base64UrlDecode(string argumento)
        {
            string s = argumento;
            s = s.Replace('-', '+');
            s = s.Replace('_', '/');
            switch (s.Length % 4)
            {
                case 0: break;
                case 2: s += "=="; break;
                case 3: s += "="; break;
                default:
                    throw new System.Exception(
                "No es una cadena base64 válida");
            }
            _ = Convert.FromBase64String(s);

            return Convert.FromBase64String(s);
        }

        /// <summary>
		/// Obtiene el token que se enviara al cliente
		/// </summary>
		/// <param name="tempToken">Token generado sin encriptar</param>		
		/// <returns>Regresa el token del cliente</returns>
		public static string Encode(Dictionary<string, object> tempToken)
        {
            byte[] keys = new Encrypted().Base64UrlDecode(ConfigurationManager.AppSettings["base64UrlDecode"].ToString());
            string token = JWT.Encode(tempToken, keys, JwsAlgorithm.HS256);
            return token;
        }

        /// <summary>
        /// Metodo para encriptar la cadena
        /// </summary>
        /// <param name="token">cadena a encriptar</param>
        /// <returns>cadena encriptada</returns>
        public static string Encode(string token)
        {
            byte[] keys = new Encrypted().Base64UrlDecode(ConfigurationManager.AppSettings["base64UrlDecode"].ToString());
            return JWT.Encode(token, keys, JwsAlgorithm.HS256);
        }

        /// <summary>
        /// Metodo para desencriptar
        /// </summary>
        /// <param name="token">cadena encriptada</param>
        /// <returns>cadena desencriptada</returns>
        public static string Decode(string token)
        {

            if (token.Contains("Bearer "))
            {
                token = token.Replace("Bearer ", "");
            }

            byte[] keys = new Encrypted().Base64UrlDecode(ConfigurationManager.AppSettings["base64UrlDecode"].ToString());
            return JWT.Decode(token, keys, null);
        }
    }
}
