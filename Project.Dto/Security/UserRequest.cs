using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
namespace Milano.BackEnd.Dto
{
	/// <summary>
	/// Clase de usuaurio que viaja como petición
	/// </summary>
	[DataContract]
	public class UserRequest
	{
		/// <summary>
		/// Numero de empleado
		/// </summary>
		[DataMember(Name = "numberEmployee")]
		public int NumberEmployee { get; set; }
		/// <summary>
		/// Contraseña del empleado
		/// </summary>
		[DataMember(Name = "password")]
		public string Password { get; set; }
		/// <summary>
		/// Numero de intentos en el login
		/// </summary>
		[DataMember(Name = "numberAttempts")]
		public int NumberAttempts { get; set; }

		/// <summary>
		/// Token del dispositivo que intenta accesar
		/// </summary>
		[DataMember(Name = "tokenDevice")]
		public string TokenDevice { get; set; }

		/// <summary>
		/// Solo cuando es el login inicial, para validar que vengan de un logout
		/// </summary>
		[DataMember(Name = "esLoginInicial")]
		public int esLoginInicial { get; set; }
	}
}
