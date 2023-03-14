using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.General
{
    /// <summary>
    /// Clase que devuelve el resultado de una operacion en base de datos
    /// </summary>
    [DataContract]
    public class RetiroParcialEfectivoResponse
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
        /// Folio del retiro en efectivo
        /// </summary>
        [DataMember(Name = "folioRetiro")]
        public string FolioRetiro { get; set; }
    }
}
