using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{
	/// <summary>
	/// Reporte de ventas por departamento
	/// </summary>
	[DataContract]
	public class ReporteVentaDepartamentoResponse
	{
		/// <summary>
		/// Departamento
		/// </summary>
		[DataMember(Name = "departamento")]
		public string Departamento { get; set; }
		/// <summary>
		/// Subdepartemnto
		/// </summary>
		[DataMember(Name = "subDepartamento")]
		public string SubDepartamento { get; set; }
		/// <summary>
		/// Cantidad vendida actual
		/// </summary>
		[DataMember(Name = "cantidadVendidaActual")]
		public int CantidadVendidaActual { get; set; }
		/// <summary>
		/// Ventas con Iva actual
		/// </summary>
		[DataMember(Name = "ventasConIvaActual")]
		public decimal VentasConIvaActual { get; set; }
		/// <summary>
		/// Ventas sin Iva actual
		/// </summary>
		[DataMember(Name = "ventasSinIvaActual")]
		public decimal VentasSinIvaActual { get; set; }
		/// <summary>
		/// Devolucion con iva actual
		/// </summary>
		[DataMember(Name = "devolucionConIvaActual")]
		public decimal DevolucionConIvaActual { get; set; }
		/// <summary>
		/// Venta neta con iva actual
		/// </summary>
		[DataMember(Name = "ventaNetaConIvaActual")]
		public decimal VentaNetaConIvaActual { get; set; }
        /// <summary>
        /// Contribucion VS Total actual
        /// </summary>
        [DataMember(Name = "contribucionVsTotalActual")]
        public decimal ContribucionVsTotalActual { get; set; }

        /// <summary>
        /// Devolucion sin iva actual
        /// </summary>
        [DataMember(Name = "devolucionSinIvaActual")]
        public decimal DevolucionSinIvaActual { get; set; }

        /// <summary>
        /// Cantidad vendida anterior
        /// </summary>
        [DataMember(Name = "cantidadVendidaAnterior")]
		public int CantidadVendidaAnterior { get; set; }
		/// <summary>
		/// Ventas con iva anterior
		/// </summary>
		[DataMember(Name = "ventasConIvaAnterior")]
		public decimal VentasConIvaAnterior{ get; set; }
		/// <summary>
		/// Ventas sin iva anterior
		/// </summary>
		[DataMember(Name = "ventasSinIvaAnterior")]
		public decimal VentasSinIvaAnterior { get; set; }
		/// <summary>
		/// Devolucion con iva anterior
		/// </summary>
		[DataMember(Name = "devolucionConIvaAnterior")]
		public decimal DevolucionConIvaAnterior { get; set; }
		/// <summary>
		/// Devolucion sin iva anterior
		/// </summary>
		[DataMember(Name = "devolucionSinIvaAnterior")]
		public decimal DevolucionSinIvaAnterior { get; set; }
        /// <summary>
        /// Venta neta con iva anterior
        /// </summary>
        [DataMember(Name = "ventaNetaConIvaAnterior")]
        public decimal VentaNetaConIvaAnterior { get; set; }
        /// <summary>
        /// Contribucion VS Total anterior
        /// </summary>
        [DataMember(Name = "contribucionVsTotalAnterior")]
        public decimal ContribucionVsTotalAnterior { get; set; }


    }
}
