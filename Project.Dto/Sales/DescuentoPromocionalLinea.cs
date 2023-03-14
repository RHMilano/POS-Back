using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.Sales
{
    /// <summary>
    /// Clase que representa un descuento por motor de promociones por línea
    /// </summary>
    [DataContract]
    public class DescuentoPromocionalLinea
    {

        /// <summary>
        /// Identificador del descuento
        /// </summary>
        [DataMember(Name = "idDescuento")]
        public int IdDescuento { get; set; }

        /// <summary>
        /// Número secuencia en el Ticket
        /// </summary>
        [DataMember(Name = "secuencia")]
        public int Secuencia { get; set; }

        /// <summary>
        /// Importe descuento
        /// </summary>
        [DataMember(Name = "importeDescuento")]
        public decimal ImporteDescuento { get; set; }

        /// <summary>
        /// Código de promoción aplicado
        /// </summary>
        [DataMember(Name = "codigoPromocionAplicado")]
        public int CodigoPromocionAplicado { get; set; }

        /// <summary>
        /// Descripción código de promoción aplicado
        /// </summary>
        [DataMember(Name = "descripcionCodigoPromocionAplicado")]
        public string DescripcionCodigoPromocionAplicado { get; set; }

        /// <summary>
        /// indica el orden en el que se revisaran las promociones
        /// </summary>
        [DataMember(Name = "codigoPromocionOrden")]
        public int CodigoPromocionOrden { get; set; }

        /// <summary>
        /// Formas de pago activos para la promoción
        /// </summary>
        [DataMember(Name = "descuentosPromocionalesFormaPago")]
        public DescuentoPromocionalFormaPago[] DescuentosPromocionalesFormaPago { get; set; }
    }
}
