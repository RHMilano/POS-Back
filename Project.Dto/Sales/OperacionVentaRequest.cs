using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.Sales
{

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class OperacionVentaRequest
    {

        /// <summary>
        /// Linea Ticket asociada
        /// </summary>
        [DataMember(Name = "lineaTicket")]
        public LineaTicket LineaTicket { get; set; }

        /// <summary>
        /// Información asociada de la cabecera de Venta
        /// </summary>
        [DataMember(Name = "cabeceraVentaAsociada")]
        public CabeceraVentaRequest cabeceraVentaRequest { get; set; }

    }
}
