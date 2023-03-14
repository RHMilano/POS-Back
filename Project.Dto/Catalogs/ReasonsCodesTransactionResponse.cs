using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{
    /// <summary>
    /// Clase de Respuesta de los codigos de razón
    /// </summary>
    [DataContract]
    public class ReasonsCodesTransactionResponse
    {
        /// <summary>
        /// Identificador del código de razón
        /// </summary>
        [DataMember(Name = "codigoRazon")]
        public int CodigoRazon { get; set; }

        /// <summary>
        /// Código de razón de MMS
        /// </summary>
        [DataMember(Name = "codigoRazonMMS")]
        public string CodigoRazonMMS { get; set; }

        /// <summary>
        /// Descripción del código de razón
        /// </summary>
        [DataMember(Name = "descripcionRazon")]
        public string DescripcionRazon { get; set; }

    }
}
