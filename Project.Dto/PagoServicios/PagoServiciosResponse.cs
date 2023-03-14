using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{
    /// <summary>
    /// DTO Información correspondiente a pago de servicios
    /// </summary>
    [DataContract]
    public class PagoServiciosResponse
    {

        /// <summary>
        /// Modulo Id
        /// </summary>
        [DataMember(Name = "moduleId")]
        public int ModuloId { get; set; }

        /// <summary>
        /// Elementos que deben pintarse/solicitarse en la interfaz
        /// </summary>
        [DataMember(Name = "elementosFormulario")]
        public ElementoFormulario[] ElementosFormulario { get; set; }

    }
}
