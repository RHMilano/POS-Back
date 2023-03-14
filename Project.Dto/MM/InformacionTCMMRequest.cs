using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.MM
{
    /// <summary>
    /// DTO Información correspondiente a TCMM
    /// </summary>
    [DataContract]
    public class InformacionTCMMRequest
    {

        /// <summary>
        /// Número de tarjeta TCMM
        /// </summary>
        [DataMember(Name = "numeroTarjeta")]
        public string NumeroTarjeta { get; set; }

        /// <summary>
        /// Bandera para indicar si debe imprimirse el Ticket
        /// </summary>
        [DataMember(Name = "imprimirTicket")]
        public Boolean ImprimirTicket { get; set; }

    }
}
