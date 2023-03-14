using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{
    /// <summary>
    /// Reporte de ventas por Hora
    /// </summary>
    [DataContract]
    public class ReporteVentasPorHoraResponse
    {
        /// <summary>
        /// Piezas
        /// </summary>
        [DataMember(Name = "Piezas")]
        public int Piezas { get; set; }
        /// <summary>
        /// Hora
        /// </summary>
        [DataMember(Name = "Fecha")]
        public string Fecha { get; set; }
        /// <summary>
        /// Hora
        /// </summary>
        [DataMember(Name = "Hora")]
        public string Hora { get; set; }
        /// <summary>
        /// Venta
        /// </summary>
        [DataMember(Name = "Venta")]
        public decimal Venta { get; set; }
        /// <summary>
        /// Tickets
        /// </summary>
        [DataMember(Name = "NumTransacciones")]
        public int NumTransacciones { get; set; }
        /// <summary>
        /// tickProm
        /// </summary>
        [DataMember(Name = "TickProm")]
        public decimal TickProm { get; set; }
        /// <summary>
        /// ppp
        /// </summary>
        [DataMember(Name = "PPP")]
        public decimal PPP { get; set; }
        /// <summary>
        /// indiceVta
        /// </summary>
        [DataMember(Name = "IndiceVta")]
        public decimal IndiceVta { get; set; }
    }
}
