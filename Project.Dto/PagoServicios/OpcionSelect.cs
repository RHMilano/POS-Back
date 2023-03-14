using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{
    /// <summary>
    /// DTO de elemento opción para el elemento tipo Select
    /// </summary>
    [DataContract]
    public class OpcionSelect
    {

        /// <summary>
        /// Texto de la opción
        /// </summary>
        [DataMember(Name = "texto")]
        public String Texto { get; set; }

        /// <summary>
        /// Valor de la opción
        /// </summary>
        [DataMember(Name = "valor")]
        public String Valor { get; set; }

        /// <summary>
        /// Cantidad de la opción
        /// </summary>
        [DataMember(Name = "cantidad")]
        public float Cantidad { get; set; }

    }
}
