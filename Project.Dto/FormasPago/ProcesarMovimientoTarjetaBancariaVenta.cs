using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.Sales
{
    /// <summary>
	/// Procesar forma de pago con tarjeta bancaria
	/// </summary>
    /// 
    [DataContract]
    public class ProcesarMovimientoTarjetaBancariaVenta
    {
        /// <summary>
        /// Número de meses antes de ser efectuado el pago
        /// </summary>
        [DataMember(Name = "mesesFinanciamiento")]
        public int MesesFinanciamiento { get; set; }

        /// <summary>
        /// Número de parcialdiades mensuales del pago
        /// </summary>
        [DataMember(Name = "mesesParcialidades")]
        public int MesesParcialidades { get; set; }

        /// <summary>
        /// Codigo de la promoción
        /// </summary>
        [DataMember(Name = "codigoPromocion")]
        public int CodigoPromocion { get; set; }

        /// <summary>
        /// Folio de operación asociada al pago
        /// </summary>
        [DataMember(Name = "folioOperacionAsociada")]
        public string FolioOperacionAsociada { get; set; }

        /// <summary>
        /// Codigo forma de pago para el pago correspondiente
        /// </summary>
        [DataMember(Name = "codigoFormaPagoImporte")]
        public string CodigoFormaPagoImporte { get; set; }

        /// <summary>
        /// Importe venta total
        /// </summary>
        [DataMember(Name = "importeVentaTotal")]
        public decimal ImporteVentaTotal { get; set; }

        /// <summary>
        /// Estatus del movimiento
        /// </summary>
        [DataMember(Name = "estatus")]
        public string Estatus { get; set; }

        /// <summary>
        /// Secuencia del importe
        /// </summary>
        [DataMember(Name = "secuenciaFormaPagoImporte")]
        public int SecuenciaFormaPagoImporte { get; set; }

        /// <summary>
		/// Tipo de  venta: REGULAR, APARTADO
		/// </summary>
		[DataMember(Name = "clasificacionVenta")]
        public string ClasificacionVenta { get; set; }

    }
}
