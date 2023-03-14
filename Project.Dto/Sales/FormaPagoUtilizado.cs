using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Milano.BackEnd.Dto.General;

namespace Milano.BackEnd.Dto.Sales
{

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class FormaPagoUtilizado
    {

        /* Información de Pagos y Cambios */

        /// <summary>
        /// Importe del pago que debe ser procesado en moneda nacional
        /// </summary>
        [DataMember(Name = "importeMonedaNacional")]
        public decimal ImporteMonedaNacional { get; set; }

        /// <summary>
        /// Importe correspondiente al cambio en la operación en moneda nacional
        /// </summary>
        [DataMember(Name = "importeCambioMonedaNacional")]
        public decimal ImporteCambioMonedaNacional { get; set; }

        /// <summary>
        /// Importe correspondiente al remanente o excedente de cambio que no será entregado al cliente
        /// </summary>
        [DataMember(Name = "importeCambioExcedenteMonedaNacional")]
        public decimal ImporteCambioExcedenteMonedaNacional { get; set; }

        /* Información de Pagos y Cambios */

        /* Información de Códigos de Pagos y Cambios */

        /// <summary>
        /// Codigo forma de pago del importe
        /// </summary>
        [DataMember(Name = "codigoFormaPagoImporte")]
        public string CodigoFormaPagoImporte { get; set; }

        /// <summary>
        /// Codigo forma de pago del cambio
        /// </summary>
        [DataMember(Name = "codigoFormaPagoImporteCambio")]
        public string CodigoFormaPagoImporteCambio { get; set; }

        /// <summary>
        /// Codigo tipo transacción de cambio excedente
        /// </summary>
        [DataMember(Name = "codigoTipoTransaccionImporteCambioExcedente")]
        public string CodigoTipoTransaccionImporteCambioExcedente { get; set; }

        /* Información de Códigos de Pagos y Cambios */

        /* Información de Secuencias */

        /// <summary>
        /// Secuencia del importe
        /// </summary>
        [DataMember(Name = "secuenciaFormaPagoImporte")]
        public int SecuenciaFormaPagoImporte { get; set; }

        /// <summary>
        /// Secuencia del cambio
        /// </summary>
        [DataMember(Name = "secuenciaFormaPagoImporteCambio")]
        public int SecuenciaFormaPagoImporteCambio { get; set; }

        /// <summary>
        /// Secuencia del cambio excedente
        /// </summary>
        [DataMember(Name = "secuenciaFormaPagoImporteCambioExcedente")]
        public int SecuenciaFormaPagoImporteCambioExcedente { get; set; }

        /* Información de Secuencias */

        /// <summary>
        /// Objeto que alberga la información de la moneda extranjera en caso de aplicar
        /// </summary>
        [DataMember(Name = "informacionTipoCambio")]
        public InformacionTipoCambio InformacionTipoCambio { get; set; }

        /// <summary>
        /// Estatus del movimiento de la forma de pago
        /// </summary>
        [DataMember(Name = "estatus")]
        public string Estatus { get; set; }

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
        /// Numero de autorizacion para el cobro con dolares (Punto Clave)
        /// </summary>
        [DataMember(Name = "autorizacion")]
        public string Autorizacion { get; set; }

    }
}
