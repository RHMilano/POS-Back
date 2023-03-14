using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
namespace Milano.BackEnd.Dto
{
	/// <summary>
	/// Clase de usuario de respuesta
	/// </summary>
	[DataContract]
	public class UserResponse
	{
		/// <summary>
		/// Numero de empleado
		/// </summary>
		[DataMember(Name = "numberEmployee")]
		public int  NumberEmployee { get; set; }
		/// <summary>
		/// Descripcion del estado del usuario
		/// </summary>
		[DataMember(Name = "estatus")]
		public string Estatus { get; set; } = "";
		/// <summary>
		/// Descripcion del estado del usuario
		/// </summary>
		[DataMember(Name = "codeEstatus")]
		public int CodeEstatus { get; set; } = 0;
		/// <summary>
		/// Numero de intentos en el login
		/// </summary>
		[DataMember(Name = "numberAttempts")]
		public int NumberAttempts { get; set; } = 0;
		/// <summary>
		/// Token generado para el usuario
		/// </summary>
		[DataMember(Name = "accesstoken")]
		public string Accesstoken { get; set; } = "";

		/// <summary>
		/// Nombre de empleado
		/// </summary>
		[DataMember(Name = "nombre")]
		public string Nombre { get; set; } = "";

		/// <summary>
		/// Numero de caja
		/// </summary>
		[DataMember(Name = "numeroCaja")]
		public int NumeroCaja { get; set; }


		/// <summary>
		/// Estatus vencimiento de password usuario
		/// </summary>
		/// AHC: Propiedad Login
		[DataMember(Name = "vencioPassword")]
		public int vencioPassword { get; set; } = 0;
		// 0: No ha vencido
		// 1: Por vencer
		// 2: Vencido
	}
}
