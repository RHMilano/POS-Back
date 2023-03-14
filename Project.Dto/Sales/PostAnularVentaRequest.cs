using Milano.BackEnd.Dto.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.Sales
{
    /// <summary>
    /// Clase para post-anular venta
    /// </summary>
    [DataContract]
    public class PostAnularVentaRequest
    {
        /// <summary>
        /// Folio de la venta
        /// </summary>
        [DataMember(Name = "folioVenta")]
        public string FolioVenta { get; set; }

        /// <summary>
        /// Identificador del código de razón de la cancelación/anulación de la transacción
        /// </summary>
        [DataMember(Name = "codigoRazon")]
        public int CodigoRazon { get; set; }

    }
}
