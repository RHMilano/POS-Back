using Milano.BackEnd.Dto.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.Sales
{
    /// <summary>
    /// Clase para ejecutar un cambio de precio
    /// </summary>
    [DataContract]
    public class CambiarPrecioRequest
    {

        /// <summary>
        /// Linea de artículo correspondiente
        /// </summary>
        [DataMember(Name = "lineaTicket")]
        public LineaTicket LineaTicket { get; set; }

        /// <summary>
        /// Identificador del código de razón del cambio de precio
        /// </summary>
        [DataMember(Name = "codigoRazon")]
        public int CodigoRazon { get; set; }

    }
}
