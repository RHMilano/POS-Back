using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{
	/// <summary>
	/// Dto anular totalizar apartado
	/// </summary>
	[DataContract]
	public class AnularTotalizarApartadoRequest
	{

		/// <summary>
		/// Folio de la venta
		/// </summary>
		[DataMember(Name = "folioApartado")]
		public string FolioApartado { get; set; }

		/// <summary>
		/// Identificador del código de razón de la cancelación/anulación de la transacción
		/// </summary>
		[DataMember(Name = "codigoRazon")]
		public int CodigoRazon { get; set; }
	}
}
