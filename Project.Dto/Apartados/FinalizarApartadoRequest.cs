using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Milano.BackEnd.Dto.Sales;

namespace Milano.BackEnd.Dto
{

    /// <summary>
    /// Clase para finalizar una transaccion de apartado
    /// </summary>
    [DataContract]
    public class FinalizarApartadoRequest
    {
        /// <summary>
        /// Folio de la venta
        /// </summary>
        [DataMember(Name = "folioApartado")]
        public string FolioApartado { get; set; }
        /// <summary>
        /// Dias de vencimientos
        /// </summary>
        [DataMember(Name = "diasVencimiento")]
        public int DiasVencimiento { get; set; }
        /// <summary>
        /// Lineas de artículos correspondientes a la venta que tienen dígito verificador incorrecto
        /// </summary>
        [DataMember(Name = "lineasConDigitoVerificadorIncorrecto")]
        public LineaTicket[] LineasConDigitoVerificadorIncorrecto { get; set; }
        /// <summary>
        /// Lista de las diferentes formas de pago que se realizaron para poder finalizar la transacción
        /// </summary>
        [DataMember(Name = "formasPagoUtilizadas")]
        public FormaPagoUtilizado[] FormasPagoUtilizadas { get; set; }
        /// <summary>
        /// Código Cabecera Tipo de Transacción Apartado
        /// </summary>
        [DataMember(Name = "codigoTipoCabeceraApartado")]
        public string TipoCabeceraApartado { get; set; }
        /// <summary>
        /// Código Detalle Tipo de Transacción Apartado
        /// </summary>
        [DataMember(Name = "codigoTipoDetalleApartado")]
        public string TipoDetalleApartado { get; set; }
		/// <summary>
		/// Importe pagado
		/// </summary>
		[DataMember(Name = "importePagado")]
		public decimal ImportePagado { get; set; }

				/// <summary>
		/// Saldo
		/// </summary>
		[DataMember(Name = "saldo")]
		public decimal Saldo { get; set; }



		/// <summary>
		/// informacion de folios de tarjeta
		/// </summary>
		[DataMember(Name = "informacionFoliosTarjeta")]
		public InformacionFoliosTarjeta[] InformacionFoliosTarjeta { get; set; }



	}

}
