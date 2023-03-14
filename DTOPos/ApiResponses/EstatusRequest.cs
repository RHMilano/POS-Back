using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DTOPos.ApiResponses
{
    /// <summary>
    /// Clase que devuelve el estado de la peticion de un servicio
    /// </summary>
    [DataContract]
    public class EstatusRequest
    {
        /// <summary>
        /// Codigo devuelto
        /// </summary>
        [DataMember(Name = "codeNumber")]
        public string CodeNumber { get; set; }
        /// <summary>
        /// Descripcion del codigo
        /// </summary>
        [DataMember(Name = "codeDescription")]
        public string CodeDescription { get; set; }

        /// <summary>
        /// Mensaje detallado del error
        /// </summary>
        [DataMember(Name = "error")]
        public string Error { get; set; }

        /// <summary>
        /// Estatus de la peticion
        /// </summary>
        [DataMember(Name = "status")]
        public bool Status { get; set; }

    }
}
