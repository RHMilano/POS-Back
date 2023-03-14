using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.General
{
    /// <summary>
    /// Clase de respuesta para obtener información sobre el tipo de cambio
    /// </summary>
    [DataContract]
    public class TipoCambioResponse
    {

        /// <summary>
        /// Importe expresado en moneda nacional
        /// </summary>
        [DataMember(Name = "importeMonedaNacional")]
        public decimal ImporteMonedaNacional { get; set; }

        /// <summary>
        /// Importe expresado en moneda extranjera
        /// </summary>
        [DataMember(Name = "importeMonedaExtranjera")]
        public decimal ImporteMonedaExtranjera { get; set; }

        /// <summary>
        /// Tasa de conversión utilizada
        /// </summary>
        [DataMember(Name = "tasaConversionVigente")]
        public decimal  TasaConversionVigente { get; set; }

        /// <summary>
        /// Monto maximo a recibir en dolares
        /// </summary>
        [DataMember(Name = "montoMaximoRecibir")]
        public decimal MontoMaximoRecibir { get; set; }

        /// <summary>
        /// Monto maximo para el cambio en dolares
        /// </summary>
        [DataMember(Name = "montoMaximoCambio")]
        public decimal MontoMaximoCambio { get; set; }

        /// <summary>
        /// Mensaje de la consulta de cambio tipo de cambio
        /// </summary>
        [DataMember(Name = "mensaje")]
		public string Mensaje { get; set; }

    }
}
