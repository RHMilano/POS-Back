using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{
    /// <summary>
    /// Clase que representa el dígito verificador del artículo
    /// </summary>
    [DataContract]
    public class DigitoVerificadorArticulo
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DigitoVerificadorArticulo()
        {
            this.DigitoVerificadorActual = "";
            this.DigitoVerificadorCorrecto = "";
        }
        /// <summary>
        /// Digito verificador actual
        /// </summary>
        [DataMember(Name = "digitoVerificadorActual")]
        public string DigitoVerificadorActual { get; set; }

        /// <summary>
        /// Digito verificador correcto
        /// </summary>
        [DataMember(Name = "digitoVerificadorCorrecto")]
        public string DigitoVerificadorCorrecto { get; set; }

        /// <summary>
        /// Bandera que indica si tiene una inconsistencia
        /// </summary>
        [DataMember(Name = "inconsistencia")]
        public bool Inconsistencia { get; set; }

    }
}
