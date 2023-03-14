using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{
    /// <summary>
    /// Clase de peticion de los codigos de razón
    /// </summary>
    [DataContract]
    public class ReasonsCodesTransactionRequest
    {

        /// <summary>
        /// Codigo por cual se hace la búsqueda
        /// </summary>
        [DataMember(Name = "codigoTipoRazonMMS")]
        public string CodigoTipoRazonMMS { get; set; }
    }
}
