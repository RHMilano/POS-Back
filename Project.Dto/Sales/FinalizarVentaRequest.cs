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
    /// Clase para finalizar una transaccion de venta
    /// </summary>

    [DataContract]
    public class FinalizarVentaRequest
    {

        /// <summary>
        /// Folio de la venta
        /// </summary>
        [DataMember(Name = "folioVenta")]
        public string FolioVenta { get; set; }

        /// <summary>
        /// Información asociada de la cabecera de Venta
        /// </summary>
        [DataMember(Name = "cabeceraVentaAsociada")]
        public CabeceraVentaRequest cabeceraVentaRequest { get; set; }

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
        /// Código Cabecera Tipo de Transacción Venta
        /// </summary>
        [DataMember(Name = "codigoTipoCabeceraVenta")]
        public string TipoCabeceraVenta { get; set; }

        /// <summary>
        /// Información del mayorista
        /// </summary>
        [DataMember(Name = "informacionMayorista")]
        public InformacionMayorista InformacionMayorista { get; set; }

        /// <summary>
        /// informacion de folios de tarjeta
        /// </summary>
        [DataMember(Name = "informacionFoliosTarjeta")]
        public InformacionFoliosTarjeta[] InformacionFoliosTarjeta { get; set; }

        ///// <summary>
        ///// OCG;Version de actualizaccion del POS
        ///// </summary>
        ////[DataMember(Name = "versionPOS")]
        ////public string versionPOS { get; set; }

    }
}
