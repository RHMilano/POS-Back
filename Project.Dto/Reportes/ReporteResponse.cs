using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{
    /// <summary>
    /// Clase de Respuesta de los Reportes
    /// </summary>
    [DataContract]
    public class ReporteResponse
    {
        /// <summary>
        /// Identificador del folio de venta
        /// </summary>
        [DataMember(Name = "folioVenta")]
        public string FolioVenta { get; set; }

        /// <summary>
        /// Código de Tienda
        /// </summary>
        [DataMember(Name = "codigoTienda")]
        public int CodigoTienda { get; set; }

        /// <summary>
        /// Código de Caja
        /// </summary>
        [DataMember(Name = "codigoCaja")]
        public int CodigoCaja { get; set; }

        /// <summary>
        /// Fecha
        /// </summary>
        [DataMember(Name = "fecha")]
        public string Fecha { get; set; }

        /// <summary>
        /// Total
        /// </summary>
        [DataMember(Name = "total")]
        public int Total { get; set; }

    }
}