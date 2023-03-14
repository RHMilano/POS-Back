using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.Impresion
{
    /// <summary>
    /// Clase para llamar el sp para los egresos
    /// </summary>
    [DataContract]
    public class PrintTicketEgresosRequest
    {
        /// <summary>
        /// Folio del retiro en efectivo
        /// </summary>
        [DataMember(Name = "folioRetiro")]
        public string FolioRetiro { get; set; }
        /// <summary>
        /// Folio del corte z para el retiro de efectivo
        /// </summary>
        [DataMember(Name = "folioCorteZ")]
        public string FolioCorteZ { get; set; }
    }
}
