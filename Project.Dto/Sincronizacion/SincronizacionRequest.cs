using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.Sincronizacion
{
    /// <summary>
    /// DTO Petición para sincronizar un conjunto de datos
    /// </summary>
    [DataContract]
    public class SincronizacionRequest
    {

        /// <summary>
        /// Indica si debe detenerse en un error de este tipo
        /// </summary>
        [DataMember(Name = "debeDetenerEnCasoNoEncontrados")]
        public int DebeDetenerEnCasoNoEncontrados { get; set; }

        /// <summary>
        /// Indica si debe ignorarse un error de este tipo
        /// </summary>
        [DataMember(Name = "debeIgnorarLlaveDuplicada")]
        public int DebeIgnorarLlaveDuplicada { get; set; }

        /// <summary>
        /// Servidor destino de la sincronización
        /// </summary>
        [DataMember(Name = "servidorDestino")]
        public string ServidorDestino { get; set; }

        /// <summary>
        /// Id servidor destino de la sincronización
        /// </summary>
        [DataMember(Name = "idServidorDestino")]
        public int IdServidorDestino { get; set; }

        /// <summary>
        ///Código de tienda origen
        /// </summary>
        [DataMember(Name = "codigoTiendaOrigen")]
        public int CodigoTiendaOrigen { get; set; }

        /// <summary>
        ///Código de caja origen
        /// </summary>
        [DataMember(Name = "codigoCajaOrigen")]
        public int CodigoCajaOrigen { get; set; }

        /// <summary>
        /// Id mínimo del bloque de sentencias SQL
        /// </summary>
        [DataMember(Name = "idSincronizacionMinimoBloque")]
        public int IdSincronizacionMinimoBloque { get; set; }

        /// <summary>
        /// Sentencias SQL incluidas
        /// </summary>
        [DataMember(Name = "sentenciasSQL")]
        public SentenciaSQL[] SentenciasSQL { get; set; }

    }
}
