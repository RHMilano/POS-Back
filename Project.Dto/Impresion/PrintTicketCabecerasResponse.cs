using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.General
{
    /// <summary>
    /// Clase de articulos para imprimir trae la respuesta 
    /// </summary>
    [DataContract]
 public class PrintTicketCabecerasResponse
    {
        /// <summary>
        /// Folio de la operacion 
        /// </summary>
        [DataMember(Name = "folioVenta")]
        public string FolioVenta { get; set; }

        /// <summary>
        /// Cabecera del ticket
        /// </summary>
        [DataMember(Name = "cabecera")]
        public string Cabecera { get; set; }


        /// <summary>
        /// footer  del ticket
        /// </summary>
        [DataMember(Name = "footer")]
        public string Footer { get; set; }


        /// <summary>
        /// tipo del ticket que se imprimira
        /// </summary>
        [DataMember(Name = "tipo")]
        public string Tipo { get; set; }

        /// <summary>
        /// Nombre de la impresora en la que se imprimira el ticket
        /// </summary>
        [DataMember(Name = "nombreImpresora")]
        public string NombreImpresora { get; set; }
    }
}
