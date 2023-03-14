using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.Sincronizacion
{
    /// <summary>
    /// DTO Información correspondiente a información del sincronizador
    /// </summary>
    [DataContract]
    public class InformacionSincronizador
    {

        /// <summary>
        /// Constructor por default
        /// </summary>
        public InformacionSincronizador()
        {
            this.CodigoCajaOrigen = -1;
            this.IdDestinoSiguienteProcesar = -1;
        }

        /// <summary>
        /// Codigo de la caja origen
        /// </summary>
        [DataMember(Name = "codigoCajaOrigen")]
        public int CodigoCajaOrigen { get; set; }

        /// <summary>
        /// id del siguiente destino que deberá procesarse
        /// </summary>
        [DataMember(Name = "idDestinoSiguienteProcesar")]
        public int IdDestinoSiguienteProcesar { get; set; }

    }
}
