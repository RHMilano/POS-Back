using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{
	/// <summary>
	/// Busqueda de cliente final de mayorista
	/// </summary>
	[DataContract]
	public class BusquedaClienteFinalRequest
	{
		/// <summary>
		/// Código de mayorista
		/// </summary>
		[DataMember(Name = "codigoMayorista")]
		public int CodigoMayorista { get; set; }
		/// <summary>
		/// Código de cliente final
		/// </summary>
		[DataMember(Name = "codigoClienteFinal")]
		public int CodigoClienteFinal { get; set; }
		/// <summary>
		/// Nombre ó nombres del cliente final
		/// </summary>
		[DataMember(Name = "nombres")]
		public string Nombres { get; set; }
		/// <summary>
		/// Apellidos del cliente final
		/// </summary>
		[DataMember(Name = "apellidos")]
		public string Apellidos { get; set; }
		/// <summary>
		/// INE del cliente final
		/// </summary>
		[DataMember(Name = "ine")]
		public string Ine { get; set; }
		/// <summary>
		/// RFC del cliente final
		/// </summary>
		[DataMember(Name = "rfc")]
		public string Rfc { get; set; }

	}
}
