using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{
    /// <summary>
    /// DTO de elemento tipo Select
    /// </summary>
    [DataContract]
    public class SelectElement
    {



        /// <summary>
        /// Opciones del elemento Select
        /// </summary>
        [DataMember(Name = "opciones")]
        public OpcionSelect[] Opciones { get; set; }

    }
}
