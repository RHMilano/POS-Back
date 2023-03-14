using Milano.BackEnd.Dto.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{
    /// <summary>
    /// Procesar forma de pago mayorista
    /// </summary>
    [DataContract]
    public class ProcesarMovimientoMayorista
    {
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
        /// Código de mayorista
        /// </summary>
        [DataMember(Name = "codigoMayorista")]
        public int CodigoMayorista { get; set; }
        /// <summary>
        /// Código de cliente final
        /// </summary>
        [DataMember(Name = "codigoClienteFinal")]
        public int CodigoClienteFinal { get; set; }

		/// <summary>
		/// Nombre del cliente final
		/// </summary>
		[DataMember(Name = "nombreClienteFinal")]
		public string NombreClienteFinal { get; set; }



        /// <summary>
        /// Numero de vale
        /// </summary>
        [DataMember(Name = "numeroVale")]
        public int NumeroVale { get; set; }

        /// <summary>
        /// Monto financiado
        /// </summary>
        [DataMember(Name = "montoFinanciado")]
        public double MontoFinanciado { get; set; }

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
