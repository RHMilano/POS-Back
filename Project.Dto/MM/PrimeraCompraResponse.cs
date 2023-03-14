using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{
    /// <summary>
    /// Respuesta de validacion de primera compra
    /// </summary>
    [DataContract]
    public class PrimeraCompraResponse
    {
        /// <summary>
        /// Codigo de error
        /// </summary>
        [DataMember(Name = "codigoError")]
        public string CodigoError { get; set; }

        /// <summary>
        /// Codigo de error
        /// </summary>
        [DataMember(Name = "mensaje")]
        public string Mensaje { get; set; }

        /// <summary>
        /// Codigo de error
        /// </summary>
        [DataMember(Name = "respuesta")]
        public string Respuesta { get; set; }

    }
}
