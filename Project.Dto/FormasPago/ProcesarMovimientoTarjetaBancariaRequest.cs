using Milano.BackEnd.Dto.BBVAv2;
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
    public class ProcesarMovimientoTarjetaBancariaRequest
    {
        /// <summary>
        /// Número de meses antes de ser efectuado el pago
        /// </summary>
        [DataMember(Name = "venta")]
        public ProcesarMovimientoTarjetaBancariaVenta Venta { get; set; }

        /// <summary>
        /// Número de meses antes de ser efectuado el pago
        /// </summary>
        [DataMember(Name = "retiro")]
        public ProcesarMovimientoTarjetaBancariaRetiro Retiro { get; set; }

        /// <summary>
        /// Número de meses antes de ser efectuado el pago
        /// </summary>
        [DataMember(Name = "puntos")]
        public ProcesarMovimientoTarjetaBancariaPuntos Puntos { get; set; }

        /// <summary>
        /// Descuentos Promocionales Aplicados por Venta para esta Forma de Pago
        /// </summary>
        [DataMember(Name = "descuentosPromocionalesPorVentaAplicados")]
        public DescuentoPromocionalesAplicados DescuentosPromocionalesPorVentaAplicados { get; set; }

        /// <summary>
        /// Descuentos Promocionales Aplicados por Linea para esta Forma de Pago
        /// </summary>
        [DataMember(Name = "descuentosPromocionalesPorLineaAplicados")]
        public DescuentoPromocionalesAplicados DescuentosPromocionalesPorLineaAplicados { get; set; }


        /// <summary>
        /// Datos de la tarjeta leida por la pinpad
        /// </summary>
        [DataMember(Name = "cardData")]
        public Card card { get; set; }

        /// <summary>
        /// Datos enviados para leer la pinpad
        /// </summary>
        [DataMember(Name = "saleRequest")]
        public SaleRequestBBVA saleRequestBBVA { get; set; }
    }
}
