using System;
using System.IO;

namespace Project.Services.LogMonitor
{
    /// <summary>
    /// Funcion para registro de logs en el archivo de texto
    /// </summary>
    public class LogRegister
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logMessage"></param>
        /// <param name="Inicio"></param>
        public void LogEntry(string logMessage, int Inicio)
        {
            string ruta = "C:\\PosMilano\\LogCreditCard.txt";

            StreamWriter w = File.AppendText(ruta);

            //if (!File.Exists(ruta))
            //{
            //    //Console.WriteLine($"{FILE_NAME} already exists!");
            //    return;
            //}

            switch (Inicio)
            {
                case 1: // Inicio de archivo
                    w.WriteLine("\r\r\n/////////////////////////////////////////////////////////////////////////////");
                    w.WriteLine("\r\nInicio archivo: ");
                    w.WriteLine($"{DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()}");
                    w.WriteLine("-----------------------------------------------------------------------------");
                    break;

                case 2: // Inicio de proceso
                    w.Write($"\r\nProcedimiento: {logMessage}");
                    break;

                case 3: // Línea
                    w.Write($"\r\nLínea: {logMessage}");
                    break;
                case 4: // Parámetros
                    w.Write($"\r\nParámetros: {logMessage}");
                    break;

                default:
                    break;
            }

            w.Close();
            w.Dispose();
        }

    }
}