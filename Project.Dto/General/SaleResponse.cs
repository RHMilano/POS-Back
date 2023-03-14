using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.General
{
    /// <summary>
    /// Clase de respuesta para obtener información sobre el tipo de cambio
    /// </summary>
    [DataContract]
    public class SaleResponse
    {
        /// <summary>
        ///
        /// </summary>
        [DataMember(Name = "idTrans")]
        public string IdTrans { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "authoNumber")]
        public string AuthoNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "responseCode")]
        public string ResponseCode { get; set; }

        /// <summary>
        ///
        /// </summary>
        [DataMember(Name = "responseMessage")]
        public string responseMessage { get; set; }
    }

    /// <summary>
    /// Clase de respuesta para obtener información sobre el tipo de cambio
    /// </summary>
    [DataContract]
    public class VerifySale
    {
      
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "authoNumber")]
        public string AuthoNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "verifyResponseCode")]
        public string VerifyResponseCode { get; set; }

        /// <summary>
        ///
        /// </summary>
        [DataMember(Name = "verifyResponseMessage")]
        public string verifyResponseMessage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "transactionResponseCode")]
        public string TransactionResponseCode { get; set; }

        /// <summary>
        ///
        /// </summary>
        [DataMember(Name = "transactionResponseMessage")]
        public string TransactionResponseMessage { get; set; }

    }




}
