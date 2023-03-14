using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.InicioFinDia
{
    /// <summary>
    /// DTO para la captura de Luz
    /// </summary>
    [DataContract]
    public class CapturaLuzRequest
    {

        /// <summary>
        /// Valor de la Lectura
        /// </summary>
        [DataMember(Name = "valorLectura")]
        public decimal ValorLectura { get; set; }

        /// <summary>
        /// Valor de la Lectura de Inicio de día si aplica al final del día
        /// </summary>
        [DataMember(Name = "valorLecturaAdicional")]
        public decimal ValorLecturaAdicional { get; set; }

        /// <summary>
        /// OCG: Version del pos, para registro de transaccion 89
        /// </summary>
        [DataMember(Name = "versionPos")]
        public string versionPos { get; set; }

    }
}
