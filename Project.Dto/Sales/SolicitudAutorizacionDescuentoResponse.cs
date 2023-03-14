using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Milano.BackEnd.Dto.Sales;

namespace Milano.BackEnd.Dto
{

    /// <summary>
    /// 
    /// </summary>

    [DataContract]
    public class SolicitudAutorizacionDescuentoResponse
    {
        /// <summary>
        /// Mensaje del resultado de la solicitud de autorizacion
        /// </summary>
        [DataMember(Name = "mensaje")]
        public string Mensaje { get; set; }
       
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "codeNumber")]
        public string CodeNumber { get; set; }

        /// <summary>
        /// Estatus de la solictud 
        /// </summary>
        [DataMember(Name = "status")]
        public int Status { get; set; }




    }
}
