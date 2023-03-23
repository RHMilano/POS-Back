using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Linq;

namespace Milano.BackEnd.Dto.Lealtad
{
    /// <summary>
    /// Clase DTO para envio de parametros AcumularPuntosDescuentosRequest
    /// </summary>
    [DataContract]
    public class AcumularPuntosDescuentosResponse
    {

        /// <summary>
        /// Mensaje
        /// </summary>
        [DataMember(Name = "sMensaje")]
        public string ssMensaje { get; set; }

        /// <summary>
        /// Codigo cliente
        /// </summary>
        [DataMember(Name = "bError")]
        public bool bbError { get; set; }

    }
}
