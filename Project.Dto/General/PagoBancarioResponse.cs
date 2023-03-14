using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{
    /// <summary>
    /// Clase que devuelve el resultado de una operacion en base de datos
    /// </summary>
    [DataContract]
    public class PagoBancarioResponse
    {
        /// <summary>
        /// Codigo del resultado
        /// </summary>
        [DataMember(Name = "codeNumber")]
        public int CodeNumber { get; set; }

        /// <summary>
        /// Descripcion del codigo del resultado
        /// </summary>
        [DataMember(Name = "codeDescription")]
        public string CodeDescription { get; set; }

        /// <summary>
		/// Número de autorización bancaria
		/// </summary>
		[DataMember(Name = "authorization")]
        public string Authorization { get; set; }

        /// <summary>
		/// Número de autorización bancaria
		/// </summary>
		[DataMember(Name = "cardNumber")]
        public string CardNumber { get; set; }

        /// <summary>
		/// Indica si es una tarjeta de crédito o débito no aplica para AMEX ni para TCMM
		/// </summary>
		[DataMember(Name = "tipoTarjeta")]
        public string TipoTarjeta { get; set; }

        /// <summary>
		/// Inidica si es la tarjeta es valida para retirar efectivo
		/// </summary>
		[DataMember(Name = "sePuedeRetirar")]
        public bool SePuedeRetirar { get; set; }

        /// <summary>
		/// Inidica si es la tarjeta es valida para retirar efectivo
		/// </summary>
		[DataMember(Name = "sePuedePagarConPuntos")]
        public bool SePuedePagarConPuntos { get; set; }
    }
}
