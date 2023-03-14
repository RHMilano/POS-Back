using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.General
{

    /// <summary>
    /// Clase que representa un retiro parcial de efectivo
    /// </summary>
    [DataContract]
    public class RetiroParcialEfectivo
    {

        /// <summary>
        /// Monto del retiro
        /// </summary>
        [DataMember(Name = "monto")]
        public decimal monto { get; set; }

    }
}
