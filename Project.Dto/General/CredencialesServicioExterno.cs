using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Milano.BackEnd.Dto
{
	/// <summary>
	/// Credenciales de Pago de servicios
	/// </summary>
	public class CredencialesServicioExterno
	{
		/// <summary>
		/// Nombre de usuario
		/// </summary>
		public string UserName { get; set; }
		/// <summary>
		/// Contraseña del usuario
		/// </summary>
		public string Password { get; set; }
		/// <summary>
		/// Licencia 
		/// </summary>
		public string Licence { get; set; }
		/// <summary>
		/// Numero de intentos 
		/// </summary>
		public int NumeroIntentos { get; set; }
	
	}
}
