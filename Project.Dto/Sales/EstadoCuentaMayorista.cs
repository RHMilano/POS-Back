using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{
	/// <summary>
	/// Estado de cuenta de mayorista
	/// </summary>
	[DataContract]
	public class EstadoCuentaMayorista
	{
		
		 /// <summary>
		 /// Existe el mayorista
		 /// </summary>
		[DataMember(Name = "existe")]
		public bool Existe { get; set; }

		/// <summary>
		/// Fecha de corte
		/// </summary>
		[DataMember(Name = "fechaCorte")]
		public string FechaCorte { get; set; }

		/// <summary>
		/// Anio del estado de cuenta
		/// </summary>
		[DataMember(Name = "anio")]
		public int Anio { get; set; }

		/// <summary>
		/// Periodo del estado de cuenta
		/// </summary>
		[DataMember(Name = "periodo")]
		public int Periodo { get; set; }

		/// <summary>
		/// Fecha inicial del estado de cuenta
		/// </summary>
		[DataMember(Name = "fechaInicial")]
		public string FechaInicial { get; set; }

		/// <summary>
		/// Fecha final del estado de cuenta
		/// </summary>
		[DataMember(Name = "fechaFinal")]
		public string FechaFinal { get; set; }

		/// <summary>
		/// Fecha limite de pago del mayorista
		/// </summary>
		[DataMember(Name = "fechaLimitePago")]
		public string FechaLimitePago { get; set; }
		/// <summary>
		/// Limite de credito del mayorista
		/// </summary>
		[DataMember(Name = "limiteCredito")]
		public decimal LimiteCredito { get; set; }

		/// <summary>
		/// Saldo anterior de mayorista
		/// </summary>
		[DataMember(Name = "saldoAnterior")]
		public decimal SaldoAnterior { get; set; }
		/// <summary>
		/// Compras del mayorista
		/// </summary>
		[DataMember(Name = "compras")]
		public decimal Compras { get; set; }
		/// <summary>
		/// Pagos del mayorista
		/// </summary>
		[DataMember(Name = "pagos")]
		public decimal Pagos { get; set; }
		/// <summary>
		/// Notas de credito
		/// </summary>
		[DataMember(Name = "notasDeCredito")]
		public decimal NotasDeCredito { get; set; }
		/// <summary>
		/// Notas de cargo
		/// </summary>
		[DataMember(Name = "notasDeCargo")]
		public decimal NotasDeCargo { get; set; }
		/// <summary>
		/// Saldo actual del mayorista
		/// </summary>
		[DataMember(Name = "saldoActual")]
		public decimal SaldoActual { get; set; }
		/// <summary>
		/// Pago quincenal del mayorista
		/// </summary>
		[DataMember(Name = "pagoQuincenal")]
		public decimal PagoQuincenal { get; set; }
		/// <summary>
		/// Pago minimo del mayorista
		/// </summary>
		[DataMember(Name = "pagoMinimo")]
		public decimal PagoMinimo { get; set; }
		/// <summary>
		/// Pago vencido del mayorista
		/// </summary>
		[DataMember(Name = "pagoVencido")]
		public decimal PagoVencido { get; set; }
		/// <summary>
		/// Credito disponible del mayorista
		/// </summary>
		[DataMember(Name = "creditoDisponible")]
		public decimal CreditoDisponible { get; set; }
		/// <summary>
		/// Numero de atrasos del mayorista 
		/// </summary>
		[DataMember(Name = "numeroAtrasos")]
		public int NumeroAtrasos { get; set; }
	}
}
