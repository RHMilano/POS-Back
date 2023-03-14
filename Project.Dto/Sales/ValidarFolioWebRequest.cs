using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{
    /// <summary>
    /// DTO Venta por Folio
    /// </summary>
    [DataContract]
    public class ValidarFolioWebRequest
    {
        /// <summary>
        /// Número de la transacción
        /// </summary>
        [DataMember(Name = "transactionId")]
        public long TransactionId { get; set; }

        /// <summary>
        /// Número de orden
        /// </summary>
        [DataMember(Name = "orderId")]
        public long OrderId { get; set; }

        /// <summary>
        /// Monto a pagar
        /// </summary>
        [DataMember(Name = "amount")]
        public decimal Amount { get; set; }

        /// <summary>
        /// Moneda del pago
        /// </summary>
        [DataMember(Name = "currency")]
        public string Currency { get; set; }


    }
}
