using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.General
{
    /// <summary>
    /// Clase de peticion para obtener información sobre el tipo de cambio
    /// </summary>
    [DataContract]
    public class TipoCambioActualizado
    {

        /// <summary>
        /// Valor del tipo de cambio al día
        /// </summary>
        [DataMember(Name = "tipoCambio")]
        public decimal TipoCambio { get; set; }

        /// <summary>
        /// Cambio máximo que se puede dar en USD
        /// </summary>
        [DataMember(Name = "cambioMaximo")]
        public decimal CambioMaximo { get; set; }

        /// <summary>
        /// Monto máximo que se puede recibir en USD
        /// </summary>
        [DataMember(Name = "reciboMaximo")]
        public decimal ReciboMaximo { get; set; }
    }
}
