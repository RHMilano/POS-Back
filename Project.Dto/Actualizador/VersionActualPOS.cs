using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.Actualizador
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class VersionActualPOS
    {
        /// <summary>
        /// Identificador de la versión base o actual del Software
        /// </summary>
        [DataMember(Name = "idVersion")]
        public int IdVersion { get; set; }

        /// <summary>
        /// Etiqueta de Lanzamiento de la versión base o actual del Software
        /// </summary>
        [DataMember(Name = "etiquetaLanzamientoVersionBase")]
        public string EtiquetaLanzamientoVersionBase { get; set; }
    }
}
