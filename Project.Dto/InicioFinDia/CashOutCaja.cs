using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.InicioFinDia
{

    /// <summary>
    /// DTO que contiene la información del Cash Out de una Caja
    /// </summary>
    [DataContract]
    public class CashOutCaja
    {
        /// <summary>
        /// Id del CashOut
        /// </summary>
        [DataMember(Name = "id")]
        public int Id { get; set; }

        /// <summary>
        /// Codigo de la caja
        /// </summary>
        [DataMember(Name = "codigoCaja")]
        public int CodigoCaja { get; set; }

        /// <summary>
        /// Información asociada de las lecturas Cash Out
        /// </summary>
        [DataMember(Name = "lecturasZ")]
        public LecturaZ[] LecturasZ { get; set; }

    }
}
