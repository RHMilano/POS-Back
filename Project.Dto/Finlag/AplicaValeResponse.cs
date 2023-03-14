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
    public class AplicaValeResponse
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "estatusMovimiento")]
        public string EstatusMovimiento { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "idTransaccion")]
        public string IdTransaccion { get; set; }

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
