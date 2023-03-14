using Milano.BackEnd.Dto.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{

    /// <summary>
    /// Clase que representa una versión geenral de software
    /// </summary>
    [DataContract]
    public class InformacionVersionSoftware
    {

        /// <summary>
        /// Id de la versión
        /// </summary>
        [DataMember(Name = "idVersion")]
        public int IdVersion { get; set; }

        /// <summary>
        /// Tipo de la actualización
        /// </summary>
        [DataMember(Name = "tipoActualizacion")]
        public string TipoActualizacion { get; set; }

        /// <summary>
        /// Etiqueta de Lanzamiento
        /// </summary>
        [DataMember(Name = "etiquetaLanzamientro")]
        public string EtiquetaLanzamientro { get; set; }

        /// <summary>
        /// Descripción
        /// </summary>
        [DataMember(Name = "descripcion")]
        public string Descripcion { get; set; }

        /// <summary>
        /// Fecha de lanzamiento
        /// </summary>
        [DataMember(Name = "fechaLanzamiento")]
        public string FechaLanzamiento { get; set; }

        /// <summary>
        /// Identificador en el repositorio
        /// </summary>
        [DataMember(Name = "identificadorRepositorio")]
        public string IdentificadorRepositorio { get; set; }

        /// <summary>
        /// Id versión minima requerida
        /// </summary>
        [DataMember(Name = "idVersionMinimaRequerida")]
        public int IdVersionMinimaRequerida { get; set; }

    }
}
