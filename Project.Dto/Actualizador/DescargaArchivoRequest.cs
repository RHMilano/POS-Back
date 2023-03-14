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
    public class DescargaArchivoRequest
    {
        /// <summary>
        /// url
        /// </summary>
        [DataMember(Name = "url")]
        public string Url { get; set; }

        /// <summary>
        /// Directorio Destino
        /// </summary>
        [DataMember(Name = "directorioDestino")]
        public string DirectorioDestino { get; set; }
    }
}
