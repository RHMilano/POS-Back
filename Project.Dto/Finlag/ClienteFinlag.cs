using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.Finlag
{
    /// <summary>
    /// Clase que ayuda a construir una peticion de para aplicar vale
    /// </summary>
    [DataContract]
    public class ClienteFinlag
    {
        /// <summary>
        /// Nombre del cliente
        /// </summary>
        [DataMember(Name = "nombre")]
        public string Nombre { get; set; }

        /// <summary>
        /// Apellido paterno del cliente
        /// </summary>
        [DataMember(Name = "aPaterno")]
        public string Apaterno { get; set; }

        /// <summary>
        /// Apellido materno del cliente
        /// </summary>
        [DataMember(Name = "aMaterno")]
        public string Amaterno { get; set; }

        /// <summary>
        /// Fecha de nacimiento del cliente
        /// </summary>
        [DataMember(Name ="fechaNacimiento")]
        public string FechaNacimiento { get; set; }
    }
}
