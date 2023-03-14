using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.Reportes
{
    /// <summary>
    /// Reporte de ventas por Vendedor
    /// </summary>
    [DataContract]
    public class ReporteApartadosDetalleLineaResponse
    {
        /// <summary>
        /// Sku
        /// </summary>
        [DataMember(Name = "sku")]
        public int Sku { get; set; }

        /// <summary>
        /// TotalPiezas
        /// </summary>
        [DataMember(Name = "totalPiezas")]
        public int TotalPiezas { get; set; }

        /// <summary>
        /// Total 
        /// </summary>
        [DataMember(Name = "total")]
        public decimal Total { get; set; }

        /// <summary>
        /// Descripcion Corta 
        /// </summary>
        [DataMember(Name = "descripcionCorta")]
        public String DescripcionCorta { get; set; }
    }
}
