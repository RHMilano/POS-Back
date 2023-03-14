using System;
using System.Diagnostics;
using System.IO;

namespace CargaLlavesBBVAv2.TryCatchI
{
    public class TryCatchBusinessExecutor
    {
        /// <summary>
        /// Ejecuta un metodo de la clase business
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action">Metodo a ejecutar</param>
        /// <param name="errMsg">Error qeu se presentara al usuario</param>
        /// <returns></returns>
        public ResponseBusiness<T> SafeExecutor<T>(Func<T> action, string errMsg = "Milano")
        {
            try
            {
                return action();
            }
            catch (Exception ex)
            {
                var message = $"{errMsg}";
                Console.BackgroundColor = ConsoleColor.Yellow;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Exception...");
                Console.WriteLine($"Mensaje: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");

                return this.AddErrorLog<T>(action.Method.Name, ex.StackTrace, ex.Message);
            }
        }

        /// <summary>
        /// Metodo que guarda en un txt el detalle del error 
        /// </summary>
        /// <typeparam name="T">Tipo de dato</typeparam>
        /// <param name="message">Mensaje</param>
        /// <param name="stackTrace">pila de errores</param>
        /// <param name="layer">capa del sistema</param>
        /// <param name="errMsg">mensaje de usuario</param>
        /// <param name="userMessage">mensaje de usuario</param>
        /// <returns></returns>
        public ResponseBusiness<T> AddErrorLog<T>(string description, string stackTrace, string errMsg)
        {
            var currentDate = DateTime.Now;
            string id = currentDate.ToString("ddMMyyyyhhmmss");

            var messageAddEvent = "";
            try
            {
                InsertError(id, description, stackTrace, errMsg);
            }
            catch (Exception ex)
            {
                messageAddEvent = $"Ocurrió un error ID [{id}] al intentar registrar la siguiente incidencia [{errMsg}].\r\n\r\nDescripción: {ex.Message}";
                Trace.WriteLine(messageAddEvent);
            }

            ResponseBusiness<T> temp = new ResponseBusiness<T>();
            temp.Result.Status = false;
            temp.Result.CodeNumber = id;
            temp.Result.Error = errMsg;
            temp.Result.CodeDescription = description;
            return temp;
        }

        /// <summary>
        /// Inserta el error en un texto
        /// </summary>
        /// <param name="id">id generado por el sistema</param>
        /// <param name="message">Mensaje</param>
        /// <param name="stacktrace">pila de errores</param>
        /// <param name="layer">capa del sistema</param>
        /// <param name="errMesg">mensaje de usuario</param>
        public void InsertError(string id, string message, string stacktrace, string errMesg)
        {
            string path = @"C:\PosMilano\LogsMilano\BBVA\";
            DateTime date = DateTime.Now;
            string nameFile = string.Format(@"{0}/{1}_{2}_{3}.log", path, date.Day, date.Month, date.Year);
            string idSerialize = "";
            idSerialize = $"{id.Substring(0, 2)}/{id.Substring(2, 2)}/{id.Substring(4, 4)}-{id.Substring(8, 2)}:{id.Substring(10, 2)}:{id.Substring(12, 2)}";

            if (!System.IO.File.Exists(nameFile))
            {
                FileStream newFile = System.IO.File.Create(nameFile);
                newFile.Close();
            }
            using (StreamWriter sw = File.AppendText(nameFile))
            {
                sw.WriteLine($"CARGA LLAVES ID: {id}");
                sw.WriteLine($"DATE: {idSerialize}");
                sw.WriteLine($"MESSAGE: {errMesg}");
                sw.WriteLine($"FUNCTION: {message}");
                sw.WriteLine($"STACKTRACE: {stacktrace}");
                sw.WriteLine("-----------------------------");
                sw.Close();
            }
        }

        ///// <summary>
        ///// Inserta el error en InsertErrorEventViewer
        ///// </summary>
        ///// <param name="id">id generado por el sistema</param>
        ///// <param name="message">Mensaje</param>
        ///// <param name="stacktrace">pila de errores</param>
        ///// <param name="layer">capa del sistema</param>
        ///// <param name="errMesg">mensaje de usuario</param>
        //private void InsertErrorEventViewer(string id, string message, string stacktrace, string layer, string errMesg)
        //{
        //    string source = "MILANO POS";
        //    string log = "Application";
        //    string messageString = string.Format("ID:{0} MESSAGE:{1} STACKTRACE:{2} LAYER:{3} MESSAGE:{4}", id, message, stacktrace, layer, errMesg);
        //    if (!EventLog.SourceExists(source))
        //        EventLog.CreateEventSource(source, log);
        //    EventLog.WriteEntry(source, messageString, EventLogEntryType.Error);
        //}

    }
}
