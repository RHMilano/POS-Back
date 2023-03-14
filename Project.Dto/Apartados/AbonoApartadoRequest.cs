using Milano.BackEnd.Dto.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{
    /// <summary>
    /// DTO abono apartado
    /// </summary>
    [DataContract]
    public class AbonoApartadoRequest
    {

        /// <summary>
        /// Folio del apartado
        /// </summary>
        [DataMember(Name = "folioApartado")]
        public string FolioApartado { get; set; }

        /// <summary>
        /// Bandera del apartado , esta liquidado 
        /// </summary>
        [DataMember(Name = "apartadoLiquidado")]
        public bool ApartadoLiquidado { get; set; }

        /// <summary>
        /// Lista de las diferentes formas de pago que se realizaron para poder finalizar la transacción
        /// </summary>
        [DataMember(Name = "formasPagoUtilizadas")]
        public FormaPagoUtilizado[] FormasPagoUtilizadas { get; set; }

        /// <summary>
        /// Código Detalle Tipo apartado
        /// </summary>
        [DataMember(Name = "codigoTipoDetalleApartado")]
        public string TipoDetalleApartado { get; set; }

        /// <summary>
        /// Importe pagado
        /// </summary>
        [DataMember(Name = "importePagado")]
        public decimal ImportePagado { get; set; }

        /// <summary>
        /// Importe cambio
        /// </summary>
        [DataMember(Name = "importeCambio")]
        public decimal ImporteCambio { get; set; }

        /// <summary>
        /// Saldo restante
        /// </summary>
        [DataMember(Name = "saldo")]
        public decimal Saldo { get; set; }

    }

}
