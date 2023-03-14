using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{
    /// <summary>
    /// Dto request reporte ventas por departamento
    /// </summary>
    [DataContract]
    public class ReporteVentaDepartamentoRequest
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
    }
}
