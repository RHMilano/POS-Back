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
    public class ProcesarMovimientoTarjetaBancariaRetiro
    {
        /// <summary>
        /// Indica si se desea retirar
        /// </summary>
        [DataMember(Name = "retirar")]
        public bool Retirar { get; set; }

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
        /// Importe del retiro
        /// </summary>
        [DataMember(Name = "importeCashBack")]
        public decimal ImporteCashBack { get; set; }

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
