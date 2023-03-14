using Milano.BackEnd.Dto.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.MM
{
    /// <summary>
    /// DTO Información correspondiente a la finalización de un pago TCMM
    /// </summary>
    [DataContract]
    public class FinalizarPagoTCMMRequest
    {

        /// <summary>
        /// Número de tarjeta TCMM
        /// </summary>
        [DataMember(Name = "numeroTarjeta")]
        public string NumeroTarjeta { get; set; }

        /// <summary>
        /// Código Cabecera Tipo de Transacción Pago TCMM
        /// </summary>
        [DataMember(Name = "codigoTipoCabeceraTCMM")]
        public string CodigoTipoCabeceraTCMM { get; set; }

        /// <summary>
        /// Código Detalle Tipo de Transacción Pago TCMM
        /// </summary>
        [DataMember(Name = "codigoTipoDetalleTCMM")]
        public string CodigoTipoDetalleTCMM { get; set; }

        /// <summary>
        /// Total pago con impuestos
        /// </summary>
        [DataMember(Name = "importePagoNeto")]
        public decimal ImportePagoNeto { get; set; }

        /// <summary>
        /// Lista de las diferentes formas de pago que se realizaron para poder finalizar la transacción
        /// </summary>
        [DataMember(Name = "formasPagoUtilizadas")]
        public FormaPagoUtilizado[] FormasPagoUtilizadas { get; set; }

    }
}
