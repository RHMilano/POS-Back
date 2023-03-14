using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{
	/// <summary>
	/// Dto respuesta de la busqeuda de mayoristas
	/// </summary>
	[DataContract]
	public class BusquedaMayoristaResponse
	{
		/// <summary>
		/// Código de mayorista
		/// </summary>
		[DataMember(Name = "codigoMayorista")]
		public int CodigoMayorista { get; set; }


		/// <summary>
		/// Mensaje de Error
		/// </summary>
		[DataMember(Name = "error")]
		public string Error { get; set; }

		/// <summary>
		/// Mensaje de busqueda
		/// </summary>
		[DataMember(Name = "mensaje")]
		public string Mensaje { get; set; }

		/// <summary>
		/// Estatus del mayorista
		/// </summary>
		[DataMember(Name = "estatus")]
		public string Estatus { get; set; }

		/// <summary>
		/// Nombre del mayorista
		/// </summary>
		[DataMember(Name = "nombre")]
		public string Nombre { get; set; }

		/// <summary>
		/// Calle del mayorista
		/// </summary>
		[DataMember(Name = "calle")]
		public string Calle { get; set; }

		/// <summary>
		/// Numero exterior del mayorista
		/// </summary>
		[DataMember(Name = "numeroExterior")]
		public string NumeroExterior { get; set; }

		/// <summary>
		/// Numero interior del mayorista
		/// </summary>
		[DataMember(Name = "numeroInterior")]
		public string NumeroInterior { get; set; }

		/// <summary>
		/// Colonia del mayorista
		/// </summary>
		[DataMember(Name = "colonia")]
		public string Colonia { get; set; }

		/// <summary>
		/// Código postal
		/// </summary>
		[DataMember(Name = "codigoPostal")]
		public int CodigoPostal { get; set; }

		/// <summary>
		/// Municipio
		/// </summary>
		[DataMember(Name = "municipio")]
		public string Municipio { get; set; }

		/// <summary>
		/// Ciudad
		/// </summary>
		[DataMember(Name = "ciudad")]
		public string Ciudad { get; set; }

		/// <summary>
		/// Estado
		/// </summary>
		[DataMember(Name = "estado")]
		public string Estado { get; set; }

		/// <summary>
		/// RFC
		/// </summary>
		[DataMember(Name = "rfc")]
		public string RFC { get; set; }

		/// <summary>
		/// Telefono
		/// </summary>
		[DataMember(Name = "telefono")]
		public string Telefono { get; set; }

		/// <summary>
		/// Limite de credito
		/// </summary>
		[DataMember(Name = "limiteCredito")]
		public decimal LimiteCredito { get; set; }

		/// <summary>
		/// Saldo
		/// </summary>
		[DataMember(Name = "saldo")]
		public decimal Saldo { get; set; }

		/// <summary>
		/// Credito disponible
		/// </summary>
		[DataMember(Name = "creditoDisponible")]
		public decimal CreditoDisponible { get; set; }

		/// <summary>
		/// Codigo de tienda
		/// </summary>
		[DataMember(Name = "codigoTienda")]
		public int CodigoTienda { get; set; }

		/// <summary>
		/// Fecha de ultimo pago
		/// </summary>
		[DataMember(Name = "fechaUltimoPago")]
		public string  FechaUltimoPago { get; set; }

		/// <summary>
		/// Porcentaje de comisión
		/// </summary>
		[DataMember(Name = "porcentajeComision")]
		public decimal PorcentajeComision { get; set; }

		/// <summary>
		/// Pagos de periodo actual
		/// </summary>
		[DataMember(Name = "pagosPeriodoActual")]
		public decimal PagosPeriodoActual { get; set; }

		/// <summary>
		/// Estado de cuenta de mayorista
		/// </summary>
		[DataMember(Name = "estadoCuentaMayorista")]
		public EstadoCuentaMayorista EstadoCuentaMayorista { get; set; }

	}
}
