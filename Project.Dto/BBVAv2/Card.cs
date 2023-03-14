using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Milano.BackEnd.Dto.BBVAv2
{
    /// <summary>
    /// Clase que devuelve el estado de la peticion de un servicio
    /// </summary>
    [DataContract]
    public class Card
    {
        
        [DataMember(Name = "saleRequest")]
        public SaleRequestBBVA SaleRequest { get; set; }

        /// <summary>
        /// Emisor (Banco)
        /// </summary>
        [DataMember(Name = "emisor")]
        public string Emisor { get; set; }

        /// <summary>
        /// pin
        /// </summary>
        [DataMember(Name = "pin")]
        public string Pin { get; set; }

        /// <summary>
        /// pan
        /// </summary>
        [DataMember(Name = "pan")]
        public string Pan { get; set; }

        /// <summary>
        /// Producto
        /// </summary>
        [DataMember(Name = "producto")]
        public string Producto { get; set; }

        /// <summary>
        /// TarjetaHabiente
        /// </summary>
        [DataMember(Name = "tarjetaHabiente")]
        public string TarjetaHabiente { get; set; }

    }
}
