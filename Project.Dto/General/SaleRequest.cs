using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.General
{
    /// <summary>
    /// Clase de respuesta para obtener información sobre el tipo de cambio
    /// </summary>
    [DataContract]
    public class SaleRequest
    {
        /// <summary>
        ///
        /// </summary>
        [DataMember(Name = "storeTeller")]
        public int StoreTeller { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "currencyCode")]
        public string CurrencyCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "clientCode")]
        public int ClientCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "receivedAmount")]
        public decimal ReceivedAmount { get; set; }

        /// <summary>
        ///
        /// </summary>
        [DataMember(Name = "purchaseAmount")]
        public decimal PurchaseAmount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "usedExchangeRate")]
        public decimal UsedExchangeRate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "purchaseAmountCurrency")]
        public string PurchaseAmountCurrency { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "idMerchantTransaction")]
        public string IdMerchantTransaction { get; set; }

        /// <summary>
        ///
        /// </summary>
        [DataMember(Name = "posTimestamp")]
        public string PosTimestamp { get; set; }
    }
}
