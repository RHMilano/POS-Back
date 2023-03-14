using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{

    /// <summary>
    /// Informacion de pago de servicios
    /// </summary>
    [DataContract]
    public class InformacionProveedorExternoAsociadaPS
    {

        /// <summary>
        /// Constructor
        /// </summary>
        public InformacionProveedorExternoAsociadaPS()
        {
            this.Cuenta = "";
            this.SkuCompania = "";
        }

        /// <summary>
        /// SKU de la compañia del servicio externo
        /// </summary>
        [DataMember(Name = "skuCompania")]
        public string SkuCompania { get; set; } = "";

        /// <summary>
        /// Número telefónico de la recarga
        /// </summary>
        [DataMember(Name = "cuenta")]
        public string Cuenta { get; set; }

        /// <summary>
        /// Informacion adicional
        /// </summary>
        [DataMember(Name = "infoAdicional")]
        public PagoServiciosInfoAdicional InfoAdicional { get; set; }

    }
}
