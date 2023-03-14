using Milano.BackEnd.Dto.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{

    /// <summary>
    /// Clase que representa la respuesta a una petición de actualización de versión de software
    /// </summary>
    [DataContract]
    public class ProcesoActualizacionSoftwareResponse
    {

        /// <summary>
        /// Codigo del resultado
        /// </summary>
        [DataMember(Name = "codeNumber")]
        public string CodeNumber { get; set; }

        /// <summary>
        /// Descripcion del codigo del resultado
        /// </summary>
        [DataMember(Name = "codeDescription")]
        public string CodeDescription { get; set; }

        /// <summary>
        /// Información adicional sobre el proceso en curso
        /// </summary>
        [DataMember(Name = "informacionDetalleActualizacionEnCurso")]
        public EstatusActualizacionSoftwareResponse InformacionDetalleActualizacionEnCurso { get; set; }

    }
}
