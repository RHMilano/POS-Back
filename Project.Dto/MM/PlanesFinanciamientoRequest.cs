using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.MM
{
    /// <summary>
    /// DTO Información correspondiente a los planes de financiamiento
    /// </summary>
    [DataContract]
    public class PlanesFinanciamientoRequest
    {

        /// Folio de operación asociada al pago
        /// </summary>
        [DataMember(Name = "folioOperacionAsociada")]
        public string FolioOperacionAsociada { get; set; }

        /// <summary>
        /// Número de tarjeta TCMM
        /// </summary>
        [DataMember(Name = "numeroTarjeta")]
        public string NumeroTarjeta { get; set; }

    }
}
