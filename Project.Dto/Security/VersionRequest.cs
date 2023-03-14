using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Milano.BackEnd.Dto.Security
{
	/// <summary>
	/// Clase de usuaurio que viaja como petición
	/// </summary>
	[DataContract]
	public class VersionRequest
    {
		
		/// <summary>
		/// Contraseña del empleado
		/// </summary>
		[DataMember(Name = "versionPOS")]
		public string versionPOS { get; set; }

	}
}
