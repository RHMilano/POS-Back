using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.Finlag
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class ConsultaMovientoPDVRequest
    {
        /// <summary>
        /// Fecha inicial
        /// </summary>
        [DataMember(Name = "idTienda")]
        public int IdTienda { get; set; }

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
