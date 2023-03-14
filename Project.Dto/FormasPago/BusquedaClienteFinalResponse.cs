using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{
	/// <summary>
	/// Respueta de busqueda de cliente final
	/// </summary>
	[DataContract]
	public class BusquedaClienteFinalResponse
	{
		/// <summary>
		/// Código de cliente final
		/// </summary>		
		[DataMember(Name = "codigoClienteFinal")]
		public int CodigoClienteFinal { get; set; }

		/// <summary>
		/// Código de mayorista
		/// </summary>
		[DataMember(Name = "codigoMayorista")]
		public int CodigoMayorista { get; set; }

		/// <summary>
		/// Fecha de nacimiento del cliente final
		/// </summary>
		[DataMember(Name = "fechaNacimiento")]
		public string FechaNacimiento { get; set; }

		/// <summary>
		/// Nombre ó nombre del cliente final
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
		/// <summary>
		/// Sexo del cliente final
		/// </summary>
		[DataMember(Name = "sexo")]
		public string Sexo { get; set; }
		/// <summary>
		/// Teleofno del cliente final
		/// </summary>
		[DataMember(Name = "telefono")]
		public string Telefono { get; set; }
		/// <summary>
		/// Mensaje acerca del cliente
		/// </summary>
		[DataMember(Name = "mensaje")]
		public string Mensaje { get; set; }
		/// <summary>
		/// Error en la busqueda de este cliente
		/// </summary>
		[DataMember(Name = "error")]
		public string Error { get; set; }


	}
}
