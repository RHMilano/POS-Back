using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{
	/// <summary>
	/// Dto Estilo 
	/// </summary>
	[DataContract]
	public class EstiloDto
	{
		/// <summary>
		/// Código del estilo
		/// </summary>
		[DataMember(Name = "codigo")]
		public string Codigo { get; set; }

		/// <summary>
		/// Descripción del estilo
		/// </summary>
		[DataMember(Name = "descripcion")]
		public string Descripcion { get; set; }
	}
}
