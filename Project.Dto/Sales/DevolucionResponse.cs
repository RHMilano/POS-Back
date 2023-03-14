using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{
    /// <summary>
    /// Clase de respuesta a una devolución
    /// </summary>
    [DataContract]
    public class DevolucionRespose
    {

        /// <summary>
        /// Folio de la devolución
        /// </summary>
        [DataMember(Name = "folioDevolucion")]
        public string FolioDevolucion { get; set; }

        /// <summary>
        /// Folio de la venta generada asociada
        /// </summary>
        [DataMember(Name = "folioVenta")]
        public string FolioVenta { get; set; }

    }
}
