using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.Sales
{
    /// <summary>
	/// Peticion para el sp de validacion de cupones
	/// </summary>
	[DataContract]
    public class CuponRedimirRequest
    {
        /// <summary>
		/// Folio del cupon a redimir
		/// </summary>
		[DataMember(Name = "folioCupon")]
        public string FolioCupon { get; set; }
        /// <summary>
		/// Folio de la venta
		/// </summary>
		[DataMember(Name = "folioVenta")]
        public string FolioVenta { get; set; }
        /// <summary>
        /// Codigo de la tienda
        /// </summary>
        [DataMember(Name = "codigoTienda")]
        public int CodigoTienda { get; set; }
        /// <summary>
        /// Codigo de la caja
        /// </summary>
        [DataMember(Name = "codigoCaja")]
        public int CodigoCaja { get; set; }
        /// <summary>
        /// Saldo del cupon a redimir
        /// </summary>
        [DataMember(Name = "saldoCupon")]
        public double SaldoCupon { get; set; }
        /// <summary>
        /// Codigo de la promocion para redencion
        /// </summary>
        [DataMember(Name = "codigoPromocion")]
        public int CodigoPromocion { get; set; }

        /// <summary>
        /// Codigo de la promocion para redencion
        /// </summary>
        [DataMember(Name = "Sesion")]
        public int Sesion { get; set; }
    }
}
