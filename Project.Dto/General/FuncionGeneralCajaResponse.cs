using Milano.BackEnd.Dto.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.General
{
    /// <summary>
    /// Clase de respuesta del congiguraciones generales de la caja y tienda
    /// </summary>
    [DataContract]
    public class FuncionGeneralCajaResponse
    {

        /// <summary>
        /// Código de la tienda
        /// </summary>
        [DataMember(Name = "codigoTienda")]
        public int CodigoTienda { get; set; }

        /// <summary>
        /// Código de la caja
        /// </summary>
        [DataMember(Name = "codigoCaja")]
        public int CodigoCaja { get; set; }

        /// <summary>
        /// Nombre del puerto donde se ubica la impresora de tickets
        /// </summary>
        [DataMember(Name = "puertoImpresoraTickets")]
        public string PuertoImpresoraTickets { get; set; }

        /// <summary>
        /// Ruta fisica donde se almacenaran los tickets
        /// </summary>
        [DataMember(Name = "rutaImpresoraTickets")]
        public string RutaImpresoraTickets { get; set; }

        /// <summary>
        /// URL del servicio de impresion
        /// </summary>
        [DataMember(Name = "urlImpresion")]
        public string UrlImpresion { get; set; }

        /// <summary>
        /// URL del servicio de pagos bancarios
        /// </summary>
        [DataMember(Name = "urlLecturaBancaria")]
        public string UrlLecturaBancaria { get; set; }
    }
}
