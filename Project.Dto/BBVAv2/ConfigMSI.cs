using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Milano.BackEnd.Dto.BBVAv2
{
    /// <summary>
    /// Clase de configuración MSI BBVAv2 VISA y Master Card
    /// </summary>
    [DataContract]
    public class ConfigMSI
    {
        //public int VersionBBVA { get; set; }

        /// <summary>
        /// Indica en bytes los meses sin intereses para los que aplica
        /// </summary>
        [DataMember(Name = "mesesSinInteresesVisa")]
        public int MesesSinInteresesVisa { get; set; }

        //public int MesesSinInteresesAmex { get; set; }

        /// <summary>
        /// Monto mínimo para pasar a meses sin intereses
        /// </summary>
        [DataMember(Name = "montoMinimoVisa")]
        public int MontoMinimoVisa { get; set; }

        /// <summary>
        /// Monto tope para meses sin intereses
        /// </summary>
        [DataMember(Name = "montoMaximoVisa")]
        public int MontoMaximoVisa { get; set; }

        //public int MontoMinimoAmex { get; set; }

        //public int MontoMaximoAmex { get; set; }



    }
}
