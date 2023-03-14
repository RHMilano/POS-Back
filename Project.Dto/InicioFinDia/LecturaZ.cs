using Milano.BackEnd.Dto.Configuracion;
using Milano.BackEnd.Dto.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.InicioFinDia
{

    /// <summary>
    /// Clase que representa una lecturaZ en Cash Out
    /// </summary>
    [DataContract]
    public class LecturaZ
    {

        /// <summary>
        /// Folio del Corte Z
        /// </summary>
        [DataMember(Name = "folioCorte")]
        public string FolioCorte { get; set; }

        /// <summary>
        /// Folio del Corte Z
        /// </summary>
        [DataMember(Name = "detallesLecturaFormaPago")]
        public DetalleLecturaFormaPago[] DetallesLecturaFormaPago { get; set; }

    }
}