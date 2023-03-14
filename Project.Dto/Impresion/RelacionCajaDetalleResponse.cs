using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.Impresion
{
    /// <summary>
    /// Detalle
    /// </summary>
    [DataContract]
    public class RelacionCajaDetalleResponse
    {
        /// <summary>
        /// Id de la relacion de caja
        /// </summary>
        [DataMember(Name = "id")]
        public int Id { get; set; }
        /// <summary>
        /// Nombre de la marca
        /// </summary>
        [DataMember(Name = "descripcion")]
        public string Descripcion { get; set; }
        /// <summary>
        /// Id de la relacion de caja
        /// </summary>
        [DataMember(Name = "totalConIva")]
        public decimal TotalConIva { get; set; }
    }
}
