using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.Impresion
{
    /// <summary>
    /// Clase para obtener los datos para imprimir el ticket de egresos
    /// </summary>
    [DataContract]
    public class PrintTicketEgresosResponse
    {
        /// <summary>
        /// Formato de la linea
        /// </summary>
        [DataMember(Name = "formato")]
        public string Formato { get; set; }

        /// <summary>
        /// Texto a imprimir
        /// </summary>
        [DataMember(Name = "Texto")]
        public string Texto { get; set; }
    }
}
