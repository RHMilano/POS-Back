using Milano.BackEnd.Dto.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.InicioFinDia
{

    /// <summary>
    /// DTO que contiene la información de la caja
    /// </summary>
    [DataContract]
    public class InformacionCajaRequest
    {

        /// <summary>
        /// Lectura de la Caja
        /// </summary>
        [DataMember(Name = "lecturaCaja")]
        public LecturaCaja LecturaCaja { get; set; }

    }
}
