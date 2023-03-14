using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{
    /// <summary>
    /// Reporte de ventas por SKU
    /// </summary>
    [DataContract]
    public class ReporteDevolucionesSKUResponse
    {
        /// <summary>
        /// SKU
        /// </summary>
        [DataMember(Name = "SKU")]
        public string SKU { get; set; }
        /// <summary>
        /// Descripcion
        /// </summary>
        [DataMember(Name = "Descripcion")]
        public string Descripcion { get; set; }
        /// <summary>
        /// Proveedor
        /// </summary>
        [DataMember(Name = "Proveedor")]
        public int Proveedor { get; set; }
        /// <summary>
        /// Estilo
        /// </summary>
        [DataMember(Name = "Estilo")]
        public string Estilo { get; set; }
        /// <summary>
        /// Cant
        /// </summary>
        [DataMember(Name = "Cant")]
        public int Cant { get; set; }
        /// <summary>
        /// Importe
        /// </summary>
        [DataMember(Name = "Importe")]
        public decimal Importe { get; set; }
    }
}
