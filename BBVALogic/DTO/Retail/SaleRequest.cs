using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace BBVALogic.DTO.Retail
{
    [DataContract]
    public class SaleRequest
    {
        /// <summary>
        /// Monto de la transacción
        /// </summary>
        ///  [DataMember(Name = "id")]
        [DataMember(Name = "transactionAmount")]
        public decimal TransactionAmount { get; set; }
        /// <summary>
        /// Número de ticket del POS
        /// </summary>
        [DataMember(Name = "merchanReference")]
        public string MerchanReference { get; set; }

        /// <summary>
        /// Paga con dolares?
        /// </summary>
        [DataMember(Name = "dollars")]
        public bool Dollars { get; set; }

        /// <summary>
        /// Paga con AMEX
        /// </summary>
        [DataMember(Name = "amex")]
        public bool Amex { get; set; }

        /// <summary>
        /// Promociones de meses sin intereses
        /// </summary>
       [DataMember(Name = "promo")]
        public int Promo { get; set; }

        /// <summary>
        /// Pagar con puntos
        /// </summary>
        [DataMember(Name = "payPoints")]
        public bool PayPoints { get; set; }
    }
}
