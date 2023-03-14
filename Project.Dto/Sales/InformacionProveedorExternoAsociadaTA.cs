using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{
    /// <summary>
    /// Clase que representa la información adicional asociada a un proveedor tercero de TA
    /// </summary>
    [DataContract]
    public class InformacionProveedorExternoTA
    {
		/// <summary>
		/// Constructor
		/// </summary>
		public InformacionProveedorExternoTA()
		{
			this.NumeroTelefonico = "";
			this.SkuCompania = "";
		}

        /// <summary>
        /// SKU de la compañia del servicio externo
        /// </summary>
        [DataMember(Name = "skuCompania")]
        public string SkuCompania { get; set; }

        /// <summary>
        /// Número telefónico de la recarga
        /// </summary>
        [DataMember(Name = "numeroTelefonico")]
        public string  NumeroTelefonico { get; set; }

    }
}
