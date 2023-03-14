using Milano.BackEnd.Dto.Reportes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{
    /// <summary>
    /// Reporte de ventas por Vendedor
    /// </summary>
    [DataContract]
    public class ReporteApartadosDetalleResponse
    {
        /// <summary>
        /// Folio Apartado
        /// </summary>
        [DataMember(Name = "FolioApartado")]
        public string FolioApartado { get; set; }
        /// <summary>
        /// Monto Apartado
        /// </summary>
        [DataMember(Name = "ImporteApartado")]
        public decimal ImporteApartado { get; set; }
        /// <summary>
        /// Saldo
        /// </summary>
        [DataMember(Name = "Saldo")]
        public decimal Saldo { get; set; }
        /// <summary>
        /// Apertura
        /// </summary>
        [DataMember(Name = "FechaApertura")]
        public string FechaApertura { get; set; }
        /// <summary>
        /// Estatus
        /// </summary>
        [DataMember(Name = "Estatus")]
        public string Estatus { get; set; }
        /// <summary>
        /// Fecha de Vencimiento
        /// </summary>
        [DataMember(Name = "FechaVencimiento")]
        public string FechaVencimiento { get; set; }
        /// <summary>
        /// Numero Telefono
        /// </summary>
        [DataMember(Name = "NumTelefono")]
        public string NumTelefono { get; set; }
        /// <summary>
        /// Fecha 
        /// </summary>
        [DataMember(Name = "Fecha")]
        public string Fecha { get; set; }
        /// <summary>
        /// Fecha 
        /// </summary>
        [DataMember(Name = "Monto")]
        public decimal Monto { get; set; }
        /// <summary>
        /// Fecha 
        /// </summary>
        [DataMember(Name = "ReporteApartadosDetalleLineaResponse")]
        public ReporteApartadosDetalleLineaResponse[] ReporteApartadosDetalleLineaResponse { get; set; }
    }
}
