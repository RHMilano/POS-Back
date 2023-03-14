using Milano.BackEnd.Dto.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{

    /// <summary>
    /// Clase que devuelve el resultado de una validación u operación en general
    /// </summary>
    [DataContract]
    public class ValidationResponse
    {

        /// <summary>
        /// 
        /// </summary>
        public ValidationResponse(string codeNumber, string codeDescription)
        {
            this.CodeNumber = codeNumber;
            this.CodeDescription = codeDescription;
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
    }
}
