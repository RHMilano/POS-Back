using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Tools
{
    public class LogSet
    {
        public void Register(LogDTO logDTO)
        {

            string pathFile = ConfigurationManager.AppSettings["logPath"];
            pathFile = pathFile + TheFileName();
            const string TheLine = "-------------------------------------------------------------------------------------------";
            var currentDate = DateTime.Now;
            string id = currentDate.ToString("ddMMyyyyhhmmss");
            id = $"{id.Substring(0, 2)}/{id.Substring(2, 2)}/{id.Substring(4, 4)}-{id.Substring(8, 2)}:{id.Substring(10, 2)}:{id.Substring(12, 2)}";

            using (StreamWriter w = File.AppendText(pathFile))
            {
                if (logDTO.LogType == LogType.BySecuence)
                {
                    switch (logDTO.BBVASecuence)
                    {
                        case BBVASecuence.SaveSettings:
                            w.WriteLine(TheLine);
                            w.WriteLine("INICIA COBRO CON TARJETA ");
                            w.WriteLine($"FECHA:{id}");

                            if (!logDTO.EsError)
                                w.WriteLine("-SaveSettings: OK");
                            else
                                w.WriteLine($"-SaveSettings: Error:{logDTO.Message} \n\t StackTrace:{logDTO.StackTrace}");
                            break;
                        case BBVASecuence.LoadSettings:
                            if (!logDTO.EsError)
                                w.WriteLine("-LoadSettings: OK");
                            else
                                w.WriteLine($"-LoadSettings: Error:{logDTO.Message} \n\t StackTrace:{logDTO.StackTrace}");
                            break;
                        case BBVASecuence.LoadPinPad:
                            if (!logDTO.EsError)
                                w.WriteLine("-LoadPinPad: OK");
                            else
                                w.WriteLine($"-LoadPinPad: Error:{logDTO.Message} \n\t StackTrace:{logDTO.StackTrace}");
                            break;
                        case BBVASecuence.Sale:
                            if (!logDTO.EsError)
                                w.WriteLine("-Sale: OK");
                            else
                                w.WriteLine($"-Sale: {logDTO.Message} \n\t StackTrace:{logDTO.StackTrace}");
                            break;
                        case BBVASecuence.LeerTarjeta:
                            if (!logDTO.EsError)
                                w.WriteLine("-LeerTarjeta: OK");
                            else
                                w.WriteLine($"-LeerTarjeta: {logDTO.Message} \n\t StackTrace:{logDTO.StackTrace}");
                            break;
                        case BBVASecuence.Autorizar:
                            if (!logDTO.EsError)
                                w.WriteLine("-Autorizar: OK");
                            else
                                w.WriteLine($"-Autorizar: {logDTO.Message} \n\t StackTrace:{logDTO.StackTrace}");
                            break;
                        case BBVASecuence.Response:
                            break;
                        case BBVASecuence.End:
                            w.WriteLine("\r\nFINALIZA COBRO CON TARJETA ");
                            break;
                        case BBVASecuence.NoSecuence:
                            break;
                        default:
                            break;
                    }
                }


                if (logDTO.LogType == LogType.Bloque)
                {
                    if (logDTO.EsError)
                        w.WriteLine("\r\nERROR REGISTRADO ");
                    else
                        w.WriteLine("\r\nINFORMACIÓN DE SEGUIMIENTO ");

                    w.WriteLine($"Fecha: {DateTime.Now.ToLongDateString()}");
                    w.WriteLine($"Hora: {DateTime.Now.ToLongTimeString()} ");
                    w.WriteLine("Mensaje:");
                    w.WriteLine(logDTO.Message);

                    if (logDTO.StackTrace != "")
                    {
                        w.WriteLine("StackTrace:");
                        w.WriteLine(logDTO.StackTrace);
                    }

                    if (logDTO.InfoExtra != "")
                    {
                        w.WriteLine("Información adicional:");
                        w.WriteLine(logDTO.InfoExtra);
                    }
                    w.WriteLine("----------------------------------------------------------");
                }


              

               
            }
        }

        public String TheFileName()
        {
            DateTime fechaActual = DateTime.Today;
            int year = fechaActual.Year;
            int month = fechaActual.Month;
            int day = fechaActual.Day;

            return $"{day}_{month}_{year}.log";
        }

    }


    public class LogDTO
    {
        public LogType LogType { get; set; }
        public BBVASecuence BBVASecuence { get; set; }

        public bool EsError { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string InfoExtra { get; set; }

    }

    public enum BBVASecuence
    {
        SaveSettings = 1,
        LoadSettings = 2,
        LoadPinPad = 3,
        Sale = 4,
        LeerTarjeta = 5,
        Autorizar = 6,
        Response = 7,
        End = 8,
        NoSecuence = 9
    }

    public enum LogType { 
        Bloque = 0,
        BySecuence = 1

    }
}
