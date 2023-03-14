using Milano.BackEnd.Dto.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{
    /// <summary>
    /// Dto request Backup
    /// </summary>
    [DataContract]
    public class BackupRequest
    {
        /// <summary>
        /// Directorio Origen
        /// </summary>
        [DataMember(Name = "directorioOrigen")]
        public string DirectorioOrigen { get; set; }

        /// <summary>
        /// Directorio Destino
        /// </summary>
        [DataMember(Name = "directorioDestino")]
        public string DirectorioDestino { get; set; }
    }
}
