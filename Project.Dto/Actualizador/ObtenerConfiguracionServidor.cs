using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.Actualizador
{
    /// <summary>
    /// DTO Configuración Servidor Actualizaciones
    /// </summary>
    [DataContract]
    public class ObtenerConfiguracionServidor
    {
        /// <summary>
        /// DNS Servidor de actualizaciones
        /// </summary>
        [DataMember(Name = "nombreServidorActualizaciones")]
        public string NombreServidorActualizaciones { get; set; }
    }
}
