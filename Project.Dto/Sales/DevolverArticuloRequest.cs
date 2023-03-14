using Milano.BackEnd.Dto.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.Sales
{
    /// <summary>
    /// Clase DTO para devolver un artículo
    /// </summary>
    [DataContract]
    public class DevolverArticuloRequest
    {

        /// <summary>
        /// Linea del artículo correspondiente
        /// </summary>
        [DataMember(Name = "lineaTicket")]
        public LineaTicket LineaTicket { get; set; }

        /// <summary>
        /// Identificador del código de razón de la devolución
        /// </summary>
        [DataMember(Name = "codigoRazon")]
        public int CodigoRazon { get; set; }

    }
}
