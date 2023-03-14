using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;


namespace Milano.BackEnd.Dto
{

	/// <summary>
	///Informacion del pago de tarjeta milano
	/// </summary>
	[DataContract]
	public class InformacionPagoTCMM
	{
		/// <summary>
		/// Numero de tarjeta a pagar
		/// </summary>
		[DataMember(Name = "numeroTarjeta")]
		public string NumeroTarjeta { get; set; }
	}
}
