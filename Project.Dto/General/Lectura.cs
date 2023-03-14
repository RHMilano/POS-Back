using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{
    /// <summary>
    /// Clase de Respuesta de los codigos de razón
    /// </summary>
    [DataContract]
    public class Lectura
    {

        /// <summary>
        /// Código de razón
        /// </summary>
        [DataMember(Name = "codigoRazon")]
        public string CodigoRazon { get; set; }

        /// <summary>
        /// Descripción del código de razón
        /// </summary>
        [DataMember(Name = "descripcionRazon")]
        public string DescripcionRazon { get; set; }

    }
}