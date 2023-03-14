using Milano.BackEnd.Dto.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.MM
{
    /// <summary>
    /// DTO Correspondiente a la respuesta de los planes de financiamiento
    /// </summary>
    [DataContract]
    public class PlanesFinanciamientoResponse
    {
        /// <summary>
		/// Codigo de respuesta
		/// </summary>
        [DataMember(Name = "codigoRespuestaTCMM")]
        public CodigoRespuestaTCMM CodigoRespuestaTCMM { get; set; }

        /// <summary>
		/// Planes de financiamiento
		/// </summary>
		[DataMember(Name = "planesFinanciamiento")]
        public List<PlanFinanciamientoResponse> PlanesFinanciamiento { get; set; }

        /// <summary>
        /// Descuentos promocional tratarse de primera compra con TCMM
        /// </summary>
        [DataMember(Name = "descuentoPromocionalPrimeraCompra")]
        public DescuentoPromocionalVenta DescuentoPromocionalPrimeraCompra { get; set; }

    }
}
