using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.InicioFinDia
{
    /// <summary>
    /// DTO que contiene el resultado de la validación de una caja
    /// </summary>
    [DataContract]
    public class ResultadoValidacionCaja
    {

        /// <summary>
        /// Código de Caja
        /// </summary>
        [DataMember(Name = "codigoCaja")]
        public int CodigoCaja { get; set; }

        /// <summary>
        /// Indica si la Lectura Z es válida para la caja
        /// </summary>
        [DataMember(Name = "procesoValidolecturaZ")]
        public bool ProcesoValidolecturaZ { get; set; }

        /// <summary>
        /// Indica si el Logout es válido para la caja
        /// </summary>
        [DataMember(Name = "procesoValidoLogout")]
        public bool ProcesoValidoLogout { get; set; }

        /// <summary>
        /// Indica si la sincronización de datos esta temrinada para la caja
        /// </summary>
        [DataMember(Name = "procesoValidoSincronizacionDatos")]
        public bool ProcesoValidoSincronizacionDatos { get; set; }

        /// <summary>
        /// Indica si el fin de día es permitido para esta caja
        /// </summary>
        [DataMember(Name = "procesoValidoFinDia")]
        public bool ProcesoValidoFinDia { get; set; }

        // <summary>
        /// Id del proceso de Control de Inicio de día Pendiente
        // </summary>
        [DataMember(Name = "idControlInicioFinDia")]
        public int IdControlInicioFinDia { get; set; }

    }
}
