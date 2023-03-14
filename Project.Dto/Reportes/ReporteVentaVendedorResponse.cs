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
    public class ReporteVentaVendedorResponse
    {
        /// <summary>
        /// Vendedor
        /// </summary>
        [DataMember(Name = "NumeroVendedor")]
        public string NumeroVendedor { get; set; }
        /// <summary>
        /// Vendedor
        /// </summary>
        [DataMember(Name = "NombreVendedor")]
        public string NombreVendedor { get; set; }
        /// <summary>
        /// ventas_brutas
        /// </summary>
        [DataMember(Name = "VentasBrutas")]
        public decimal VentasBrutas { get; set; }
        /// <summary>
        /// devoluciones
        /// </summary>
        [DataMember(Name = "Devoluciones")]
        public int Devoluciones { get; set; }
        /// <summary>
        /// ventas_brutas
        /// </summary>
        [DataMember(Name = "VentasNetas")]
        public decimal VentasNetas { get; set; }
        /// <summary>
        /// numPzas
        /// </summary>
        [DataMember(Name = "NumPzas")]
        public int NumPzas { get; set; }
        /// <summary>
        /// numTicks
        /// </summary>
        [DataMember(Name = "NumTransacciones")]
        public int NumTransacciones { get; set; }
        /// <summary>
        /// ppp
        /// </summary>
        [DataMember(Name = "PPP")]
        public int PPP { get; set; }
        /// <summary>
        /// indiceVta
        /// </summary>
        [DataMember(Name = "IndiceVta")]
        public int IndiceVta { get; set; }
        /// <summary>
        /// tickProm
        /// </summary>
        [DataMember(Name = "TickProm")]
        public int TickProm { get; set; }
    }
}
