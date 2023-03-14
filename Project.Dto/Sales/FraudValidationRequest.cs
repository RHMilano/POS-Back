using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Milano.BackEnd.Dto.Sales
{
    /// <summary>
	/// Clase para la validción de fraude Tiempo Aire
	/// </summary>
	[DataContract]
    public class FraudValidationRequest
    {
        /// <summary>
		/// Numero Telefonico
		/// </summary>
		[DataMember(Name = "numeroTelefonico")]
        public decimal NumeroTelefonico { get; set; }

    }
}
