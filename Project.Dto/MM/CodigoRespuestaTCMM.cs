using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.MM
{
    /// <summary>
    /// DTO Información correspondiente a códigos de respuesta de TCMM
    /// </summary>
    [DataContract]
    public class CodigoRespuestaTCMM
    {

        /// <summary>
        /// Código de Respuesta TCMM
        /// </summary>
        [DataMember(Name = "codigoRespuesta")]
        public string CodigoRespuesta { get; set; }

        /// <summary>
        /// Mensaje de Retorno TCMM
        /// </summary>
        [DataMember(Name = "mensajeRetorno")]
        public string MensajeRetorno { get; set; }

    }
}
