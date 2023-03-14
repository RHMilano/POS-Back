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
    public class ConsultaValeFinlagRequest
    {
        /// <summary>
        /// Folio del vale a consultar
        /// </summary>
        [DataMember(Name = "folioVale")]
        public string FolioVale { get; set; }
    }
}
