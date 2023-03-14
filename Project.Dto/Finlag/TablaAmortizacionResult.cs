using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.Finlag
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class TablaAmortizacionResult
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "fechaPago")]
        public string FechaPago { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "numPago")]
        public string NumeroPago { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "capital")]
        public string Capital { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "total")]
        public decimal Total { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "totalQuincenal")]
        public decimal TotalQuincenal { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "tipoPago")]
        public string TipoPago { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "estatusVale")]
        public string EstatusVale { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "efectivoPuntos")]
        public string EfectivoPuntos { get; set; }

        [DataMember(Name = "puntos")]
        public string Puntos { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "NumeroCodigo")]
        public int NumeroCodigo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "DescripcionCodigo")]
        public string DescripcionCodigo { get; set; }
    }
}
