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
    public class DescuentoPromocionalAplicado
    {

        /// <summary>
        /// Número secuencia en el Ticket si se trata de una promocion por Linea de Ticket
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
        /// Porcentaje equivalente
        /// </summary>
        [DataMember(Name = "porcentajeDescuento")]
        public decimal PorcentajeDescuento { get; set; }

        /// <summary>
        /// Codigo razon del descuento
        /// </summary>
        [DataMember(Name = "codigoRazonDescuento")]
        public int CodigoRazonDescuento { get; set; }

        /// <summary>
        /// Forma de pago de la promoción
        /// </summary>
        [DataMember(Name = "formaPagoCodigoPromocionAplicado")]
        public string FormaPagoCodigoPromocionAplicado { get; set; }

    }
}