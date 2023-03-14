using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.Sales
{
    /// <summary>
	/// Dto para aplicar descuento a mercancia dañada.
	/// </summary>
    [DataContract]
    public class DescuentoMercanciaDañada
    {
        /// <summary>
		/// uLSesion
		/// </summary>
		[DataMember(Name = "uLSesion")]
        public string Sesion { get; set; }

        /// <summary>
        /// Cantidad
        /// </summary>
        [DataMember(Name = "cantidad")]
        public int Cantidad { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        [DataMember(Name = "sku")]
        public int SKU { get; set; }

        /// <summary>
        /// Transaccion
        /// </summary>
        [DataMember(Name = "transaccion")]
        public int Transaccion { get; set; }

        /// <summary>
        /// CodigoRazonDescuento
        /// </summary>
        [DataMember(Name = "codigoRazonDescuento")]
        public int CodigoRazonDescuento { get; set; }

        /// <summary>
        /// SecuenciaDetalleVenta
        /// </summary>
        [DataMember(Name = "secuenciaDetalleVenta")]
        public int SecuenciaDetalleVenta { get; set; }

    }
}
