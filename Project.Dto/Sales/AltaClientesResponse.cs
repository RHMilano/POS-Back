using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{
    [DataContract]
    public class AltaClientesResponse
    {
        /// <summary>
        /// Codigo del resultado
        /// </summary>
        [DataMember(Name = "codeNumber")]
        public string CodeNumber { get; set; }

        /// <summary>
        /// Descripcion del codigo del resultado
        /// </summary>
        [DataMember(Name = "codeDescription")]
        public string CodeDescription { get; set; }

        /// <summary>
        /// Codigo Cliente
        /// </summary>
        [DataMember(Name = "codigoCliente")]
        public Int64 CodigoCliente { get; set; }

    }
}
