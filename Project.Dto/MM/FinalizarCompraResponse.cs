using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.MM
{
    /// <summary>
    /// Clase que devuelve el resultado de una operacion en base de datos
    /// </summary>
    [DataContract]
    public class FinalizarCompraResponse
    {
        /// <summary>
		/// Codigo de respuesta
		/// </summary>
        [DataMember(Name = "codigoRespuestaTCMM")]
        public CodigoRespuestaTCMM CodigoRespuestaTCMM { get; set; }

        /// <summary>
		/// Número de autorización bancaria
		/// </summary>
		[DataMember(Name = "authorization")]
        public string Authorization { get; set; }
    }
}
