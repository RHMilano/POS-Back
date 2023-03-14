using Milano.BackEnd.Dto.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.General
{

    /// <summary>
    /// Clase que representa la lectura en Caja
    /// </summary>
    [DataContract]
    public class LecturaCaja
    {

        /// <summary>
        /// Codigo de la caja
        /// </summary>
        [DataMember(Name = "codigoCaja")]
        public int CodigoCaja { get; set; }

        /// <summary>
        /// Información asociada de las lecturas con detalle de la Forma de Pago
        /// </summary>
        [DataMember(Name = "lecturasTotales")]
        public LecturaTotalDetalleFormaPago[] LecturasTotales { get; set; }

    }
}