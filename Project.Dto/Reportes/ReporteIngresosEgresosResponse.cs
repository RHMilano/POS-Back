using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{
    /// <summary>
    /// Reporte de Ingresos y Egresos
    /// </summary>
    [DataContract]
    public class ReporteIngresosEgresosResponse
    {
        /// <summary>
        /// Caja
        /// </summary>
        [DataMember(Name = "Caja")]
        public int Caja { get; set; }
        /// <summary>
        /// Transaccion
        /// </summary>
        [DataMember(Name = "Transaccion")]
        public string Transaccion { get; set; }
        /// <summary>
        /// Fecha
        /// </summary>
        [DataMember(Name = "Fecha")]
        public string Fecha { get; set; }
        /// <summary>
        /// Hora
        /// </summary>
        [DataMember(Name = "Hora")]
        public string Hora { get; set; }
        /// <summary>
        /// TipoTransaccion
        /// </summary>
        [DataMember(Name = "TipoTransaccion")]
        public string TipoTransaccion { get; set; }
        /// <summary>
        /// Razon
        /// </summary>
        [DataMember(Name = "Razon")]
        public string Razon { get; set; }
        /// <summary>
        /// Importe
        /// </summary>
        [DataMember(Name = "Importe")]
        public decimal Importe { get; set; }
    }
}
