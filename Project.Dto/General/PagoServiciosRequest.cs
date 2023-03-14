using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{
	
	[DataContract]
	public class PagoServiciosRequest
	{
		/// <summary>
		/// Codigo de SKU de servicio
		/// </summary>
		[DataMember(Name = "skuCodePagoServicio")]
		public string SkuCodePagoServicio { get; set; }


		/// <summary>
		/// Codigo de SKU de servicio
		/// </summary>
		[DataMember(Name = "skuCode")]
		public int SkuCode { get; set; }


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
