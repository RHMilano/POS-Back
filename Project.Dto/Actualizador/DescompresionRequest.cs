using Milano.BackEnd.Dto.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{

    /// <summary>
    /// Dto request reporte Apartados Sin Detalle
    /// </summary>
    [DataContract]
    public class DescompresionRequest
    {
        /// <summary>
        /// Archivo ZIP origen
        /// </summary>
        [DataMember(Name = "archivoOrigen")]
        public string ArchivoOrigen { get; set; }

        /// <summary>
        /// Directorio Destino
        /// </summary>
        [DataMember(Name = "directorioDestino")]
        public string DirectorioDestino { get; set; }
    }
}
