using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;


namespace Milano.BackEnd.Dto
{
	/// <summary>
	/// Empleado Milano
	/// </summary>
	[DataContract]
	public class EmpleadoMilanoResponse
	{
		/// <summary>
		/// Codigo de empleado
		/// </summary>
		[DataMember(Name = "codigo")]
		public Int64 Codigo { get; set; } //OCG: Se cambia el tipo para coincidir con el del WSPos

		/// <summary>
		/// Apellido paterno
		/// </summary>
		[DataMember(Name = "apellidoPaterno")]
		public string ApellidoPaterno { get; set; }

		/// <summary>
		/// Apellido Materno
		/// </summary>
		[DataMember(Name = "apellidoMaterno")]
		public string ApellidoMaterno { get; set; }

		/// <summary>
		/// Nombre 
		/// </summary>
		[DataMember(Name = "nombre")]
		public string Nombre { get; set; }

		/// <summary>
		/// Monto del descuento
		/// </summary>
		[DataMember(Name = "montoCredito")]
		public double  MontoCredito { get; set; }

		/// <summary>
		/// Mensaje del resultado de la consulta del servicio
		/// </summary>
		[DataMember(Name = "mensaje")]
		public string Mensaje { get; set; }

	}
}
