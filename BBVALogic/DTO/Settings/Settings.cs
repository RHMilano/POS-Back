using System;
using System.Xml;
using Tools;
using BBVALogic.Data;
using BBVALogic.DTO.Setings;

namespace EGlobal.DemoTotalPosSDKNet.Settings
{
    internal static class Settings
    {
        static LogSet logSet = new LogSet();
        static LogDTO logDTO = new LogDTO();

        #region Variables para propiedades
        private static string _sOperatorId = "OPE001";
        private static string _sClaveLogs = "";
        private static string _sPinPadConexion = "";
        private static string _sPinPadTimeOut = "";
        private static string _sPinPadPuertoWiFi = "";
        private static string _sPinPadMensaje = "";
        private static string _sClaveBinesExcepcion = "";
        private static string _sHostUrl = "";
        private static string _sBinesUrl = "";
        private static string _sTokenUrl = "";
        private static string _sUpdateUrl = "";
        private static string _sHostTimeOut = "";
        private static string _sComercioAfiliacion = "";
        private static string _sComercioTerminal = "";
        private static string _sComercioMac = "";
        private static string _sIdAplicacion = "";
        private static string _sClaveSecreta = "";

        private static bool _bLogs = false;
        private static bool _bContactless = false;
        private static bool _bGaranti = false;
        private static bool _bMoto = false;
        private static bool _bCanDigit = true;
        #endregion

        #region Propiedades
        public static string Operador
        {
            get { return _sOperatorId; }
            set { _sOperatorId = value; }
        }

        public static string ClaveLogs
        {
            get { return _sClaveLogs; }
            set { _sClaveLogs = value; }
        }

        public static string PinPadConexion
        {
            get { return _sPinPadConexion; }
            set { _sPinPadConexion = value; }
        }

        public static string PinPadTimeOut
        {
            get { return _sPinPadTimeOut; }
            set { _sPinPadTimeOut = value; }
        }

        public static string PinPadPuertoWiFi
        {
            get { return _sPinPadPuertoWiFi; }
            set { _sPinPadPuertoWiFi = value; }
        }

        public static string PinPadMensaje
        {
            get { return _sPinPadMensaje; }
            set { _sPinPadMensaje = value; }
        }

        public static string ClaveBinesExcepcion
        {
            get { return _sClaveBinesExcepcion; }
            set { _sClaveBinesExcepcion = value; }
        }

        public static string HostUrl
        {
            get { return _sHostUrl; }
            set { _sHostUrl = value; }
        }

        public static string BinesUrl
        {
            get { return _sBinesUrl; }
            set { _sBinesUrl = value; }
        }

        public static string TokenUrl
        {
            get { return _sTokenUrl; }
            set { _sTokenUrl = value; }
        }

        public static string TelecargaUrl
        {
            get { return _sUpdateUrl; }
            set { _sUpdateUrl = value; }
        }
        public static string HostTimeOut
        {
            get { return _sHostTimeOut; }
            set { _sHostTimeOut = value; }
        }

        public static string ComercioAfiliacion
        {
            get { return _sComercioAfiliacion; }
            set { _sComercioAfiliacion = value; }
        }

        public static string ComercioTerminal
        {
            get { return _sComercioTerminal; }
            set { _sComercioTerminal = value; }
        }

        public static string ComercioMac
        {
            get { return _sComercioMac; }
            set { _sComercioMac = value; }
        }

        public static string IdAplicacion
        {
            get { return _sIdAplicacion; }
            set { _sIdAplicacion = value; }
        }

        public static string ClaveSecreta
        {
            get { return _sClaveSecreta; }
            set { _sClaveSecreta = value; }
        }

        public static bool Logs
        {
            get { return _bLogs; }
            set { _bLogs = value; }
        }

        public static bool PinPadContactless
        {
            get { return _bContactless; }
            set { _bContactless = value; }
        }

        public static bool FuncionalidadGaranti
        {
            get { return _bGaranti; }
            set { _bGaranti = value; }
        }

        public static bool FuncionalidadMoto
        {
            get { return _bMoto; }
            set { _bMoto = value; }
        }

        public static bool TecladoLiberado
        {
            get { return _bCanDigit; }
            set { _bCanDigit = value; }
        }
        #endregion

        /// <summary>
        /// Lee los datos del archivo XML para instanciar la variable settings e instanciar la pinpad
        /// </summary>
        public static void LoadSettings()
        {
            XmlDocument doc;

            doc = new XmlDocument();
            // doc.Load(@"C:\PosMilano\ClientBBVAv2\pinpad_params.config");

            doc.Load("Local.config");

            _sOperatorId = "OPE001";
            _bLogs = doc.SelectSingleNode("/configuracion/interfaz/logs/@value").Value == "1";
            _sClaveLogs = doc.SelectSingleNode("/configuracion/interfaz/clavelogs/@value").Value;
            _sPinPadConexion = doc.SelectSingleNode("/configuracion/interfaz/pinpadconexion/@value").Value;
            _sPinPadTimeOut = doc.SelectSingleNode("/configuracion/interfaz/pinpadtimeout/@value").Value;
            _sPinPadPuertoWiFi = doc.SelectSingleNode("/configuracion/interfaz/pinpadpuertowifi/@value").Value;
            _sPinPadMensaje = doc.SelectSingleNode("/configuracion/interfaz/pinpadmensaje/@value").Value;
            _bContactless = doc.SelectSingleNode("/configuracion/interfaz/contactless/@value").Value == "1";
            _sClaveBinesExcepcion = doc.SelectSingleNode("/configuracion/interfaz/clavebinesexcepcion/@value").Value;
            _sHostUrl = doc.SelectSingleNode("/configuracion/interfaz/hosturl/@value").Value;
            _sBinesUrl = doc.SelectSingleNode("/configuracion/interfaz/binesurl/@value").Value;
            _sTokenUrl = doc.SelectSingleNode("/configuracion/interfaz/tokenurl/@value").Value;
            _sUpdateUrl = doc.SelectSingleNode("/configuracion/interfaz/update/@value").Value;
            _sHostTimeOut = doc.SelectSingleNode("/configuracion/interfaz/hosttimeout/@value").Value;
            _bGaranti = doc.SelectSingleNode("/configuracion/interfaz/funcionalidadgaranti/@value").Value == "1";
            _bMoto = doc.SelectSingleNode("/configuracion/interfaz/funcionalidadmoto/@value").Value == "1";
            _bCanDigit = doc.SelectSingleNode("/configuracion/interfaz/tecladoliberado/@value").Value == "1";
            _sComercioAfiliacion = doc.SelectSingleNode("/configuracion/interfaz/comercioafiliacion/@value").Value;
            _sComercioTerminal = doc.SelectSingleNode("/configuracion/interfaz/comercioterminal/@value").Value;
            _sComercioMac = doc.SelectSingleNode("/configuracion/interfaz/comerciomac/@value").Value;
            _sIdAplicacion = doc.SelectSingleNode("/configuracion/interfaz/idaplicacion/@value").Value;
            _sClaveSecreta = doc.SelectSingleNode("/configuracion/interfaz/clavesecreta/@value").Value;

            //logDTO.LogType = LogType.BySecuence;
            //logDTO.BBVASecuence = BBVASecuence.LoadSettings;

            //logSet.Register(logDTO);
        }

        /// <summary>
        /// Recupera los datos de la base de datos y los escribe en el archivo XML
        /// </summary>
        public static void SaveSettings()
        {
            XmlDocument doc;
            Pos pos = new Pos();
            SettingsMilano settings = new SettingsMilano();

            settings = pos.GetBBVAConfig();

            doc = new XmlDocument();
            //doc.Load(@"C:\PosMilano\ClientBBVAv2\pinpad_params.config");
            doc.Load(@"Local.config");

            _bLogs = settings.Logs;
            _sClaveLogs = settings.ClaveLogs;
            _sPinPadConexion = settings.PinPadConexion;
            _sPinPadTimeOut = settings.PinPadTimeOut;
            _sPinPadPuertoWiFi = settings.PinPadPuertoWiFi;
            _sPinPadMensaje = settings.PinPadMensaje;
            _bContactless = settings.PinPadContactless;
            _sClaveBinesExcepcion = settings.ClaveBinesExcepcion;
            _sHostUrl = settings.HostUrl;
            _sBinesUrl = settings.BinesUrl;
            _sTokenUrl = settings.TokenUrl;
            _sUpdateUrl = settings.TelecargaUrl; ;

            _sHostTimeOut = settings.HostTimeOut;
            _bGaranti = settings.FuncionalidadGaranti;
            _bMoto = settings.FuncionalidadMoto;
            _bCanDigit = settings.TecladoLiberado;
            _sComercioAfiliacion = settings.ComercioAfiliacion;
            _sComercioTerminal = settings.ComercioTerminal;
            _sComercioMac = settings.ComercioMac;
            _sIdAplicacion = settings.IdAplicacion;
            _sClaveSecreta = settings.ClaveSecreta;
            _sOperatorId = settings.Operador;

            doc.SelectSingleNode("/configuracion/interfaz/logs/@value").Value = _bLogs ? "1" : "0";
            doc.SelectSingleNode("/configuracion/interfaz/clavelogs/@value").Value = _sClaveLogs;
            doc.SelectSingleNode("/configuracion/interfaz/pinpadconexion/@value").Value = _sPinPadConexion;
            doc.SelectSingleNode("/configuracion/interfaz/pinpadtimeout/@value").Value = _sPinPadTimeOut;
            doc.SelectSingleNode("/configuracion/interfaz/pinpadpuertowifi/@value").Value = _sPinPadPuertoWiFi;
            doc.SelectSingleNode("/configuracion/interfaz/pinpadmensaje/@value").Value = _sPinPadMensaje;
            doc.SelectSingleNode("/configuracion/interfaz/contactless/@value").Value = _bContactless ? "1" : "0";
            doc.SelectSingleNode("/configuracion/interfaz/clavebinesexcepcion/@value").Value = _sClaveBinesExcepcion;
            doc.SelectSingleNode("/configuracion/interfaz/hosturl/@value").Value = _sHostUrl;
            doc.SelectSingleNode("/configuracion/interfaz/binesurl/@value").Value = _sBinesUrl;
            doc.SelectSingleNode("/configuracion/interfaz/tokenurl/@value").Value = _sTokenUrl;
            doc.SelectSingleNode("/configuracion/interfaz/update/@value").Value = _sUpdateUrl;
            doc.SelectSingleNode("/configuracion/interfaz/hosttimeout/@value").Value = _sHostTimeOut;
            doc.SelectSingleNode("/configuracion/interfaz/funcionalidadgaranti/@value").Value = _bGaranti ? "1" : "0";
            doc.SelectSingleNode("/configuracion/interfaz/funcionalidadmoto/@value").Value = _bMoto ? "1" : "0";
            doc.SelectSingleNode("/configuracion/interfaz/tecladoliberado/@value").Value = _bCanDigit ? "1" : "0";
            doc.SelectSingleNode("/configuracion/interfaz/comercioafiliacion/@value").Value = _sComercioAfiliacion;
            doc.SelectSingleNode("/configuracion/interfaz/comercioterminal/@value").Value = _sComercioTerminal;
            doc.SelectSingleNode("/configuracion/interfaz/comerciomac/@value").Value = _sComercioMac;
            doc.SelectSingleNode("/configuracion/interfaz/idaplicacion/@value").Value = _sIdAplicacion;
            doc.SelectSingleNode("/configuracion/interfaz/clavesecreta/@value").Value = _sClaveSecreta;

            //doc.Save(@"C:\PosMilano\ClientBBVAv2\pinpad_params.config");
            doc.Load(@"Local.config");

            logDTO.LogType = LogType.BySecuence;
            logDTO.BBVASecuence = BBVASecuence.SaveSettings;

            logSet.Register(logDTO);

        }
    }
}
