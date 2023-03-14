using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{
    /// <summary>
    /// Informacion de folios de tarjeta
    /// </summary>
    [DataContract]
    public class InformacionFoliosTarjeta
    {

        /// <summary>
        /// Folio de la tarjeta
        /// </summary>
        [DataMember(Name = "folioTarjeta")]
        public int FolioTarjeta { get; set; }

        /// <summary>
        /// SKU del producto
        /// </summary>
        [DataMember(Name = "sku")]
        public int Sku { get; set; }

    }
}
