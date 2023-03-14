using CargaLlavesBBVAv2.TryCatchI;
using EGlobal.TotalPosSDKNet.Interfaz.Authorizer;
using EGlobal.TotalPosSDKNet.Interfaz.Catalog;
using EGlobal.TotalPosSDKNet.Interfaz.Exceptions;
using EGlobal.TotalPosSDKNet.Interfaz.Layout;
using EGlobal.TotalPosSDKNet.Interfaz.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargaLlavesBBVAv2
{
    public class Llaves : BaseBusiness
    {

        public ResponseBusiness<Boolean> CargaManual()
        {
            return tryCatch.SafeExecutor(() =>
            {
                Settings.SaveSettings();
                Settings.LoadSettings();
                LoadPinPad();
                CargaLlaves();
                return true;
            });

            Console.Read();
        }

        public void CargaLlaves()
        {
            if (!Settings.FuncionalidadMoto)
            {
                Peticion p = new Peticion();
                p.SetAfiliacion(Settings.ComercioAfiliacion, Moneda.Pesos);
                p.SetTerminal(Settings.ComercioTerminal, Settings.ComercioMac);
                p.Operador = Settings.Operador;
                p.Fecha = DateTime.Now.ToString("yyyyMMddHHmmss");
                p.Amex = false;
                p.SetOperacion(Operacion.CargaLlaves, null);

                Respuesta respuesta = p.Autorizar();

                if (respuesta.CodigoRespuesta == "00")
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Carga de llaves !exitosa¡");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Respuesta del proceso: {respuesta.Leyenda}");
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Respuesta del proceso: No se puede realizar carga de llaves con funcionalidad MoTo activa.", "E-Global TotalPos SDK.Net Demo");
            }
        }

        private void LoadPinPad()
        {

            Configuracion configuracion;

            configuracion = new Configuracion
            {
                Logs = Settings.Logs,
                ClaveLogs = Settings.ClaveLogs,

                PinPadConexion = Settings.PinPadConexion,
                PinPadTimeOut = Settings.PinPadTimeOut,
                PinPadPuerto = Settings.PinPadPuertoWiFi,
                PinPadMensaje = Settings.PinPadMensaje,
                PinPadContactless = Settings.PinPadContactless,
                ClaveBines = Settings.ClaveBinesExcepcion,
                HostUrl = Settings.HostUrl,
                BinesUrl = Settings.BinesUrl,
                TokenUrl = Settings.TokenUrl,
                TelecargaUrl = Settings.TelecargaUrl,
                HostTimeOut = Settings.HostTimeOut,

                FuncionalidadGaranti = Settings.FuncionalidadGaranti,
                FuncionalidadMoto = Settings.FuncionalidadMoto,
                TecladoLiberado = Settings.TecladoLiberado,

                ComercioAfiliacion = Settings.ComercioAfiliacion,
                ComercioTerminal = Settings.ComercioTerminal,
                ComercioMac = Settings.ComercioMac,

                IdAplicacion = Settings.IdAplicacion,
                ClaveSecreta = Settings.ClaveSecreta
            };

            Interfaz.Instance.Configuracion = configuracion;

            Interfaz.Instance.Inicializar();


            //logDTO.LogType = LogType.BySecuence;
            //logDTO.BBVASecuence = BBVASecuence.LoadPinPad;

            //logSet.Register(logDTO);

        }
    }
}
