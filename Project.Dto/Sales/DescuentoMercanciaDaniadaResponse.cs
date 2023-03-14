using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{

    [DataContract]
    public class DescuentoMercanciaDaniadaResponse
    {

        [DataMember(Name = "error")]
        public string Error { get; set; }

        [DataMember(Name = "mensaje")]
        public string Mensaje { get; set; }

        [DataMember(Name = "porcentanjeDescuento")]
        public decimal PorcentanjeDescuento { get; set; }

        [DataMember(Name = "uLSession")]
        public string ULSession { get; set; }

        /// <summary>
        /// Codigo de razon del descuento
        /// </summary>
        [DataMember(Name = "codigoRazon")]
        public int CodigoRazon { get; set; }

    }
}
