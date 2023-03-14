using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{
    /// <summary>
    /// Reporte de ventas por Caja
    /// </summary>
    [DataContract]
    public class ReporteVentaCajaResponse
    {
        /// <summary>
        /// Caja
        /// </summary>
        [DataMember(Name = "Caja")]
        public int Caja { get; set; }
        /// <summary>
        /// Venta Total
        /// </summary>
        [DataMember(Name = "VentaTotal")]
        public decimal VentaTotal { get; set; }
        /// <summary>
        /// Devolucion
        /// </summary>
        [DataMember(Name = "Devolucion")]
        public int Devolucion { get; set; }
        /// <summary>
        /// venta neta
        /// </summary>
        [DataMember(Name = "VentaNeta")]
        public decimal VentaNeta { get; set; }
        /// <summary>
        /// Numero Transacciones
        /// </summary>
        [DataMember(Name = "NumTran")]
        public int NumTran { get; set; }
        /// <summary>
        /// tickets Promocion
        /// </summary>
        [DataMember(Name = "TickProm")]
        public int TickProm { get; set; }
    }
}
