using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{
	/// <summary>
	/// Datos de tarjeta de regalo
	/// </summary>
	[DataContract]
	public class BusquedaTarjetasRegalo
	{
		/// <summary>
		/// Folio de la tarjeta de regalo
		/// </summary>
		[DataMember(Name = "folioTarjetaRegalo")]
		public int FolioTarjetaRegalo { get; set; }

		/// <summary>
		/// Estatus de la tarjeta de regalo
		/// </summary>
		[DataMember(Name = "estatus")]
		public string Estatus { get; set; }
	}
}
