using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.InicioFinDia
{
    /// <summary>
    /// DTO Respuesta a la petición Fin de Día
    /// </summary>
    [DataContract]
    public class FinDiaResponse
    {

        /// <summary>
        /// Indica si la acción de fin de día está permitida
        /// </summary>
        [DataMember(Name = "finDiaPermitido")]
        public bool FinDiaPermitido { get; set; }

        /// <summary>
        /// Indica si debe solicitarse la captura de Luz
        /// </summary>
        [DataMember(Name = "requiereCapturarLuz")]
        public bool RequiereCapturarLuz { get; set; }

        /// <summary>
        /// Indica si debe solicitarse la captura de Luz de Inicio de día para escenarios Offline
        /// </summary>
        [DataMember(Name = "requiereCapturarLuzInicioDia")]
        public bool RequiereCapturarLuzInicioDia { get; set; }

        /// <summary>
        /// Mensaje asociado
        /// </summary>
        [DataMember(Name = "mensajeAsociado")]
        public string MensajeAsociado { get; set; }

        /// <summary>
        /// Resultados de la validación de las cajas
        /// </summary>
        [DataMember(Name = "resultadosValidacionesCajas")]
        public ResultadoValidacionCaja[] ResultadosValidacionesCajas { get; set; }

    }
}
