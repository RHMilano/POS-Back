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
    public class ExchangeRateResponse
    {
        /// <summary>
        ///
        /// </summary>
        [DataMember(Name = "responseCode")]
        public string ResponseCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "responseMessage")]
        public string ResponseMessage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "dateTime")]
        public string DateTime { get; set; }

        /// <summary>
        ///
        /// </summary>
        [DataMember(Name = "clientCode")]
        public string ClientCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "bank")]
        public string Bank { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "currencyCode")]
        public string CurrencyCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "lastDateModified")]
        public string LastDateModified { get; set; }

        /// <summary>
        ///
        /// </summary>
        [DataMember(Name = "maxAmountPerSale")]
        public decimal MaxAmountPerSale { get; set; }
        /// <summary>
		/// 
		/// </summary>
		[DataMember(Name = "maxChangePerSale")]
        public decimal MaxChangePerSale { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "minExchangeRateBuy")]
        public decimal MinExchangeRateBuy { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "maxExchangeRateBuy")]
        public decimal MaxExchangeRateBuy { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "minExchangeRateSell")]
        public decimal MinExchangeRateSell { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "maxExchangeRateSell")]
        public decimal MaxExchangeRateSell { get; set; }

    }

    ///// <summary>
    ///// 
    ///// </summary>
    //[DataContract]
    //public class Transaction
    //{
    //    /// <summary>
    //    ///
    //    /// </summary>
    //    [DataMember(Name = "responseCode")]
    //    public string ResponseCode { get; set; }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    [DataMember(Name = "responseMessage")]
    //    public string ResponseMessage { get; set; }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    [DataMember(Name = "dateTime")]
    //    public string DateTime { get; set; }

    //    /// <summary>
    //    ///
    //    /// </summary>
    //    [DataMember(Name = "clientCode")]
    //    public string ClientCode { get; set; }

    //}

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class ExchangeInfo
        {

  //      /// <summary>
  //      /// 
  //      /// </summary>
  //      [DataMember(Name = "bank")]
  //      public string Bank { get; set; }

  //      /// <summary>
  //      /// 
  //      /// </summary>
  //      [DataMember(Name = "currencyCode")]
  //      public string CurrencyCode { get; set; }

  //      /// <summary>
  //      /// 
  //      /// </summary>
  //      [DataMember(Name = "lastDateModified")]
  //      public string LastDateModified { get; set; }

  //      /// <summary>
  //      ///
  //      /// </summary>
  //      [DataMember(Name = "maxAmountPerSale")]
  //      public string MaxAmountPerSale { get; set; }
  //      /// <summary>
		///// 
		///// </summary>
		//[DataMember(Name = "maxChangePerSale")]
  //      public string MaxChangePerSale { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        //[DataMember(Name = "buy")]
        //public Buy[] Buy { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        //[DataMember(Name = "sell")]
        //public Sell[] Sell { get; set; }

    }

    ///// <summary>
    ///// 
    ///// </summary>
    //[DataContract]
    //public class Buy
    //{
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    [DataMember(Name = "minExchangeRate")]
    //    public string MinExchangeRate { get; set; }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    [DataMember(Name = "maxExchangeRate")]
    //    public string MaxExchangeRate { get; set; }

    //}

    ///// <summary>
    ///// 
    ///// </summary>
    //[DataContract]
    //public class Sell
    //{
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    [DataMember(Name = "minExchangeRate")]
    //    public string MinExchangeRate { get; set; }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    [DataMember(Name = "maxExchangeRate")]
    //    public string MaxExchangeRate { get; set; }

    //}


}
