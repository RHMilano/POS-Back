using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{
	/// <summary>
	/// DTO Cliente
	/// </summary>
	[DataContract]
	public class ClienteRequest
	{
		/// <summary>
		/// Código de cliente 
		/// </summary>
		[DataMember(Name = "codigoCliente")]
		public Int64 CodigoCliente { get; set; }

		/// <summary>
		/// Nombre del cliente
		/// </summary>
		[DataMember(Name = "nombre")]
		public string Nombre { get; set; }

		/// <summary>
		/// Telefono del cliente
		/// </summary>
		[DataMember(Name = "telefono")]
		public string Telefono { get; set; }
	}
}
