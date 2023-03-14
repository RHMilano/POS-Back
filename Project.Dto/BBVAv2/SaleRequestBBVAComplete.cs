using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.BBVAv2
{
    /// <summary>
    /// Solicitud inicial para leer la tarjeta en la versión 2 de BBVA
    /// </summary>
    [DataContract]
    public class SaleRequestBBVAComplete
    {
        /// <summary>
        /// Se define como valor por defecto cero "0", por que es el valor que indica
        /// que si se va a manejar una promición
        /// </summary>
        public SaleRequestBBVAComplete() {
            Promo = 1;
        }
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
