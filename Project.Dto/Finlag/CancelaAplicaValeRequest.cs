using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.Finlag
{
    /// <summary>
    /// Clase que ayuda a la petición de cancelacion de un vale
    /// </summary>
    public class CancelaAplicaValeRequest
    {
        /// <summary>
        /// El folio del vale de entrada
        /// </summary>
        [DataMember(Name = "folioVale")]
        public string FolioVale { get; set; }

        /// <summary>
        /// Folio de venta que esta asociada al vale de finlag
        /// </summary>
        [DataMember(Name = "folioVenta")]
        public string FolioVenta { get; set; }

        /// <summary>
        /// Motivo por el cual se esta realizando la cancelación
        /// </summary>
        [DataMember(Name = "motivo")]
        public string Motivo { get; set; }

        /// <summary>
        /// Nombre de usuario con el que se identificará al solicitante
        /// </summary>
        [DataMember(Name = "usuario")]
        public string usuario { get; set; }

        /// <summary>
        /// Password del solicitante del vale
        /// </summary>
        [DataMember(Name = "password")]
        public string password { get; set; }

        /// <summary>
        /// Password que solicitará al momento de realizar la cancelación
        /// </summary>
        [DataMember(Name = "passCancelacion")]
        public string PassCancelacion { get; set; }
    }
}
