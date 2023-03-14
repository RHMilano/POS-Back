using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Milano.BackEnd.Dto.BBVAv2
{
    /// <summary>
    /// Clase para lectura de tarjeta y configuración de MSI juntas
    /// </summary>
    [DataContract]
    public class CardMSIConfig
    {
        /// <summary>
        /// Instancia datos leídos de tarjeta
        /// </summary>
        [DataMember(Name = "card")]
        public Card Card { get; set; }


        /// <summary>
        /// Instancia Configuración de MSI
        /// </summary>
        [DataMember(Name = "configMSI")]
        public ConfigMSI ConfigMSI { get; set; }


        /// <summary>
        /// Listado de meses sin intereses
        /// </summary>
        [DataMember(Name = "msiList")]
        public MsiItem[] MsiList { get; set; }

    }
}
