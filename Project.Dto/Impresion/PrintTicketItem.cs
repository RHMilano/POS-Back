using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.General
{
    /// <summary>
    /// Clase que representa un articulo para imprimir
    /// </summary>
    [DataContract]
    public class PrintTicketItem
    {
        /// <summary>
        /// SKU de item
        /// </summary>
        [DataMember(Name = "sku")]
        public string Sku { get; set; }
        /// <summary>
        /// Cantidad de articulos
        /// </summary>
        [DataMember(Name = "cantidad")]
        public int Cantidad { get; set; }
        /// <summary>
        /// Número secuencia en el Ticket
        /// </summary>
        [DataMember(Name = "costoUnitario")]
        public int CostoUnitario { get; set; }
        /// <summary>
        /// Importe total del articulo
        /// </summary>
        [DataMember(Name = "total")]
        public int Total { get; set; }
        /// <summary>
        /// SKU de item
        /// </summary>
        [DataMember(Name = "descripcion")]
        public string Descripcion { get; set; }
        /// <summary>
        /// Codigo de la caja en que se genera el ticket
        /// </summary>
        [DataMember(Name = "codigoCaja")]
        public int CodigoCaja { get; set; }

        /// <summary>
        ///  Codigo de la tienda en que se genera el ticket
        /// </summary>
        [DataMember(Name = "codigoTienda")]
        public int CodigoTienda { get; set; }
    }
}
