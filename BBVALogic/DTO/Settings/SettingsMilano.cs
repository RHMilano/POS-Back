using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBVALogic.DTO.Setings
{
    public class SettingsMilano
    {
        /// <summary>
        /// OperatorId
        /// </summary>
        public string Operador { get; set; }
        /// <summary>
        /// ClaveLogs
        /// </summary>
        public string ClaveLogs { get; set; }
        /// <summary>
        /// PinPadConexion
        /// </summary>
        public string PinPadConexion { get; set; }
        /// <summary>
        /// PinPadTimeOut
        /// </summary>
        public string PinPadTimeOut { get; set; }
        /// <summary>
        /// PinPadPuertoWiFi
        /// </summary>
        public string PinPadPuertoWiFi { get; set; }
        /// <summary>
        /// PinPadMensaje
        /// </summary>
        public string PinPadMensaje { get; set; }
        /// <summary>
        /// ClaveBinesExcepcion
        /// </summary>
        public string ClaveBinesExcepcion { get; set; }
        /// <summary>
        /// HostUrl
        /// </summary>
        public string HostUrl { get; set; }
        /// <summary>
        /// BinesUrl
        /// </summary>
        public string BinesUrl { get; set; }
        /// <summary>
        /// TokenUrl
        /// </summary>
        public string TokenUrl { get; set; }
        /// <summary>
        /// UpdateUrl
        /// </summary>
        public string TelecargaUrl { get; set; }
        /// <summary>
        /// HostTimeOut
        /// </summary>
        public string HostTimeOut { get; set; }
        /// <summary>
        /// ComercioAfiliacion
        /// </summary>
        public string ComercioAfiliacion { get; set; }
        /// <summary>
        /// ComercioTerminal
        /// </summary>
        public string ComercioTerminal { get; set; }
        /// <summary>
        /// ComercioMac
        /// </summary>
        public string ComercioMac { get; set; }
        /// <summary>
        /// IdAplicacion
        /// </summary>
        public string IdAplicacion { get; set; }
        /// <summary>
        /// ClaveSecreta
        /// </summary>
        public string ClaveSecreta { get; set; }
        /// <summary>
        /// logs
        /// </summary>
        public bool Logs { get; set; }
        /// <summary>
        /// Contactless
        /// </summary>
        public bool PinPadContactless { get; set; }
        /// <summary>
        /// Garanti
        /// </summary>
        public bool FuncionalidadGaranti { get; set; }
        /// <summary>
        /// Moto
        /// </summary>
        public bool FuncionalidadMoto { get; set; }
        /// <summary>
        /// CanDigit
        /// </summary>
        public bool TecladoLiberado { get; set; }
        /// <summary>
        /// Saber si la transferencia de datos es correcta
        /// </summary>
        public bool Correcto { get; set; }


        public string Valores() {

            string clase = "";

            clase = $"OperatorId:{Operador}, ClaveLogs:{ClaveLogs}, PinPadConexion:{PinPadConexion}, PinPadTimeOut:{PinPadTimeOut} \n";
            clase += $"PinPadPuertoWiFi:{PinPadPuertoWiFi}, PinPadMensaje:{PinPadMensaje}, ClaveBinesExcepcion:{ClaveBinesExcepcion} \n ";
            clase += $"HostUrl:{HostUrl}";
            clase += $"BinesUrl:{BinesUrl}";
            clase += $"TelecargaUrl:{TelecargaUrl}";
            clase += $"TokenUrl:{TokenUrl}";
            clase += $"HostTimeOut:{HostTimeOut}, ComercioAfiliacion:{ComercioAfiliacion}, ComercioTerminal:{ComercioTerminal}, ComercioMac:{ComercioMac} \n";
            clase += $"IdAplicacion:{IdAplicacion}, ClaveSecreta:{ClaveSecreta}, Logs:{Logs}, PinPadContactless:{PinPadContactless} \n";
            clase += $"FuncionalidadGaranti:{FuncionalidadGaranti}, FuncionalidadMoto:{FuncionalidadMoto}, TecladoLiberado:{TecladoLiberado}";
            return clase;
        
        }


    }
}
