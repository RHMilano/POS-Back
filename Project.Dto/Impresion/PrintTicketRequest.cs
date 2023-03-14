using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.General
{
    /// <summary>
    /// Clase de articulos para imprimir
    /// </summary>
    [DataContract]
    public class PrintTickertRequest
    {

        /// <summary>
        /// Folio de la operacion 
        /// </summary>
        [DataMember(Name = "folioOperacion")]
        public int FolioOperacion { get; set; }

        /// <summary>
        /// Codigo de la tienda
        /// </summary>
        [DataMember(Name = "codigoTienda")]
        public int CodigoTienda { get; set; }

        /// <summary>
        /// Codigo de la caja que se realizo la operacion
        /// </summary>
        [DataMember(Name = "codigoCaja")]
        public int CodigoCaja { get; set; }

    }
}
