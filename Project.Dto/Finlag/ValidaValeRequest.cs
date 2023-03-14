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
    public class ValidaValeRequest
    {
        /// <summary>
        /// Folio del vale a consultar
        /// </summary>
        [DataMember(Name = "idDistribuidora")]
        public int IdDistribuidora { get; set; }

        /// <summary>
        /// Folio del vale a consultar
        /// </summary>
        [DataMember(Name = "folioVale")]
        public string FolioVale { get; set; }

        /// <summary>
        /// Folio del vale a consultar
        /// </summary>
        [DataMember(Name = "montoVale")]
        public double Monto { get; set; }
    }
}
