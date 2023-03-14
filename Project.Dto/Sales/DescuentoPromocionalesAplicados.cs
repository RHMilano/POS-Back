using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.Sales
{

    /// <summary>
    /// Clase que contiene los decuentos promocionales aplicados 
    /// </summary>
    [DataContract]
    public class DescuentoPromocionalesAplicados
    {

        /// <summary>
        /// Arreglo que contiene los decuentos promocionales aplicados
        /// </summary>
        [DataMember(Name = "descuentoPromocionesAplicados")]
        public DescuentoPromocionalAplicado[] DescuentoPromocionesAplicados { get; set; }

    }
}
