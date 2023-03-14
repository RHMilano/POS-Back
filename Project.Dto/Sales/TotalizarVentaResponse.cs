using Milano.BackEnd.Dto.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.Sales
{

    /// <summary>
    /// Clase de respuesta de totalización de venta
    /// </summary>
    [DataContract]
    public class TotalizarVentaResponse
    {

        /// <summary>
        /// Folio de operación asignado
        /// </summary>
        [DataMember(Name = "folioOperacion")]
        public string FolioOperacion { get; set; }

        /* Descuentos Promocionales */

        /// <summary>
        /// Descuentos promocionales aplicados por venta
        /// </summary>
        [DataMember(Name = "descuentosPromocionalesAplicadosVenta")]
        public DescuentoPromocionalVenta[] DescuentosPromocionalesAplicadosVenta { get; set; }

        /// <summary>
        /// Descuentos promocionales posibles de aplicar por venta con restricción de forma de pago
        /// </summary>
        [DataMember(Name = "descuentosPromocionalesPosiblesVenta")]
        public DescuentoPromocionalVenta[] DescuentosPromocionalesPosiblesVenta { get; set; }

        /// <summary>
        /// Descuentos promocionales aplicados por linea de venta
        /// </summary>
        [DataMember(Name = "descuentosPromocionalesAplicadosLinea")]
        public DescuentoPromocionalVenta[] DescuentosPromocionalesAplicadosLinea { get; set; }

        /// <summary>
        /// Descuentos promocionales posibles de aplicar por linea de venta con restricción de forma de pago
        /// </summary>
        [DataMember(Name = "descuentosPromocionalesPosiblesLinea")]
        public DescuentoPromocionalVenta[] DescuentosPromocionalesPosiblesLinea { get; set; }

        /* Descuentos Promocionales */

        /// <summary>
        /// Para una venta mayorista, producto que representa la linea del ticket adicional que agregará el porcentaje adicional correspondiente a pago con vale mayorista
        /// </summary>
        [DataMember(Name = "productoPagoConValeMayorista")]
        public ProductsResponse ProductoPagoConValeMayorista { get; set; }

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

        /// <summary>
        /// Información asociada sobre la devolución
        /// </summary>
        [DataMember(Name = "informacionAsociadaDevolucion")]
        public InformacionAsociadaDevolucion InformacionAsociadaDevolucion { get; set; }

    }
}
