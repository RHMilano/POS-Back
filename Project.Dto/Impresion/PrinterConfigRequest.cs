using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.Impresion
{
    /// <summary>
    /// Clase para obtener la configuracion de la impresora de tickets
    /// </summary>
    [DataContract]
   public class PrinterConfigRequest
    {
        /// <summary>
        /// Codigo de la tienda
        /// </summary>
        [DataMember(Name = "codigoTienda")]
        public int CodigoTienda { get; set; }

        /// <summary>
        /// Codigo de la Caja
        /// </summary>
        [DataMember(Name = "codigoCaja")]
        public int CodigoCaja { get; set; }

    }
}
