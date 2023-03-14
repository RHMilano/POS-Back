

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using Milano.BackEnd.Dto;
namespace Milano.BackEnd.Business
{
    /// <summary>
    /// Clase que sirve como monitor de la ejecucion de los metodos de las clases de business
    /// </summary>
    public class TryCatchBusinessExecutor
    {

        /// <summary>
        /// Ejecuta un metodo de la clase business
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action">Metodo a ejecutar</param>
        /// <param name="errMsg">Error qeu se presentara al usuario</param>
        /// <returns></returns>
        public ResponseBussiness<T> SafeExecutor<T>(Func<T> action, string errMsg = "Ocurrió un error en la ejecución del proceso.")
        {
            try
            {
                return action();
            }
            catch (Exception ex)
            {
                var message = $"Mensaje:{errMsg}\r\n\r\nOrigen:\r\n{ex.Source}\r\n\r\nDescripcion:\r\n{ex.Message}";
                return this.AddErrorLog<T>(message, ex.StackTrace, "Negocio", errMsg + "; " + ex.Message, errMsg);
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
        public ResponseBussiness<T> AddErrorLog<T>(string message, string stackTrace, string layer, string errMsg, string userMessage)
        {
            var currentDate = DateTime.Now;
            var id = currentDate.ToString("ddMMyyyyhhmmss");
            var messageAddEvent = "";
            try
            {
                InsertError(id, message, stackTrace, layer, errMsg);
            }
            catch (Exception ex)
            {
                messageAddEvent = $"Ocurrió un error en la capa [{layer}] al intentar registrar la siguiente incidencia [{message}].\r\n\r\nDescripción: {ex.Message}";
                Trace.WriteLine(messageAddEvent);
            }
            ResponseBussiness<T> temp = new ResponseBussiness<T>();
            temp.Result.Status = false;
            temp.Result.CodeNumber = id;
            temp.Result.Error = errMsg;
            temp.Result.CodeDescription = userMessage;
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
        public void InsertError(string id, string message, string stacktrace, string layer, string errMesg)
        {
            string path = ConfigurationManager.AppSettings["logPath"].ToString();
            DateTime date = DateTime.Now;
            string nameFile = string.Format(@"{0}/{1}_{2}_{3}.txt", path, date.Day, date.Month, date.Year);
            if (!System.IO.File.Exists(nameFile))
            {
                FileStream newFile = System.IO.File.Create(nameFile);
                newFile.Close();
            }
            using (StreamWriter sw = File.AppendText(nameFile))
            {
                sw.WriteLine("ID:" + id);
                sw.WriteLine("MESSAGE:" + message);
                sw.WriteLine("STACKTRACE:" + stacktrace);
                sw.WriteLine("LAYER:" + layer);
                sw.WriteLine("ErrMesg:" + errMesg);
                sw.WriteLine("-----------------------------");
                sw.Close();
            }
        }

        /// <summary>
        /// Inserta el error en InsertErrorEventViewer
        /// </summary>
        /// <param name="id">id generado por el sistema</param>
        /// <param name="message">Mensaje</param>
        /// <param name="stacktrace">pila de errores</param>
        /// <param name="layer">capa del sistema</param>
        /// <param name="errMesg">mensaje de usuario</param>
        private void InsertErrorEventViewer(string id, string message, string stacktrace, string layer, string errMesg)
        {
            string source = "MILANO POS";
            string log = "Application";
            string messageString = string.Format("ID:{0} MESSAGE:{1} STACKTRACE:{2} LAYER:{3} MESSAGE:{4}", id, message, stacktrace, layer, errMesg);
            if (!EventLog.SourceExists(source))
                EventLog.CreateEventSource(source, log);
            EventLog.WriteEntry(source, messageString, EventLogEntryType.Error);
        }

    }
}
