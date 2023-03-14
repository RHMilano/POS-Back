using Milano.BackEnd.Dto.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.FormasPago
{

    /// <summary>
    /// Clase que representa un pago con redencíon de cupon
    /// </summary>
    [DataContract]
    public class ProcesarMovimientoRedencionCuponRequest
    {

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
        /// Folio del Cupón Promocional
        /// </summary>
        [DataMember(Name = "folioCuponPromocional")]
        public string FolioCuponPromocional { get; set; }

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

    }
}
