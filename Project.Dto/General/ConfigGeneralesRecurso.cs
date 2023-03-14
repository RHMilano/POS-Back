using Milano.BackEnd.Dto.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.General
{

    /// <summary>
    /// Clase que engloba la configuracion de un recurso susceptible en el sistema
    /// </summary>
    [DataContract]
    public class ConfigGeneralesRecurso
    {

        /// <summary>
        /// Endpoint del recurso
        /// </summary>
        [DataMember(Name = "endpoint")]
        public string Endpoint { get; set; }

        /// <summary>
        /// Descripción del recurso
        /// </summary>
        [DataMember(Name = "descripcion")]
        public string Descripcion { get; set; }

    }
}