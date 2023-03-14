using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Milano.BackEnd.Dto.General
{
    /// <summary>
    /// DTO para almacenar la información de promoción con tarjeta bancaría
    /// </summary>
    [DataContract]
    public class ConfiguracionMSI
    {

        /// <summary>
        /// Versión del pago con bancomer
        /// </summary>
        [DataMember(Name = "versionBBVA")]
        public int VersionBBVA { get; set; }

        /// <summary>
        /// Un valor mayor a uno, indica que si existen meses sin intereses
        /// aplicados y se guardan en formato de bytes
        /// </summary>
        [DataMember(Name = "mesesSinInteresesVisa")]
        public int MesesSinInteresesVisa { get; set; }



        /// <summary>
        /// Un valor mayor a uno, indica que si existen meses sin intereses
        /// aplicados y se guardan en formato de bytes
        /// </summary>
        [DataMember(Name = "mesesSinInteresesAmex")]
        public int MesesSinInteresesAmex { get; set; }

        /// <summary>
        /// Monto mínimo para promoción Visa
        /// </summary>
        [DataMember(Name = "montoMinimoVisa")]
        public int MontoMinimoVisa { get; set; }
        
        /// <summary>
        /// Monto máximo para promoción Visa
        /// </summary>
        [DataMember(Name = "montoMaximoVisa")]
        public int MontoMaximoVisa { get; set; }

        /// <summary>
        /// Monto mínimo para promoción Amex
        /// </summary>
        [DataMember(Name = "montoMinimoAmex")]
        public int MontoMinimoAmex { get; set; }

        /// <summary>
        /// Monto máximo para promoción Amex
        /// </summary>
        [DataMember(Name = "montoMaximoAmex")]
        public int MontoMaximoAmex { get; set; }




    }

}
