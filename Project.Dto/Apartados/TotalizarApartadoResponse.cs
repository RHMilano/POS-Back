using Milano.BackEnd.Dto.General;
using Milano.BackEnd.Dto.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{

    /// <summary>
    /// Clase de respuesta de totalización de venta
    /// </summary>
    [DataContract]
    public class TotalizarApartadoResponse
    {
        /// <summary>
        /// Folio de venta asignado
        /// </summary>
        [DataMember(Name = "folioOperacion")]
        public string FolioOperacion { get; set; }

        /// <summary>
        /// Descuentos promocionales por venta
        /// </summary>
        [DataMember(Name = "descuentosPromocionalesVenta")]
        public DescuentoPromocionalVenta[] DescuentosPromocionalesVenta { get; set; }


		/// <summary>
		/// Información asociada de Formas de Pago que deben estar disponibles
		/// </summary>
		[DataMember(Name = "informacionAsociadaFormasPago")]
		public ConfigGeneralesCajaTiendaFormaPago[] InformacionAsociadaFormasPago { get; set; }

		/// <summary>
		/// Información asociada de Formas de Pago Moneda Extranjera que deben estar disponibles
		/// </summary>
		[DataMember(Name = "informacionAsociadaFormasPagoMonedaExtranjera")]
		public ConfigGeneralesCajaTiendaFormaPago[] InformacionAsociadaFormasPagoMonedaExtranjera { get; set; }
	}
}