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
   public class PrintTicketResponse
    {


        /// <summary>
        /// Folio de la operacion 
        /// </summary>
        [DataMember(Name = "folioOperacion")]
        public int FolioOperacion { get; set; }

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
        /// cuerpo del ticket
        /// </summary>
        [DataMember(Name = "cuerpo")]
        public string Cuerpo { get; set; }

    }
}
