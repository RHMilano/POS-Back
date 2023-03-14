using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{
    /// <summary>
    /// Clase que representa un descuento directo en una línea de artículo
    /// </summary>
    [DataContract]
    public class DescuentoDirectoLinea
    {

        /// <summary>
        /// Constructor
        /// </summary>
        public DescuentoDirectoLinea()
        {
            this.TipoDescuento = "";
        }

        /// <summary>
        /// Porcentaje del descuento
        /// </summary>
        [DataMember(Name = "porcentajeDescuento")]
        public decimal PorcentajeDescuento { get; set; }

        /// <summary>
        /// Importe del descuento
        /// </summary>
        [DataMember(Name = "importeDescuento")]
        public decimal ImporteDescuento { get; set; }

        /// <summary>
        /// Código razón del descuento
        /// </summary>
        [DataMember(Name = "codigoRazonDescuento")]
        public int CodigoRazonDescuento { get; set; }

        /// <summary>
        /// Tipo del descuento
        /// </summary>
        [DataMember(Name = "tipoDescuento")]
        public string TipoDescuento { get; set; }

        /// <summary>
        /// Session
        /// </summary>
        [DataMember(Name = "uLSession")]
        public string ULSession { get; set; }

        /// <summary>
        /// Descripcion de razon descuento 
        /// </summary>
        [DataMember(Name = "descripcionRazonDescuento")]
        public string DescripcionRazonDescuento { get; set; }


    }
}
