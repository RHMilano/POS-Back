using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{
	/// <summary>
	/// Dto tiempo aire
	/// </summary>
	[DataContract]
	public class TiempoAireRequest
	{
		/// <summary>
		/// Codigo de SKU de recarga
		/// </summary>
		[DataMember(Name = "skuCode")]
		public string SkuCode { get; set; }
		/// <summary>
		/// Telefono
		/// </summary>
		[DataMember(Name = "telefono")]
		public string Telefono { get; set; }
		/// <summary>
		/// Monto de la recarga
		/// </summary>
		[DataMember(Name = "monto")]
		public float    Monto { get; set; }
	}
}
