using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.InicioFinDia
{
    /// <summary>
    /// DTO para la regresar la fecha de operración
    /// </summary>
    [DataContract]
    public class FechaOperacionResponse
    {

        /// <summary>
        /// Codigo del resultado
        /// </summary>
        [DataMember(Name = "fechaOperacion")]
        public DateTime FechaOperacion { get; set; }

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

    }
}
