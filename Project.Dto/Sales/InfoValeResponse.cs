using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Milano.BackEnd.Dto
{
	public class InfoValeResponse
	{
		public int CodigoMayorista { get; set; }
		public int CodigoTiendaPago { get; set; }
		public string Estatus { get; set; }
		public DateTime FechaCanje { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int NoAutorizacion { get; set; }
		public int NoRevision { get; set; }
		public int Referencia { get; set; }
		public string Error { get; set; }
		public string Mensaje { get; set; }
		public DateTime FechaModificacion { get; set; }
	}

	/// <summary>
	/// 
	/// </summary>
	public class ValidaTransferenciaResponse
	{
		/// <summary>
		/// 
		/// </summary>
		public string Error { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string Mensaje { get; set; }
	}
}
