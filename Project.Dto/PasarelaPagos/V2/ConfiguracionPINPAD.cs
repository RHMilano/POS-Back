using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.PasarelaPagos
{

    /// <summary> 
    /// Respuesta al envio a Dll 
    /// </summary> 
    public class ConfiguracionPINPAD
    {

        static String TimeOutPinpad = "";

        /// <summary> 
        /// Consecutivo del header 
        /// </summary>
        public int HeaderConsecutivo { get; set; }

        /// <summary> 
        /// Nombre del Comercio 
        /// </summary> 
        public string Comercio { get; set; }

        /// <summary> 
        /// Ip del Host Autorizador 
        /// </summary> 
        public string IpHost { get; set; }
        /// <summary> 
        /// Tiempo limite de espera de respuesta del host autorizador 
        /// </summary> 

        public int HostTimeOut { get; set; }

        /// <summary> 
        /// Puerto del host Autorizador 
        /// </summary> 
        public int PuertoHost { get; set; }

        /// <summary> 
        /// Nombre del puesrto en el cual esta configurado la PinPad 
        /// </summary> 
        public string PuertoSerie { get; set; }

        /// <summary> 
        /// Ip de la PinPad 
        /// </summary> 
        public string PinPadID { get; set; }

        /// <summary> 
        /// Ip del Host Autorizador 
        /// </summary> 
        public string PinpadTimeOut
        {
            get
            {
                if (TimeOutPinpad == "")

                {
                    return TimeOutPinpad = "0";
                }
                else
                {
                    return TimeOutPinpad;
                }
            }
        }

        /// <summary> 
        /// id de afiliacion 
        /// </summary> 
        public static String Afiliacion

        {
            get { return Afiliacion; }
        }

        /// <summary> 
        /// campo de id de terminal 
        /// </summary> 
        public static String Terminal
        {
            get { return Terminal; }
        }

        /// <summary> 
        /// es un campo fijo en cero ya que es un campo requerido unicamente para hoteles 
        /// </summary> 
        public static String IsFolio
        {
            get { return IsFolio; }
        }

        /// <summary> 
        /// de uso interno para Bancomer y se especifica siempre en cero 0 
        /// </summary> 
        public static String IsLog
        {
            get { return IsLog; }
        }

        /// <summary> 
        /// Cam de uso interno de Bancomer 
        /// </summary> 
        public static String DirUserDB
        {
            get { return DirUserDB; }
            set { DirUserDB = value; }
        }

        /// <summary> 
        /// clave para descarga de archivo de bines 
        /// </summary> 
        public static String IDBinesComercio
        {
            get { return IDBinesComercio; }
        }

        /// <summary> 
        /// bandera utilizada para identificar si el pinpad requierer carga de llaves 
        /// </summary> 
        public static int CargaLlave
        {
            get { return CargaLlave; }
        }

        /// <summary> 
        ///         
        /// </summary> 
        public static String SAO
        {
            get { return SAO; }
        }

        /// <summary> 
        /// Password de acceso a host 
        /// </summary> 
        public static String Password
        {
            get { return Password; }
        }

        /// <summary> 
        /// Campo donde se especifica tiempo de impresion el formato debera ser AAMMDDHHMMSS en esquema de 24 hrs 
        /// </summary> 
        public static String Impresion
        {
            get { return Impresion; }
        }

        /// <summary> 
        /// campo requerido para contener la direccion de donde se encontraran los archivos de actualizaciones 
        /// </summary> 
        public static String DIRACTVersion
        {
            get { return DIRACTVersion; }
            set { DIRACTVersion = value; }
        }

        /// <summary> 
        /// Campo del importe 
        /// </summary> 
        public static String Cash
        {
            get { return Cash; }
            set { Cash = value; }
        }

        /// <summary> 
        /// Bandera requerida para actualizar PINPAD de forma automatica 
        /// </summary> 
        public static int BACTVersion
        {
            get { return BACTVersion; }
            set { BACTVersion = value; }
        }

        /// <summary> 
        /// Flag identificador cargo con tarjeta AMEX 
        /// </summary> 
        public static int ConAMEX
        {
            get { return ConAMEX; }
            set { ConAMEX = value; }
        }

    }

}