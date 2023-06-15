using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Linq;

namespace Milano.BackEnd.Dto.Lealtad
{
    /// <summary>
    /// Clase que ayuda a construir una peticion de para aplicar vale
    /// </summary>
    [DataContract]
    public class RegistroLealtadResponse
    {

     



        /// <summary>
        /// Indica si hubo algun error
        /// </summary>
        [DataMember(Name = "bError")]
        public bool bbError { get; set; }

        /// <summary>
        /// Mensaje de la respuesta
        /// </summary>
        [DataMember(Name = "sMensaje")]
        public string ssMensaje { get; set; }

        /// <summary>
        /// Nivel de estado
        /// </summary>
        [DataMember(Name = "sNivel")]
        public string ssNivel { get; set; }

        /// <summary>
        /// Numero telefonico del cliente
        /// </summary>
        [DataMember(Name = "bPrimeraCompra")]
        public bool bbPrimeraCompra { get; set; }

        /// <summary>
        /// Apellido paterno del cliente
        /// </summary>
        [DataMember(Name = "iCodigoCliente")]
        public int iiCodigoCliente { get; set; }

    }
}
