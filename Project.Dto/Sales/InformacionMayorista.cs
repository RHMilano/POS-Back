using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.Sales
{

	[DataContract]
	public class InformacionMayorista
	{

		/// <summary>
		/// Codigo de mayorista de la venta
		/// </summary>
		[DataMember(Name = "codigoMayorista")]
		public int CodigoMayorista { get; set; }

		/// <summary>
		/// Total venta Subtotal (Sin impuestos)
		/// </summary>
		[DataMember(Name = "importeVentaBruto")]
		public decimal ImporteVentaBruto { get; set; }

		/// <summary>
		/// Total impuestos de la venta
		/// </summary>
		[DataMember(Name = "importeVentaImpuestos")]
		public decimal ImporteVentaImpuestos { get; set; }

		/// <summary>
		/// Total venta con impuestos
		/// </summary>
		[DataMember(Name = "importeVentaNeto")]
		public decimal ImporteVentaNeto { get; set; }

	}
}
