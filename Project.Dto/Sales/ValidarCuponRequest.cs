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
    public class ValidarCuponRequest
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
        /// Saldo del cupon a validar
        /// </summary>
        [DataMember(Name = "saldoARedimir")]
        public double saldoARedimir { get; set; }

    }
}
