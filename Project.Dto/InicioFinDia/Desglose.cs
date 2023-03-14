using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.InicioFinDia
{

    /// <summary>
    /// DTO que contiene la información de un Desglose
    /// </summary>
    [DataContract]
    public class Desglose
    {

        /// <summary>
        /// Descripciópn del Desglose
        /// </summary>
        [DataMember(Name = "descripcion")]
        public string Descripcion { get; set; }

        /// <summary>
        /// Total con IVA del Desglose
        /// </summary>
        [DataMember(Name = "totalConIVA")]
        public decimal TotalConIVA { get; set; }

        /// <summary>
        /// Información asociada al Desglose Segundo Nivel
        /// </summary>
        [DataMember(Name = "detalleDesglose")]
        public Desglose[] DetalleDesglose { get; set; }

    }
}
