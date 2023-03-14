using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.InicioFinDia
{
    /// <summary>
    /// DTO que representa Control Inicio y Fin de Día
    /// </summary>
    [DataContract]
    public class ControlInicioFinDeDia
    {

        /// <summary>
        /// Fecha de Operación
        /// </summary>
        [DataMember(Name = "fechaOperacion")]
        public DateTime FechaOperacion { get; set; }

        /// <summary>
        /// Captura de Luz Inicio de Día
        /// </summary>
        [DataMember(Name = "inicioDiaCapturaLuz")]
        public int InicioDiaCapturaLuz { get; set; }

    }
}
