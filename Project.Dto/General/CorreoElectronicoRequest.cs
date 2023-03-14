using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Milano.BackEnd.Dto.General
{

    /// <summary>
    /// Clase de peticion de Correo Electronico
    /// </summary>
    [DataContract]
    public class CorreoElectronicoRequest
    {
        /// <summary>
        /// Tipo de Correo que se va a enviar
        /// </summary>
        [DataMember(Name = "tipoCorreo")]
        public int TipoCorreo { get; set; }
    }
}
