using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{
	/// <summary>
	/// Monto de recargas
	/// </summary>
	[DataContract]
	public class MontoTAResponse
	{
		/// <summary>
		/// Monto de la recarga
		/// </summary>
		[DataMember(Name = "monto")]
		public int Monto { get; set; }
		/// <summary>
		/// Sku interno
		/// </summary>
		[DataMember(Name = "sku")]
		public int Sku { get; set; }
		/// <summary>
		/// Sku de tiempo aire
		/// </summary>
		[DataMember(Name = "skuCompania")]
		public string SkuCompania { get; set; }

		/// <summary>
		/// Impuesto
		/// </summary>
		[DataMember(Name = "impuesto")]
		public Decimal  Impuesto { get; set; }
	}
}
