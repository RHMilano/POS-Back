using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{
	[DataContract]
	public class InfoElementosRequest
	{
		/// <summary>
		/// Codigo de SKU de recarga
		/// </summary>
		[DataMember(Name = "skuCode")]
		public string SkuCode { get; set; }
		
		/// <summary>
		/// Telefono
		/// </summary>
		[DataMember(Name = "cuenta")]
		public string Cuenta { get; set; }


		/// <summary>
		/// Informacion adicinoal de pago de servicios
		/// </summary>
		[DataMember(Name = "infoAdicional")]
		public PagoServiciosInfoAdicional InfoAdicional { get; set; }


	}
}
