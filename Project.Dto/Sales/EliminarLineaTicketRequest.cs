using Milano.BackEnd.Dto.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.Sales
{

    /// <summary>
    /// Clase de peticion para eliminar una línea del Ticket
    /// </summary>
    [DataContract]
    public class EliminarLineaTicketRequest
    {

        /// <summary>
        /// Secuencia original de la linea del ticket eliminada
        /// </summary>
        [DataMember(Name = "secuenciaOriginalLineaTicket")]
        public int SecuenciaOriginalLineaTicket { get; set; }

        /// <summary>
        /// Información de la linea del ticket eliminada con la secuencia correspondiente
        /// </summary>
        [DataMember(Name = "lineaTicket")]
        public LineaTicket LineaTicket { get; set; }

        /// <summary>
        /// Identificador del código de razón de la eliminación
        /// </summary>
        [DataMember(Name = "codigoRazon")]
        public int CodigoRazon { get; set; }

    }
}
