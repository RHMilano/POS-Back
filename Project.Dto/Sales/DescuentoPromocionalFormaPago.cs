using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.Sales
{
    /// <summary>
    /// Clase que representa un descuento por motor de promociones por forma de pago utilizado
    /// </summary>
    [DataContract]
    public class DescuentoPromocionalFormaPago
    {

        /// <summary>
        /// Identificador del descuento
        /// </summary>
        [DataMember(Name = "codigoFormaPago")]
        public string codigoFormaPago { get; set; }
    }
}
