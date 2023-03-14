using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{

	/// <summary>
	/// DTO apartados plazos
	/// </summary>
	[DataContract]
	public class ApartadoPlazosResponse
	{
		/// <summary>
		/// Código de plazo
		/// </summary>
		[DataMember(Name = "codigoPlazo")]
		public int CodigoPlazo { get; set; }
		/// <summary>
		/// Dias del plazo
		/// </summary>
		[DataMember(Name = "dias")]
		public int Dias { get; set; }
		/// <summary>
		/// Descripción del plazo
		/// </summary>
		[DataMember(Name = "descripcion")]
		public string Descripcion { get; set; }

	}
}
