using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Milano.BackEnd.Dto.Configuracion
{

    /// <summary>
    /// DTO de Configuración Botonera
    /// </summary>
    [DataContract]
    public class ConfiguracionBotonera
    {

        /// <summary>
        /// Configuración de botones
        /// </summary>
        [DataMember(Name = "configuracionBotones")]
        public ConfiguracionBoton[] ConfiguracionBotones { get; set; }

    }
}
