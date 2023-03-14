using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.General
{

    /// <summary>
    /// Clase que representa un egreso efectivo de caja
    /// </summary>
    [DataContract]
    public class Egreso
    {

        /// <summary>
        /// Monto del egreso
        /// </summary>
        [DataMember(Name = "monto")]
        public decimal monto { get; set; }

        /// <summary>
		/// Identificador del código de razón de la cancelación/anulación de la transacción
		/// </summary>
		[DataMember(Name = "codigoRazon")]
        public int CodigoRazon { get; set; }

        /// <summary>
        /// Número o folio de autorización para el egreso
        /// </summary>
        [DataMember(Name = "numeroAutorizacion")]
        public int NumeroAutorizacion { get; set; }

    }
}
