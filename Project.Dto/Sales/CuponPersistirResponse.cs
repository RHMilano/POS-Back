using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.Sales
{
    /// <summary>
    /// Respuesta de persistencia de cupones
    /// </summary>
    [DataContract]
    public class CuponPersistirResponse
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
        /// Descripcion del codigo del resultado
        /// </summary>
        [DataMember(Name = "folioCupon")]
        public string FolioCupon { get; set; }

    }
}
