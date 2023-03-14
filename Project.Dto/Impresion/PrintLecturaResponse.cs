using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.Impresion
{    /// <summary>
     /// Clase para obtener la configuracion de la impresora de tickets
     /// </summary>
    [DataContract]
    public class PrintLecturaResponse
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
