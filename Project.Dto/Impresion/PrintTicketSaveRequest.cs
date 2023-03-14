using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Project.Dto.General
{ 
    /// <summary>
    /// Clase para guardar los tickets generados
    /// </summary>
    [DataContract]
  public class PrintTicketSaveRequest
    {
        /// <summary>
        /// Folio de la operacion 
        /// </summary>
        [DataMember(Name = "folioOperacion")]
        public string FolioOperacion { get; set; }
        /// <summary>
        /// Codigo de la tienda donde se genero el ticket
        /// </summary>
        [DataMember(Name = "codigoTienda")]
        public int CodigoTienda { get; set; }
        /// <summary>
        /// Codigo de la caja en que se genero el ticket
        /// </summary>
        [DataMember(Name = "codigoCaja")]
        public int CodigoCaja { get; set; }
        /// <summary>
        /// Tipo de ticket 
        /// </summary>
        [DataMember(Name = "tipoTicket")]
        public int TipoTicket { get; set; }
        /// <summary>
        /// Cuerpo del ticket 
        /// </summary>
        [DataMember(Name = "cuerpo")]
        public string Cuerpo { get; set; }
        /// <summary>
        /// Fecha de creacion del ticket
        /// </summary>
        [DataMember(Name = "fechaOperacion")]
        public string FechaOperacion { get; set; }

    }
}
