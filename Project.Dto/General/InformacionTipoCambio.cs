using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.General
{
    /// <summary>
    /// Clase para albergar la información del tipo de cambio asociada a un movimiento
    /// </summary>
    [DataContract]
    public class InformacionTipoCambio
    {

        /// <summary>
        /// Código del tipo de divisa al que se desea convertir la equivalencia
        /// </summary>
        [DataMember(Name = "codigoTipoDivisa")]
        public string CodigoTipoDivisa { get; set; }

        /// <summary>
        /// Importe equivalente expresado en moneda extranjera
        /// </summary>
        [DataMember(Name = "importeMonedaExtranjera")]
        public decimal ImporteMonedaExtranjera { get; set; }

        /// <summary>
        /// Tasa de conversión utilizada
        /// </summary>
        [DataMember(Name = "tasaConversionVigente")]
        public decimal TasaConversionVigente { get; set; }

    }
}
