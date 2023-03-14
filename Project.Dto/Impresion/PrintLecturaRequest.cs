using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.Impresion
{
    /// <summary>
    /// Clase para obtener la configuracion de la impresora de tickets
    /// </summary>
    [DataContract]
   public class PrintLecturaRequest
    {
        /// <summary>
        /// Codigo de la tienda
        /// </summary>
        [DataMember(Name = "folioCorte")]
        public string FolioCorte { get; set; }

        /// <summary>
        /// Tipo lectura x o z
        /// </summary>
        [DataMember(Name = "tipoLectura")]
        public string TipoLectura { get; set; }

        
    }
}
