using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.InicioFinDia
{
    /// <summary>
    /// DTO Respuesta a la petición Inicio de Día
    /// </summary>
    [DataContract]
    public class InicioDiaResponse
    {

        /// <summary>
        /// Indica si la acción de inicio de día está permitida
        /// </summary>
        [DataMember(Name = "inicioDiaPermitido")]
        public bool InicioDiaPermitido { get; set; }

        /// <summary>
        /// Mensaje asociado
        /// </summary>
        [DataMember(Name = "mensajeAsociado")]
        public string MensajeAsociado { get; set; }

        /// <summary>
        /// Indica si debe solicitarse la captura de Luz
        /// </summary>
        [DataMember(Name = "requiereCapturarLuz")]
        public bool RequiereCapturarLuz { get; set; }

        /// <summary>
        /// Indica si debe solicitarse la autenticación Offline
        /// </summary>
        [DataMember(Name = "requiereAutenticacionOffline")]
        public bool RequiereAutenticacionOffline { get; set; }

        /// <summary>
        /// Fecha de operacion
        /// </summary>
        [DataMember(Name = "fechaOperacion")]
        public string FechaOperacion { get; set; }

        /// <summary>
        /// Folio de operacion
        /// </summary>
        [DataMember(Name = "folioOperacion")]
        public int FolioOperacion { get; set; }

    }
}
