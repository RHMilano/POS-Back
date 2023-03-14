using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.Sales
{
    /// <summary>
    /// DTO con Información posterior a la totalización de una devolución
    /// </summary>
    [DataContract]
    public class InformacionAsociadaDevolucion
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

    }
}
