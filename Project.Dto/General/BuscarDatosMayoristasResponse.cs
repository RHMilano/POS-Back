using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.General
{
    /// <summary>
	/// Clase de respuesta del mayorista
	/// </summary>
	[DataContract]
    public class BuscarDatosMayoristasResponse
    {

        /// <summary>
        /// Codigo del Mayorista
        /// </summary>
        [DataMember(Name = "mayoristaCode")]
        public int MayoristaCode { get; set; }

        /// <summary>
        /// Nombre del Mayorista
        /// </summary>
        [DataMember(Name = "mayoristaName")]
        public string MayoristaName { get; set; }

    }
}
