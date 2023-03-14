using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{
	/// <summary>
	/// Clase que representa la información adicional asociada a la tarjeta de regalo
	/// </summary>
	[DataContract]
	public class InformacionTarjetaRegalo
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public InformacionTarjetaRegalo()
		{
			this.Descripcion = "";
			this.Estatus = "";
		}
		/// <summary>
		/// Estatus de la tarjeta
		/// </summary>
		[DataMember(Name = "estatus")]
		public string Estatus { get; set; }

		/// <summary>
		/// Descripcion de la tarjeta
		/// </summary>
		[DataMember(Name = "descripcion")]
		public string Descripcion { get; set; }

		/// <summary>
		/// Folio de la tarjeta de regalo
		/// </summary>
		[DataMember(Name = "folioTarjeta")]
		public int FolioTarjeta { get; set; }
	}
}
