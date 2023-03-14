using Milano.BackEnd.Dto.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{

    /// <summary>
    /// Clase que representa la respuesta a una petición de estatus de una actualización de versión de software
    /// </summary>
    [DataContract]
    public class EstatusActualizacionSoftwareResponse
    {

        /// <summary>
        /// IdVersion del Software
        /// </summary>
        [DataMember(Name = "idVersion")]
        public int IdVersion { get; set; }

        /// <summary>
        /// Estatus actual del proceso
        /// </summary>
        [DataMember(Name = "estatusProceso")]
        public string EstatusProceso { get; set; }

        /// <summary>
        /// Descripción para el usuario final sobre el estatus actual del proceso
        /// </summary>
        [DataMember(Name = "descripcionEtatusProceso")]
        public string DescripcionEtatusProceso { get; set; }

        /// <summary>
        /// Log adicional del proceso
        /// </summary>
        [DataMember(Name = "logProceso")]
        public string LogProceso { get; set; }

        /// <summary>
        /// Indica si hay un proceso de actualización en curso
        /// </summary>
        [DataMember(Name = "procesoActualizacionEnCurso")]
        public int ProcesoActualizacionEnCurso { get; set; }

        /// <summary>
        /// Fecha de inicio del proceso de actualización
        /// </summary>
        [DataMember(Name = "fechaInicioProcesoActualizacion")]
        public string FechaInicioProcesoActualizacion { get; set; }

        /// <summary>
        /// Fecha de fin del proceso de actualización
        /// </summary>
        [DataMember(Name = "fechaFinProcesoActualizacion")]
        public string FechaFinProcesoActualizacion { get; set; }

        /// <summary>
        /// Fecha en que se hizo la ultima actualización
        /// </summary>
        [DataMember(Name = "fechaUltimaActualizacion")]
        public string FechaUltimaActualizacion { get; set; }

    }
}
