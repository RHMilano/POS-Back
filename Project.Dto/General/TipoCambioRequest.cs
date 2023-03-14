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
    public class TipoCambioRequest
    {

        /// <summary>
        /// Importe expresado en moneda nacional
        /// </summary>
        [DataMember(Name = "importeMonedaNacional")]
        public decimal ImporteMonedaNacional { get; set; }

        /// <summary>
        /// Código del tipo de divisa al que se desea convertir la equivalencia
        /// </summary>
        [DataMember(Name = "codigoTipoDivisa")]
        public string CodigoTipoDivisa { get; set; }

    }
}
