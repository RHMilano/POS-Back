using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.Finlag
{
    /// <summary>
    /// Propiedad con la cual podremos devolver el resultado al cancelar un vale aplicado
    /// </summary>
    [DataContract]
    public class CancelaAplicaValeResponse
    {
        /// <summary>
        /// Propiedad donde se almacenará el resultado al cancelar el vale aplicado
        /// </summary>
        [DataMember(Name = "cancelaAplicaValeResult")]
        public string CancelaAplicaValeResult { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "NumeroCodigo")]
        public int NumeroCodigo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "DescripcionCodigo")]
        public string DescripcionCodigo { get; set; }
    }
}
