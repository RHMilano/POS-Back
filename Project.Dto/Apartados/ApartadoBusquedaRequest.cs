using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{
	/// <summary>
	/// Dto busqueda de apartados
	/// </summary>
	[DataContract]
	public class ApartadoBusquedaRequest
	{
		/// <summary>
		/// Folio de apartado
		/// </summary>
		[DataMember(Name = "folioApartado")]
		public string FolioApartado { get; set; }

		/// <summary>
		/// Telefono del cliente del apartado
		/// </summary>
		[DataMember(Name = "telefono")]
		public string Telefono { get; set; }

		/// <summary>
		///Nombre del cliente del apartado
		/// </summary>
		[DataMember(Name = "nombre")]
		public string Nombre { get; set; }


	}
}
