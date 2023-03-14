using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{

    /// <summary>
    /// DTO Divisa
    /// </summary>
	[DataContract]
    public class Divisa
    {
        /// <summary>
        /// Codigo del elemento
        /// </summary>
        [DataMember(Name = "codigo")]
        public string Codigo { get; set; }

        /// <summary>
        /// Descripcion del elemento
        /// </summary>
        [DataMember(Name = "descripcion")]
        public string Descripcion { get; set; }

        /// <summary>
        /// Monto máximo que puede aceptarse al pagar
        /// </summary>
        [DataMember(Name = "montoMaximoMovimientoDivisaTransaccion")]
        public decimal MontoMaximoMovimientoDivisaTransaccion { get; set; }

        /// <summary>
        /// Monto máximo que puede otorgarse como cambio
        /// </summary>
        [DataMember(Name = "montoMaximoCambioDivisaTransaccion")]
        public decimal MontoMaximoCambioDivisaTransaccion { get; set; }

    }
}
