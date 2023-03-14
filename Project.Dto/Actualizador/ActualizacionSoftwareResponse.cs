using Milano.BackEnd.Dto.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{

    /// <summary>
    /// Clase que devuelve el resultado de la validación de versionado del POS
    /// </summary>
    [DataContract]
    public class ActualizacionSoftwareResponse
    {

        /// <summary>
        /// Constructor por default
        /// </summary>
        public ActualizacionSoftwareResponse()
        {
            this.InformacionVersionesSoftwarePendientesPorInstalar = new List<InformacionVersionSoftware>().ToArray();
        }

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
        /// Identificador de la versión base o actual del Software
        /// </summary>
        [DataMember(Name = "idVersionBase")]
        public int IdVersionBase { get; set; }

        /// <summary>
        /// Etiqueta de Lanzamiento de la versión base o actual del Software
        /// </summary>
        [DataMember(Name = "etiquetaLanzamientoVersionBase")]
        public string EtiquetaLanzamientoVersionBase { get; set; }

        /// <summary>
        /// Identificador de la versión a la que será actualizado el Software al terminar el proceso
        /// </summary>
        [DataMember(Name = "idVersionMaximaActualizacion")]
        public int IdVersionMaximaActualizacion { get; set; }

        /// <summary>
        /// Etiqueta de Lanzamiento de la versión a la que será actualizado el Software al terminar el proceso
        /// </summary>
        [DataMember(Name = "etiquetaLanzamientoVersionMaximaActualizacion")]
        public string EtiquetaLanzamientoVersionMaximaActualizacion { get; set; }

        /// <summary>
        /// Listado de versiones a las que será actualizado el Software
        /// </summary>
        [DataMember(Name = "informacionVersionesSoftwarePendientesPorInstalar")]
        public InformacionVersionSoftware[] InformacionVersionesSoftwarePendientesPorInstalar { get; set; }

    }
}
