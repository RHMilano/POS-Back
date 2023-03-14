using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{
    /// <summary>
    /// Descuento de primera compra
    /// </summary>
    [DataContract]
    public class DescuentoTCMMPrimeraCompra
    {

        /// <summary>
        /// Bandera que indica si aplica descuneto
        /// </summary>
        [DataMember(Name = "aplicaDescuento")]
        public bool AplicaDescuento { get; set; }
        
        /// <summary>
        /// Porcentaje descuento 
        /// </summary>
        [DataMember(Name = "porcentajeDescuento")]
        public decimal PorcentajeDescuento { get; set; }

    }
}
