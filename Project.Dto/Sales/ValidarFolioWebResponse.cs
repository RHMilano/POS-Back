using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{
    /// <summary>
    /// DTO Venta por Folio
    /// </summary>
    [DataContract]
    public class ValidarFolioWebResponse
    {
        /// <summary>
        /// Código de Error
        /// </summary>
        [DataMember(Name = "errorCode")]
        public int ErrorCode { get; set; }

        /// <summary>
        /// Descripción del error
        /// </summary>
        [DataMember(Name = "message")]
        public string Message { get; set; }

        /// <summary>
        /// Total venta con impuestos original
        /// </summary>
        [DataMember(Name = "response")]
        public ValidarFolioWebDetalleResponse Response { get; set; }


      

    }
}
