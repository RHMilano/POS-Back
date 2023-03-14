using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{
    [DataContract]
    public class ConsultaDescuentoRequest
    {

        [DataMember(Name = "cantidad")]
        public int Cantidad { get; set; }

        /// <summary>
        /// SKU del producto
        /// </summary>
        [DataMember(Name = "sku")]
        public int Sku { get; set; }

    }
}
