using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.Sincronizacion
{
    /// <summary>
    /// DTO Información correspondiente a información que debe sincronizarse
    /// </summary>
    [DataContract]
    public class SentenciaSQL
    {

        /// <summary>
        /// Identificador de tabla sincronización
        /// </summary>
        [DataMember(Name = "id")]
        public int Id { get; set; }

        /// <summary>
        /// Sentencia SQL que debe sincronizarse
        /// </summary>
        [DataMember(Name = "sentencia")]
        public string Sentencia { get; set; }

    }
}
