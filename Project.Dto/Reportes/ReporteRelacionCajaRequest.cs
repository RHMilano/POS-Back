using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.Reportes
{
    /// <summary>
    /// DTO Reporte de Relaciones de Caja
    /// </summary>
    [DataContract]
    public class ReporteRelacionCajaRequest
    {

        /// <summary>
        /// Fecha inicial
        /// </summary>
        [DataMember(Name = "fechaInicial")]
        public string FechaInicial { get; set; }

        /// <summary>
        /// Fecha final
        /// </summary>
        [DataMember(Name = "fechaFinal")]
        public string FechaFinal { get; set; }

        /// <summary>
        /// Numero de pagina 
        /// </summary>
        [DataMember(Name = "numeroPagina")]
        public int NumeroPagina { get; set; }

        /// <summary>
        /// Registros por pagina
        /// </summary>
        [DataMember(Name = "registrosPorPagina")]
        public int RegistrosPorPagina { get; set; }

    }
}
