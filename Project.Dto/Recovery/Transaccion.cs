using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Milano.BackEnd.Dto.Recovery
{

    /// <summary>
    /// Clase que representa un registro de transacción en el POS
    /// </summary>
    [DataContract]
    public class Transaccion
    {

        /// <summary>
        /// Registro Tabla trxTransaccionesCab
        /// </summary>
        [DataMember(Name = "trxTransaccionesCab")]
        public TrxTransaccionesCab TrxTransaccionesCab { get; set; }

        /// <summary>
        /// Registro trxTransaccionesDet
        /// </summary>
        [DataMember(Name = "trxTransaccionesDet")]
        public TrxTransaccionesDet TrxTransaccionesDet { get; set; }

        /// <summary>
        /// Registro trxValoresDet
        /// </summary>
        [DataMember(Name = "trxValoresDet")]
        public TrxValoresDet TrxValoresDet { get; set; }

        /// <summary>
        /// Registro venControlCajaReg
        /// </summary>
        [DataMember(Name = "venControlCajaReg")]
        public VenControlCajaReg VenControlCajaReg { get; set; }

        /// <summary>
        /// Registro venVentasCab
        /// </summary>
        [DataMember(Name = "venVentasCab")]
        public VenVentasCab VenVentasCab { get; set; }

    }
}
