using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Milano.BackEnd.Dto.Sales
{
    /// <summary>
    /// Clase para la validación de devoluciones
    /// </summary>
    [DataContract]
    public class ReturnOfSaleValidationRequest
    {

        /// <summary>
        /// Folio de venta asignado
        /// </summary>
        [DataMember(Name = "folioVenta")]
        public string FolioVenta { get; set; }

        /// <summary>
        /// Linea Ticket
        /// </summary>
        [DataMember(Name = "linea")]
        public LineaTicket Linea { get; set; }

    }
}
