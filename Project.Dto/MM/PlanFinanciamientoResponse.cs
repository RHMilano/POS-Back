using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.MM
{
    /// <summary>
    /// Plan de financiamineto para las compras por tarjeta MM
    /// </summary>
    [DataContract]
    public class PlanFinanciamientoResponse
    {
        /// <summary>
        /// Identificador del plan
        /// </summary>
        [DataMember(Name = "id")]
        public int Id { get; set; }

        /// <summary>
        /// Descripción
        /// </summary>
        [DataMember(Name = "descripcion")]
        public string Descripcion { get; set; }

        /// <summary>
        /// Periodo
        /// </summary>
        [DataMember(Name = "periodo")]
        public int Periodo { get; set; }
    }
}
