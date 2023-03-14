using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Milano.BackEnd.Dto.General
{

    /// <summary>
    /// Clase de Respuesta de Correo Electronico
    /// </summary>
    [DataContract]
    public class CorreoElectronicoResponse
    {
        /// <summary>
        /// Destinatario
        /// </summary>
        [DataMember(Name = "destinatario")]
        public String Destinatario { get; set; }

        /// <summary>
        /// Cabecera de Correo
        /// </summary>
        [DataMember(Name = "cabecera")]
        public String Cabecera { get; set; }

        /// <summary>
        /// Contenido Correo
        /// </summary>
        [DataMember(Name = "content")]
        public String Content { get; set; }
    }
}
