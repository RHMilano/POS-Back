using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.Impresion
{
    /// <summary>
    /// Clase para obtener la configuracion de la impresora de tickets
    /// </summary>
    [DataContract]
  public class PrinterConfigResponse
    {
        /// <summary>
        /// Nombre de la impresion 
        /// </summary>
        [DataMember(Name = "nombreImpresora")]
        public string NombreImpresora { get; set; }
        /// <summary>
        /// Url para la configuracion de impresion 
        /// </summary>
        [DataMember(Name = "urlImpresion")]
        public string UrlImpresion { get; set; }

        /// <summary>
        /// Codigo Apertura Cajon
        /// </summary>
        [DataMember(Name = "codigoAperturaCajon")]
        public string CodigoAperturaCajon { get; set; }

    }
}
