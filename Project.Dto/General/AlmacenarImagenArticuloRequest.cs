using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.General
{
    /// <summary>
    /// Clase de peticion para almacenar la imagen localmente de un artículo remoto
    /// </summary>
    [DataContract]
    public class AlmacenarImagenArticuloRequest
    {

        /// <summary>
        /// Liga HTTP de la imagen remota
        /// </summary>
        [DataMember(Name = "url")]
        public string Url { get; set; }

    }

}
