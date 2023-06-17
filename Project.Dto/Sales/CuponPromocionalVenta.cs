using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.Sales
{
    /// <summary>
    /// Clase que representa un cupon por motor de promociones por venta
    /// </summary>
    [DataContract]
    public class CuponPromocionalVenta
    {

        /// <summary>
        /// Codigo de Tienda
        /// </summary>
        [DataMember(Name = "codigoTienda")]
        public int CodigoTienda { get; set; }

        /// <summary>
        /// Codigo de Caja
        /// </summary>
        [DataMember(Name = "codigoCaja")]
        public int CodigoCaja { get; set; }

        /// <summary>
        /// Número de transacción
        /// </summary>
        [DataMember(Name = "transaccion")]
        public int Transaccion { get; set; }

        /// <summary>
        /// Folio de Operación
        /// </summary>
        [DataMember(Name = "folioOperacion")]
        public string FolioOperacion { get; set; }

        /// <summary>
        /// Importe descuento
        /// </summary>
        [DataMember(Name = "importeDescuento")]
        public decimal ImporteDescuento { get; set; }

        /// <summary>
        /// Código de promoción aplicado
        /// </summary>
        [DataMember(Name = "codigoPromocionAplicado")]
        public int CodigoPromocionAplicado { get; set; }

        /// <summary>
        /// Fecha de creación
        /// </summary>
        [DataMember(Name = "fechaCreacion")]
        public DateTime FechaCreacion { get; set; }

        /// <summary>
        /// Fecha de cancelación
        /// </summary>
        [DataMember(Name = "fechaCancelacion")]
        public DateTime FechaCancelacion { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [DataMember(Name = "status")]
        public string Status { get; set; }

        /// <summary>
        /// Saldo
        /// </summary>
        [DataMember(Name = "saldo")]
        public decimal Saldo { get; set; }

        /// <summary>
        /// Mensaje del cupón generado
        /// </summary>
        [DataMember(Name = "mensajeCupon")]
        public string MensajeCupon { get; set; }

        /// <summary>
        /// Tipo de acumulación
        /// </summary>
        [DataMember(Name = "tipoAcumulacion")]
        public string TipoAcumulacion { get; set; }

        /// <summary>
        /// Mercancia sin IVA lealtad
        /// </summary>
        [DataMember(Name = "mercanciaSinIva")]
        public double MercanciaSinIva { get; set; }


        /// <summary>
        /// IVA Lealtad
        /// </summary>
        [DataMember(Name = "iva")]
        public double Iva { get; set; }

    }
}
