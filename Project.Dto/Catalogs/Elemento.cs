using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{
	/// <summary>
	/// Dto Elemento
	/// </summary>
	[DataContract]
	public class Elemento
	{
		/// <summary>
		/// Codigo del elemento
		/// </summary>
		[DataMember(Name = "codigo")]
		public int Codigo { get; set; }
		/// <summary>
		/// Descripcion del elemento
		/// </summary>
		[DataMember(Name = "descripcion")]
		public string Descripcion { get; set; }

	}
}
