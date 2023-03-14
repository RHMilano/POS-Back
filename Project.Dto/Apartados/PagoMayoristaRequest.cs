using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{
	/// <summary>
	/// Pago de mayorista
	/// </summary>
	[DataContract]
	public class PagoCreditoMayoristaRequest
	{
		/// Folio de operación asociada al pago
		/// </summary>
		[DataMember(Name = "folioOperacionAsociada")]
		public string FolioOperacionAsociada { get; set; }

			/// <summary>
		/// Importe venta total
		/// </summary>
		[DataMember(Name = "importePago")]
		public decimal ImportePago { get; set; }

		/// <summary>
		/// Código de mayorista
		/// </summary>
		[DataMember(Name = "codigoMayorista")]
		public int CodigoMayorista { get; set; }



	}
}
