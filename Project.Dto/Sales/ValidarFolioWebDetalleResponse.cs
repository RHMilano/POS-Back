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
    public class ValidarFolioWebDetalleResponse
    {
        /// <summary>
        /// Número de Orden Shopify
        /// </summary>
        [DataMember(Name = "orderNuber")]
        public string OrderNuber { get; set; }

        /// <summary>
        /// ID de Orden Shopify
        /// </summary>
        [DataMember(Name = "orderId")]
        public string OrderId { get; set; }

        /// <summary>
        /// Nombre del Cliente
        /// </summary>
        [DataMember(Name = "customerName")]
        public string CustomerName { get; set; }

        /// <summary>
        /// Número de transacción
        /// </summary>
        [DataMember(Name = "transactionId")]
        public string TransactionId { get; set; }

        /// <summary>
        /// Monto
        /// </summary>
        [DataMember(Name = "amount")]
        public decimal Amount { get; set; }

        /// <summary>
        /// Moneda
        /// </summary>
        [DataMember(Name = "currency")]
        public string Currency { get; set; }

    }
}
