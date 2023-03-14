using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.Sincronizacion
{
    /// <summary>
    /// DTO Información correspondiente a periodicidad de ejecución
    /// </summary>
    [DataContract]
    public class Periodicidad
    {

        /// <summary>
        /// Periodicidad en segundos
        /// </summary>
        [DataMember(Name = "periodicidadProcesoMotorSincronizacion")]
        public int PeriodicidadProcesoMotorSincronizacion { get; set; }

        /// <summary>
        /// Periodicidad en segundos
        /// </summary>
        [DataMember(Name = "periodicidadProcesoCederApartados")]
        public int PeriodicidadProcesoCederApartados { get; set; }

    }
}
