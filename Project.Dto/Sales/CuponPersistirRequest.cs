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
   public class CuponPersistirRequest
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
        /// Codigo de empleado
        /// </summary>
        [DataMember(Name = "codigoEmpleado")]
        public int CodigoEmpleado { get; set; }
        /// <summary>
        /// Saldo del cupon a redimir
        /// </summary>
        [DataMember(Name = "maximoRedencion")]
        public decimal MaximoRedencion { get; set; }
        /// <summary>
        /// Codigo de la promocion para redencion
        /// </summary>
        [DataMember(Name = "transaccion")]
        public int Transaccion { get; set; }

        /// <summary>
        /// Codigo de la promocion para redencion
        /// </summary>
        [DataMember(Name = "Sesion")]
        public long Sesion { get; set; }
    }
}
