using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.Sincronizacion
{
    /// <summary>
    /// DTO Información correspondiente al resultado de la sincronización
    /// </summary>
    [DataContract]
    public class ResultadoSincronizacion
    {
        /// <summary>
        /// Constructor por default
        /// </summary>
        public ResultadoSincronizacion()
        {
            this.UltimoIdSincronizado = -1;
            this.MensajeAsociado = "";
            this.ServidorDestino = "";
            this.IdServidorDestino = -1;
        }

        /// <summary>
        /// Ultimo Id sincronizado
        /// </summary>
        [DataMember(Name = "ultimoIdSincronizado")]
        public int UltimoIdSincronizado { get; set; }

        /// <summary>
        /// Mensaje asociado
        /// </summary>
        [DataMember(Name = "mensajeAsociado")]
        public string MensajeAsociado { get; set; }

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

    }
}
