using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Milano.BackEnd.Dto
{
	/// <summary>
	/// Dto compañias de tiempo aire
	/// </summary>
	public class CompaniasPagoServicioResponse
	{
		/// <summary>
		/// Codigo de la compañia
		/// </summary>
		public string Codigo { get; set; }
		/// <summary>
		/// Marca de la compañia
		/// </summary>
		public string Marca { get; set; }
		/// <summary>
		/// Nombre de la compañia
		/// </summary>
		public string Nombre { get; set; }
	}
}
